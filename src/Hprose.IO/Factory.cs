/**********************************************************\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: http://www.hprose.com/                 |
|                   http://www.hprose.org/                 |
|                                                          |
\**********************************************************/
/**********************************************************\
 *                                                        *
 * Factory.cs                                             *
 *                                                        *
 * Factory class for C#.                                  *
 *                                                        *
 * LastModified: Jan 10, 2019                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

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
                return () => (T)Activator.CreateInstance(typeof(T), true);
            }
        }
        public static T New() {
            return constructor();
        }
    }
}