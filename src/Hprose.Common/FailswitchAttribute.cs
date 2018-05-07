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
 * FailswitchAttribute.cs                                 *
 *                                                        *
 * Failswitch Attribute for C#.                           *
 *                                                        *
 * LastModified: May 7, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;

namespace Hprose.Common {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class FailswitchAttribute : Attribute {
        public FailswitchAttribute(bool value = true) => Value = value;
        public bool Value { get; set; }
    }
}
