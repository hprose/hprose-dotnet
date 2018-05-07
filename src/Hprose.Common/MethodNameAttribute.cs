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
 * MethodNameAttribute.cs                                 *
 *                                                        *
 * MethodName Attribute for C#.                           *
 *                                                        *
 * LastModified: May 7, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;

namespace Hprose.Common {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MethodNameAttribute : Attribute {
        public MethodNameAttribute(string value) => Value = value;
        public string Value { get; set; }
    }
}
