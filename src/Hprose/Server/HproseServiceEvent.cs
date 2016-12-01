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
 * LastModified: Dec 1, 2016                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !ClientOnly
using System;
using Hprose.Common;

namespace Hprose.Server {
    public delegate void BeforeInvokeEvent(string name, ref object[] args, bool byRef, HproseContext context);
    public delegate void AfterInvokeEvent(string name, ref object[] args, bool byRef, ref object result, HproseContext context);
    public delegate void SendHeaderEvent(HproseContext context);
    public delegate void SendErrorEvent(Exception e, HproseContext context);
}
#endif
