using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public class TaskResult<T> {
        public static async Task<object> Get(Task<T> task) {
            return await task;
        }
    }
    public class TaskResult {
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
                return await cache.GetOrAdd(type, factory).Value(task);
            }
            await task;
            return null;
        }
    }
}
