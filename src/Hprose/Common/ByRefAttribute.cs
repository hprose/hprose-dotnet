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
 * ByRefAttribute.cs                                      *
 *                                                        *
 * ByRef Attribute for C#.                                *
 *                                                        *
 * LastModified: Jan 18, 2016                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
using System;
using System.Reflection;

namespace Hprose.Common {
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
    public class ByRefAttribute : System.Attribute {
    	private bool byRef;
    	public ByRefAttribute(bool byRef) {
    		this.byRef = byRef;
    	}
    	public bool Value {
    		get { return byRef; }
    		set { byRef = value; }
    	}
    }
}