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
 * HproseContext.cs                                       *
 *                                                        *
 * hprose context class for C#.                           *
 *                                                        *
 * LastModified: May 30, 2015                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
using System.Collections;
#if !(dotNET10 || dotNET11 || dotNETCF10)
using System.Collections.Generic;
#endif

namespace Hprose.Common {
    public class HproseContext {
#if (dotNET10 || dotNET11 || dotNETCF10)
        private static readonly Hashtable userdata = new Hashtable();
#else
        private static readonly Dictionary<string, object> userdata = new Dictionary<string, object>();
#endif
        public object this[string key] {
            get {
                if (userdata.ContainsKey(key)) {
                    return userdata[key];
                }
                return null;
            }
            set {
                userdata[key] = value;
            }
        }
        public byte GetByte(string key) {
            if (userdata.ContainsKey(key)) {
                return (byte)userdata[key];
            }
            return 0;
        }
        public short GetShort(string key) {
            if (userdata.ContainsKey(key)) {
                return (short)userdata[key];
            }
            return 0;
        }
        public int GetInt(string key) {
            if (userdata.ContainsKey(key)) {
                return (int)userdata[key];
            }
            return 0;
        }
        public long GetLong(string key) {
            if (userdata.ContainsKey(key)) {
                return (long)userdata[key];
            }
            return 0;
        }
        public float GetFloat(string key) {
            if (userdata.ContainsKey(key)) {
                return (float)userdata[key];
            }
            return 0;
        }
        public double GetDouble(string key) {
            if (userdata.ContainsKey(key)) {
                return (double)userdata[key];
            }
            return 0;
        }
        public bool GetBoolean(string key) {
            if (userdata.ContainsKey(key)) {
                return (bool)userdata[key];
            }
            return false;
        }
        public string GetString(string key) {
            if (userdata.ContainsKey(key)) {
                return (string)userdata[key];
            }
            return "";
        }
        public void GetByte(string key, byte value) {
            userdata[key] = value;
        }
        public void SetShort(string key, short value) {
            userdata[key] = value;
        }
        public void SetInt(string key, int value) {
            userdata[key] = value;
        }
        public void SetLong(string key, long value) {
            userdata[key] = value;
        }
        public void SetFloat(string key, float value) {
            userdata[key] = value;
        }
        public void SetDouble(string key, double value) {
            userdata[key] = value;
        }
        public void SetBoolean(string key, bool value) {
            userdata[key] = value;
        }
        public void SetString(string key, string value) {
            userdata[key] = value;
        }
    }
}
