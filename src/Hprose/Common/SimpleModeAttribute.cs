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
 * SimpleModeAttribute.cs                                 *
 *                                                        *
 * SimpleMode Attribute for C#.                           *
 *                                                        *
 * LastModified: Feb 21, 2014                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
using System;
using System.Reflection;

namespace Hprose.Common {
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
    public class SimpleModeAttribute : System.Attribute {
    	private bool simple;
    	public SimpleModeAttribute(bool simple) {
    		this.simple = simple;
    	}
    	public bool Value {
    		get { return simple; }
    		set { simple = value; }
    	}
    }
}