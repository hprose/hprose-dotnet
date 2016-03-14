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
 * LastModified: Feb 22, 2014                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
using System;
using System.Reflection;

namespace Hprose.Common {
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
    public class MethodNameAttribute : System.Attribute {
    	private string name;
    	public MethodNameAttribute(string name) {
    		this.name = name;
    	}
    	public string Value {
    		get { return name; }
    		set { name = value; }
    	}
    }
}