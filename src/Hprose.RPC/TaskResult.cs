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
|  LastModified: Jan 29, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public static class TaskResult<T> {
        public static async Task<object> Get(Task<T> task) => await task.ConfigureAwait(false);
    }
    public static class TaskResult {
        private static readonly ConcurrentDictionary<Type, Lazy<Func<Task, Task<object>>>> cache = new ConcurrentDictionary<Type, Lazy<Func<Task, Task<object>>>>();
        private static Func<Task, Task<object>> GetFunc(Type type) {
            var resultType = type.GetGenericArguments()[0];
            var method = typeof(TaskResult<>).MakeGenericType(resultType).GetMethod("Get");
            var task = Expression.Parameter(typeof(Task), "task");
            return Expression.Lambda<Func<Task, Task<object>>>(
                Expression.Call(method, Expression.Convert(task, type)),
                task
            ).Compile();
        }
        private static readonly Func<Type, Lazy<Func<Task, Task<object>>>> factory = (type) => new Lazy<Func<Task, Task<object>>>(() => GetFunc(type));
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
