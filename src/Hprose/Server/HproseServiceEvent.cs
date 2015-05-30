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
 * LastModified: May 30, 2015                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !ClientOnly
using System;
using Hprose.Common;

namespace Hprose.Server {
    public delegate void BeforeInvokeEvent(string name, object[] args, bool byRef, HproseContext context);
    public delegate void AfterInvokeEvent(string name, object[] args, bool byRef, object result, HproseContext context);
    public delegate void SendHeaderEvent(HproseContext context);
    public delegate void SendErrorEvent(Exception e, HproseContext context);
}
#endif
