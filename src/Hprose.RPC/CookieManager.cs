/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  CookieManager.cs                                        |
|                                                          |
|  CookieManager for .NET 3.5 CF                           |
|                                                          |
|  LastModified: Mar 7, 2020                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/
#if NET35_CF
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

public class CookieManager: IDisposable {
    private static readonly char[] EQU = new char[] {'='};
    private static readonly char[] SEM = new char[] {';'};
    private readonly Dictionary<string, Dictionary<string, Dictionary<string, string>>> container = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
    private readonly ReaderWriterLockSlim rwlock = new ReaderWriterLockSlim();
    public CookieManager() {
    }
    public void SetCookie(string[] cookieList, string host) {
        if (cookieList == null) return;
        foreach (string cookieString in cookieList) {
            if (cookieString == "")
                continue;
            string[] cookies = cookieString.Trim().Split(SEM);
            var cookie = new Dictionary<string, string>();
            string[] value = cookies[0].Trim().Split(EQU, 2);
            cookie["name"] = value[0];
            if (value.Length == 2)
                cookie["value"] = value[1];
            else
                cookie["value"] = "";
            for (int i = 1; i < cookies.Length; ++i) {
                value = cookies[i].Trim().Split(EQU, 2);
                if (value.Length == 2)
                    cookie[value[0].ToUpper()] = value[1];
                else
                    cookie[value[0].ToUpper()] = "";
            }
            // Tomcat can return SetCookie2 with path wrapped in "
            if (cookie.ContainsKey("PATH")) {
                string path = cookie["PATH"];
                if (path[0] == '"')
                    path = path.Substring(1);
      
                if (path[^1] == '"')
                    path = path[0..^1];
                cookie["PATH"] = path;
            }
            else {
                cookie["PATH"] = "/";
            }
            if (cookie.ContainsKey("DOMAIN")) {
                cookie["DOMAIN"] = cookie["DOMAIN"].ToLower();
            }
            else {
                cookie["DOMAIN"] = host;
            }
            rwlock.EnterWriteLock();
            try {
                if (!container.ContainsKey(cookie["DOMAIN"])) {
                    container[cookie["DOMAIN"]] = new Dictionary<string, Dictionary<string, string>>();
                }
                container[cookie["DOMAIN"]][cookie["name"]] = cookie;
            }
            finally {
                rwlock.ExitWriteLock();
            }
        }
    }

    public string GetCookie(string host, string path, bool secure) {
        rwlock.EnterReadLock();
        StringBuilder cookies = new StringBuilder();
        try {
            foreach (var entry in container) {
                string domain = entry.Key;
                var cookieList = entry.Value;
                if (host.EndsWith(domain)) {
                    List<string> names = new List<string>();
                    foreach (var entry2 in cookieList) {
                        var cookie = entry2.Value;
                        if (cookie.ContainsKey("EXPIRES") && DateTime.Now > DateTime.Parse(cookie["EXPIRES"])) {
                            names.Add(entry2.Key);
                        }
                        else if (path.StartsWith(cookie["PATH"])) {
                            if (secure == cookie.ContainsKey("SECURE") && cookie["value"] != "") {
                                if (cookies.Length != 0) {
                                    cookies.Append("; ");
                                }
                                cookies.Append(cookie["name"]);
                                cookies.Append('=');
                                cookies.Append(cookie["value"]);
                            }
                        }
                    }
                    rwlock.EnterUpgradeableReadLock();
                    try {
                        foreach (var name in names) {
                            container[domain].Remove(name);
                        }
                    }
                    finally {
                        rwlock.ExitUpgradeableReadLock();
                    }
                }
            }
        }
        finally {
            rwlock.ExitReadLock();
        }
        return cookies.ToString();
    }
    private bool disposed = false;
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing) {
        if (disposed) return;
        if (disposing) {
            rwlock.Dispose();
        }
        disposed = true;
    }
}
#endif