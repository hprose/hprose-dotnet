/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Factory.cs                                              |
|                                                          |
|  Factory class for C#.                                   |
|                                                          |
|  LastModified: Feb 21, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Linq.Expressions;

namespace Hprose.IO {
    public static class Factory<T> {
        private static readonly Func<T> constructor = GetConstructor();
        private static Func<T> GetConstructor() {
            try {
                return Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();
            }
            catch {
#if NET35_CF
                var ctor = typeof(T).GetConstructor(new Type[0]);
                return () => (T)ctor.Invoke(new object[0]);
#else
                return () => (T)Activator.CreateInstance(typeof(T), true);
#endif
            }
        }
        public static T New() {
            return constructor();
        }
    }
}