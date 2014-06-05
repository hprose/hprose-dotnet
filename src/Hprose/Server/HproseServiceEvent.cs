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
 * HproseServiceEvent.cs                                  *
 *                                                        *
 * hprose service event for C#.                           *
 *                                                        *
 * LastModified: Mar 17, 2014                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !ClientOnly
using System;

namespace Hprose.Server {
    public delegate void BeforeInvokeEvent(string name, object[] args, bool byRef, object context);
    public delegate void AfterInvokeEvent(string name, object[] args, bool byRef, object result, object context);
    public delegate void SendHeaderEvent(object context);
    public delegate void SendErrorEvent(Exception e, object context);
}
#endif