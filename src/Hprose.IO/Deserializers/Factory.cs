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
 * LastModified: Apr 29, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Linq.Expressions;

namespace Hprose.IO.Deserializers {
    public static class Factory<T> {
        private static readonly Func<T> constructor;
        static Factory() {
            try {
                constructor = Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();
            }
            catch {
                constructor =() => {
                    try {
                        return (T)Activator.CreateInstance(typeof(T));
                    }
                    catch {
                        return (T)Activator.CreateInstance(typeof(T), true);
                    }
                };
            }
        }
        public static T New() {
            return constructor();
        }
    }
}