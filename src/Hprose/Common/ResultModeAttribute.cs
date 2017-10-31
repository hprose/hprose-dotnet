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
 * LastModified: Feb 21, 2014                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
using System;
using System.Reflection;

namespace Hprose.Common {
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
    public class ResultModeAttribute : System.Attribute {
    	private HproseResultMode mode;
    	public ResultModeAttribute(HproseResultMode mode) {
    		this.mode = mode;
    	}
    	public HproseResultMode Value {
    		get { return mode; }
    		set { mode = value; }
    	}
    }
}