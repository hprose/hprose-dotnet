/**********************************************************\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: http://www.hprose.com/                 |
|                   http://www.hprose.net/                 |
|                   http://www.hprose.org/                 |
|                                                          |
\**********************************************************/
/**********************************************************\
 *                                                        *
 * HproseServiceEvent.cs                                  *
 *                                                        *
 * hprose service event for C#.                           *
 *                                                        *
 * LastModified: Feb 27, 2014                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !ClientOnly
using System;

namespace Hprose.Server {
    public delegate void BeforeInvokeEvent(string name, object[] args, bool byRef);
    public delegate void AfterInvokeEvent(string name, object[] args, bool byRef, object result);
    public delegate void SendHeaderEvent(object context);
    public delegate void SendErrorEvent(Exception e);
}
#endif