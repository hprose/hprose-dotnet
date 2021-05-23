/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  TaskResult.cs                                           |
|                                                          |
|  TaskResult class for C#.                                |
|                                                          |
|  LastModified: Mar 20, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public static class TaskResult {
        private static readonly ConcurrentDictionary<Type, Lazy<Func<Task, Task<object>>>> cache = new();
        private static Func<Task, Task<object>> GetFunc(Type type) {
            var resultType = type.GetGenericArguments()[0];
            var method = typeof(TaskResult).GetMethod(nameof(GetTask), BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(resultType);
            var task = Expression.Parameter(typeof(Task), "task");
            return Expression.Lambda<Func<Task, Task<object>>>(
                Expression.Call(method, Expression.Convert(task, type)),
                task
            ).Compile();
        }
        static async Task<object> GetTask<T>(Task<T> task) => await task.ConfigureAwait(false);
#if !NET35_CF
        private static readonly Func<Type, Lazy<Func<Task, Task<object>>>> factory = (type) => new Lazy<Func<Task, Task<object>>>(() => GetFunc(type));
#else
        private static readonly Func2<Type, Lazy<Func<Task, Task<object>>>> factory = (type) => new Lazy<Func<Task, Task<object>>>(() => GetFunc(type));
#endif
        public static async Task<object> Get<T>(Task<T> task) => await task.ConfigureAwait(false);
        public static async Task<object> Get(Task task) {
            var type = task.GetType();
            if (type.IsGenericType) {
                return await cache.GetOrAdd(type, factory).Value(task).ConfigureAwait(false);
            }
            await task.ConfigureAwait(false);
            return null;
        }
    }
}
