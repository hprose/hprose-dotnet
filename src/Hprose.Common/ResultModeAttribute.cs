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
 * ResultModeAttribute.cs                                 *
 *                                                        *
 * ResultMode Attribute for C#.                           *
 *                                                        *
 * LastModified: May 7, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;

namespace Hprose.Common {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ResultModeAttribute : Attribute {
        public ResultModeAttribute(ResultMode value) => Value = value;
        public ResultMode Value { get; set; }
    }
}
