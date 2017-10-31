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
 * CookieManager.cs                                       *
 *                                                        *
 * cookie manager class for .NET Compact Framework.       *
 *                                                        *
 * LastModified: Jan 22, 2015                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if (PocketPC || Smartphone || WindowsCE || dotNETMF)
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
#if !dotNETMF
using System.Text.RegularExpressions;
#endif

public class CookieManager {
#if !dotNETMF
    private static readonly Regex regex = new Regex("=");
#else
    private static readonly char[] EQU = new char[] {'='};
#endif
    private static readonly char[] SEM = new char[] {';'};
    private Hashtable container = new Hashtable();
    public CookieManager() {
    }
    public void SetCookie(string[] cookieList, string host) {
        if (cookieList == null) return;
        lock (this) {
            foreach (string cookieString in cookieList) {
                if (cookieString == "")
                    continue;
                string[] cookies = cookieString.Trim().Split(SEM);
                Hashtable cookie = new Hashtable();
#if !dotNETMF
                string[] value = regex.Split(cookies[0].Trim(), 2);
#else
                string[] value = cookies[0].Trim().Split(EQU, 2);
#endif
                cookie["name"] = value[0];
                if (value.Length == 2)
                    cookie["value"] = value[1];
                else
                    cookie["value"] = "";
                for (int i = 1; i < cookies.Length; ++i) {
#if !dotNETMF
                    value = regex.Split(cookies[i].Trim(), 2);
#else
                    value = cookies[i].Trim().Split(EQU, 2);
#endif
                    if (value.Length == 2)
                        cookie[value[0].ToUpper()] = value[1];
                    else
                        cookie[value[0].ToUpper()] = "";
                }
                // Tomcat can return SetCookie2 with path wrapped in "
                if (cookie.Contains("PATH")) {
                    string path = ((string)cookie["PATH"]);
                    if (path[0] == '"')
                        path = path.Substring(1);
                    if (path[path.Length - 1] == '"')
                        path = path.Substring(0, path.Length - 1);
                    cookie["PATH"] = path;
                }
                else {
                    cookie["PATH"] = "/";
                }
#if !dotNETMF
                if (cookie.Contains("EXPIRES")) {
                    cookie["EXPIRES"] = DateTime.Parse((string)cookie["EXPIRES"]);
                }
#endif
                if (cookie.Contains("DOMAIN")) {
                    cookie["DOMAIN"] = ((string)cookie["DOMAIN"]).ToLower();
                }
                else {
                    cookie["DOMAIN"] = host;
                }
                cookie["SECURE"] = cookie.Contains("SECURE");
                if (!container.Contains(cookie["DOMAIN"])) {
                    container[cookie["DOMAIN"]] = new Hashtable();
                }
                ((Hashtable)container[cookie["DOMAIN"]])[cookie["name"]] = cookie;
            }
        }
    }

#if dotNETMF
    private static bool StartsWith(string s1, string s2) {
        return s1.Length >= s2.Length && 0 == string.Compare(s1.Substring(0, s2.Length), s2);
    }

    private static bool EndsWith(string s1, string s2) {
        return s1.Length >= s2.Length && 0 == string.Compare(s1.Substring(s1.Length - s2.Length, s2.Length), s2);
    }
#else
    private static bool StartsWith(string s1, string s2) {
        return s1.StartsWith(s2);
    }

    private static bool EndsWith(string s1, string s2) {
        return s1.EndsWith(s2);
    }
#endif

    public string GetCookie(string host, string path, bool secure) {
        lock(this) {
            StringBuilder cookies = new StringBuilder();
            foreach (DictionaryEntry entry in container) {
                string domain = (string) entry.Key;
                Hashtable cookieList = (Hashtable)entry.Value;
                if (EndsWith(host, domain)) {
                    ArrayList names = new ArrayList();
                    foreach (DictionaryEntry entry2 in cookieList) {
                        Hashtable cookie = (Hashtable)entry2.Value;
                        if (cookie.Contains("EXPIRES") && DateTime.Now > (DateTime)cookie["EXPIRES"]) {
                            names.Add(entry2.Key);
                        }
                        else if (StartsWith(path, (string)cookie["PATH"])) {
                            if (((secure && (bool)cookie["SECURE"]) || !(bool)cookie["SECURE"]) && (string)cookie["value"] != "") {
                                if (cookies.Length != 0) {
                                    cookies.Append("; ");
                                }
                                cookies.Append(cookie["name"]);
                                cookies.Append('=');
                                cookies.Append(cookie["value"]);
                            }
                        }
                    }
                    foreach (object name in names) {
                        ((Hashtable)container[domain]).Remove(name);
                    }
                }
            }
            return cookies.ToString();
        }
    }
}
#endif