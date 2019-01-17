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
 * IdempotentAttribute.cs                                 *
 *                                                        *
 * Idempotent Attribute for C#.                           *
 *                                                        *
 * LastModified: May 7, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;

namespace Hprose.RPC.Common {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class IdempotentAttribute : Attribute {
        public IdempotentAttribute(bool value = true) => Value = value;
        public bool Value { get; set; }
    }
}
