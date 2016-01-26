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
 * HproseService.cs                                       *
 *                                                        *
 * hprose service class for C#.                           *
 *                                                        *
 * LastModified: Jan 23, 2016                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !ClientOnly
using System;
using System.Collections;
#if !(dotNET10 || dotNET11 || dotNETCF10 || dotNETMF)
using System.Collections.Generic;
#endif
using System.IO;
using System.Reflection;
using Hprose.IO;
using Hprose.Common;

namespace Hprose.Server {

    public abstract class HproseService {
#if !dotNETMF
        private HproseMode mode = HproseMode.MemberMode;
#endif
        private bool debugEnabled = false;
        protected HproseMethods gMethods = null;
        public event BeforeInvokeEvent OnBeforeInvoke = null;
        public event AfterInvokeEvent OnAfterInvoke = null;
        public event SendErrorEvent OnSendError = null;
#if (dotNET10 || dotNET11 || dotNETCF10 || dotNETMF)
        private readonly ArrayList filters = new ArrayList();
#else
        private readonly List<IHproseFilter> filters = new List<IHproseFilter>();
#endif

#if !(Smartphone || dotNETMF)
        [ThreadStatic]
        private static HproseContext currentContext;

        public static HproseContext CurrentContext {
            get {
                return currentContext;
            }
        }
#endif

        public virtual HproseMethods GlobalMethods {
            get {
                if (gMethods == null) {
                    gMethods = new HproseMethods();
                }
                return gMethods;
            }
            set {
                gMethods = value;
            }
        }

 #if !dotNETMF
        public HproseMode Mode {
            get {
                return mode;
            }
            set {
                mode = value;
            }
        }
#endif

        public bool IsDebugEnabled {
            get {
                return debugEnabled;
            }
            set {
                debugEnabled = value;
            }
        }

#if dotNETMF
        [CLSCompliantAttribute(false)]
#endif
        public IHproseFilter Filter {
            get {
                if (filters.Count == 0) {
                    return null;
                }
#if (dotNET10 || dotNET11 || dotNETCF10 || dotNETMF)
                return (IHproseFilter)filters[0];
#else
                return filters[0];
#endif
            }
            set {
                if (filters.Count > 0) {
                    filters.Clear();
                }
                if (value != null) {
                    filters.Add(value);
                }
            }
        }

#if dotNETMF
        [CLSCompliantAttribute(false)]
#endif
        public void AddFilter(IHproseFilter filter) {
            filters.Add(filter);
        }

#if dotNETMF
        [CLSCompliantAttribute(false)]
#endif
        public bool RemoveFilter(IHproseFilter filter) {
#if (dotNET10 || dotNET11 || dotNETCF10 || dotNETMF)
            if (filters.Contains(filter)) {
                filters.Remove(filter);
                return true;
            }
            return false;
#else
            return filters.Remove(filter);
#endif
        }

        public void Add(string methodName, object obj, Type[] paramTypes, string aliasName) {
            GlobalMethods.AddMethod(methodName, obj, paramTypes, aliasName);
        }

        public void Add(string methodName, object obj, Type[] paramTypes, string aliasName, HproseResultMode mode) {
            GlobalMethods.AddMethod(methodName, obj, paramTypes, aliasName, mode);
        }

        public void Add(string methodName, object obj, Type[] paramTypes, string aliasName, bool simple) {
            GlobalMethods.AddMethod(methodName, obj, paramTypes, aliasName, simple);
        }

        public void Add(string methodName, object obj, Type[] paramTypes, string aliasName, HproseResultMode mode, bool simple) {
            GlobalMethods.AddMethod(methodName, obj, paramTypes, aliasName, mode, simple);
        }

        public void Add(string methodName, Type type, Type[] paramTypes, string aliasName) {
            GlobalMethods.AddMethod(methodName, type, paramTypes, aliasName);
        }

        public void Add(string methodName, Type type, Type[] paramTypes, string aliasName, HproseResultMode mode) {
            GlobalMethods.AddMethod(methodName, type, paramTypes, aliasName, mode);
        }

        public void Add(string methodName, Type type, Type[] paramTypes, string aliasName, bool simple) {
            GlobalMethods.AddMethod(methodName, type, paramTypes, aliasName, simple);
        }

        public void Add(string methodName, Type type, Type[] paramTypes, string aliasName, HproseResultMode mode, bool simple) {
            GlobalMethods.AddMethod(methodName, type, paramTypes, aliasName, mode, simple);
        }

        public void Add(string methodName, object obj, Type[] paramTypes) {
            GlobalMethods.AddMethod(methodName, obj, paramTypes);
        }

        public void Add(string methodName, object obj, Type[] paramTypes, HproseResultMode mode) {
            GlobalMethods.AddMethod(methodName, obj, paramTypes, mode);
        }

        public void Add(string methodName, object obj, Type[] paramTypes, bool simple) {
            GlobalMethods.AddMethod(methodName, obj, paramTypes, simple);
        }

        public void Add(string methodName, object obj, Type[] paramTypes, HproseResultMode mode, bool simple) {
            GlobalMethods.AddMethod(methodName, obj, paramTypes, mode, simple);
        }

        public void Add(string methodName, Type type, Type[] paramTypes) {
            GlobalMethods.AddMethod(methodName, type, paramTypes);
        }

        public void Add(string methodName, Type type, Type[] paramTypes, HproseResultMode mode) {
            GlobalMethods.AddMethod(methodName, type, paramTypes, mode);
        }

        public void Add(string methodName, Type type, Type[] paramTypes, bool simple) {
            GlobalMethods.AddMethod(methodName, type, paramTypes, simple);
        }

        public void Add(string methodName, Type type, Type[] paramTypes, HproseResultMode mode, bool simple) {
            GlobalMethods.AddMethod(methodName, type, paramTypes, mode, simple);
        }

#if !dotNETMF
        public void Add(MethodInfo method, object obj, string aliasName) {
            GlobalMethods.AddMethod(method, obj, aliasName);
        }

        public void Add(MethodInfo method, object obj, string aliasName, HproseResultMode mode) {
            GlobalMethods.AddMethod(method, obj, aliasName, mode);
        }

        public void Add(MethodInfo method, object obj, string aliasName, bool simple) {
            GlobalMethods.AddMethod(method, obj, aliasName, simple);
        }

        public void Add(MethodInfo method, object obj, string aliasName, HproseResultMode mode, bool simple) {
            GlobalMethods.AddMethod(method, obj, aliasName, mode, simple);
        }

        public void Add(string methodName, object obj, string aliasName) {
            GlobalMethods.AddMethod(methodName, obj, aliasName);
        }

        public void Add(string methodName, object obj, string aliasName, HproseResultMode mode) {
            GlobalMethods.AddMethod(methodName, obj, aliasName, mode);
        }

        public void Add(string methodName, object obj, string aliasName, bool simple) {
            GlobalMethods.AddMethod(methodName, obj, aliasName, simple);
        }

        public void Add(string methodName, object obj, string aliasName, HproseResultMode mode, bool simple) {
            GlobalMethods.AddMethod(methodName, obj, aliasName, mode, simple);
        }

        public void Add(string methodName, Type type, string aliasName) {
            GlobalMethods.AddMethod(methodName, type, aliasName);
        }

        public void Add(string methodName, Type type, string aliasName, HproseResultMode mode) {
            GlobalMethods.AddMethod(methodName, type, aliasName, mode);
        }

        public void Add(string methodName, Type type, string aliasName, bool simple) {
            GlobalMethods.AddMethod(methodName, type, aliasName, simple);
        }

        public void Add(string methodName, Type type, string aliasName, HproseResultMode mode, bool simple) {
            GlobalMethods.AddMethod(methodName, type, aliasName, mode, simple);
        }

        public void Add(string methodName, object obj) {
            GlobalMethods.AddMethod(methodName, obj);
        }

        public void Add(string methodName, object obj, HproseResultMode mode) {
            GlobalMethods.AddMethod(methodName, obj, mode);
        }

        public void Add(string methodName, object obj, bool simple) {
            GlobalMethods.AddMethod(methodName, obj, simple);
        }

        public void Add(string methodName, object obj, HproseResultMode mode, bool simple) {
            GlobalMethods.AddMethod(methodName, obj, mode, simple);
        }

        public void Add(string methodName, Type type) {
            GlobalMethods.AddMethod(methodName, type);
        }

        public void Add(string methodName, Type type, HproseResultMode mode) {
            GlobalMethods.AddMethod(methodName, type, mode);
        }

        public void Add(string methodName, Type type, bool simple) {
            GlobalMethods.AddMethod(methodName, type, simple);
        }

        public void Add(string methodName, Type type, HproseResultMode mode, bool simple) {
            GlobalMethods.AddMethod(methodName, type, mode, simple);
        }

        public void Add(string[] methodNames, object obj, string[] aliasNames) {
            GlobalMethods.AddMethods(methodNames, obj, aliasNames);
        }

        public void Add(string[] methodNames, object obj, string[] aliasNames, HproseResultMode mode) {
            GlobalMethods.AddMethods(methodNames, obj, aliasNames, mode);
        }

        public void Add(string[] methodNames, object obj, string[] aliasNames, bool simple) {
            GlobalMethods.AddMethods(methodNames, obj, aliasNames, simple);
        }

        public void Add(string[] methodNames, object obj, string[] aliasNames, HproseResultMode mode, bool simple) {
            GlobalMethods.AddMethods(methodNames, obj, aliasNames, mode, simple);
        }

        public void Add(string[] methodNames, object obj, string aliasPrefix) {
            GlobalMethods.AddMethods(methodNames, obj, aliasPrefix);
        }

        public void Add(string[] methodNames, object obj, string aliasPrefix, HproseResultMode mode) {
            GlobalMethods.AddMethods(methodNames, obj, aliasPrefix, mode);
        }

        public void Add(string[] methodNames, object obj, string aliasPrefix, bool simple) {
            GlobalMethods.AddMethods(methodNames, obj, aliasPrefix, simple);
        }

        public void Add(string[] methodNames, object obj, string aliasPrefix, HproseResultMode mode, bool simple) {
            GlobalMethods.AddMethods(methodNames, obj, aliasPrefix, mode, simple);
        }

        public void Add(string[] methodNames, object obj) {
            GlobalMethods.AddMethods(methodNames, obj);
        }

        public void Add(string[] methodNames, object obj, HproseResultMode mode) {
            GlobalMethods.AddMethods(methodNames, obj, mode);
        }

        public void Add(string[] methodNames, object obj, bool simple) {
            GlobalMethods.AddMethods(methodNames, obj, simple);
        }

        public void Add(string[] methodNames, object obj, HproseResultMode mode, bool simple) {
            GlobalMethods.AddMethods(methodNames, obj, mode, simple);
        }

        public void Add(string[] methodNames, Type type, string[] aliasNames) {
            GlobalMethods.AddMethods(methodNames, type, aliasNames);
        }

        public void Add(string[] methodNames, Type type, string[] aliasNames, HproseResultMode mode) {
            GlobalMethods.AddMethods(methodNames, type, aliasNames, mode);
        }

        public void Add(string[] methodNames, Type type, string[] aliasNames, bool simple) {
            GlobalMethods.AddMethods(methodNames, type, aliasNames, simple);
        }

        public void Add(string[] methodNames, Type type, string[] aliasNames, HproseResultMode mode, bool simple) {
            GlobalMethods.AddMethods(methodNames, type, aliasNames, mode, simple);
        }

        public void Add(string[] methodNames, Type type, string aliasPrefix) {
            GlobalMethods.AddMethods(methodNames, type, aliasPrefix);
        }

        public void Add(string[] methodNames, Type type, string aliasPrefix, HproseResultMode mode) {
            GlobalMethods.AddMethods(methodNames, type, aliasPrefix, mode);
        }

        public void Add(string[] methodNames, Type type, string aliasPrefix, bool simple) {
            GlobalMethods.AddMethods(methodNames, type, aliasPrefix, simple);
        }

        public void Add(string[] methodNames, Type type, string aliasPrefix, HproseResultMode mode, bool simple) {
            GlobalMethods.AddMethods(methodNames, type, aliasPrefix, mode, simple);
        }

        public void Add(string[] methodNames, Type type) {
            GlobalMethods.AddMethods(methodNames, type);
        }

        public void Add(string[] methodNames, Type type, HproseResultMode mode) {
            GlobalMethods.AddMethods(methodNames, type, mode);
        }

        public void Add(string[] methodNames, Type type, bool simple) {
            GlobalMethods.AddMethods(methodNames, type, simple);
        }

        public void Add(string[] methodNames, Type type, HproseResultMode mode, bool simple) {
            GlobalMethods.AddMethods(methodNames, type, mode, simple);
        }

        public void Add(object obj, Type type, string aliasPrefix) {
            GlobalMethods.AddInstanceMethods(obj, type, aliasPrefix);
        }

        public void Add(object obj, Type type, string aliasPrefix, HproseResultMode mode) {
            GlobalMethods.AddInstanceMethods(obj, type, aliasPrefix, mode);
        }

        public void Add(object obj, Type type, string aliasPrefix, bool simple) {
            GlobalMethods.AddInstanceMethods(obj, type, aliasPrefix, simple);
        }

        public void Add(object obj, Type type, string aliasPrefix, HproseResultMode mode, bool simple) {
            GlobalMethods.AddInstanceMethods(obj, type, aliasPrefix, mode, simple);
        }

        public void Add(object obj, Type type) {
            GlobalMethods.AddInstanceMethods(obj, type);
        }

        public void Add(object obj, Type type, HproseResultMode mode) {
            GlobalMethods.AddInstanceMethods(obj, type, mode);
        }

        public void Add(object obj, Type type, bool simple) {
            GlobalMethods.AddInstanceMethods(obj, type, simple);
        }

        public void Add(object obj, Type type, HproseResultMode mode, bool simple) {
            GlobalMethods.AddInstanceMethods(obj, type, mode);
        }

        public void Add(object obj, string aliasPrefix) {
            GlobalMethods.AddInstanceMethods(obj, aliasPrefix);
        }

        public void Add(object obj, string aliasPrefix, HproseResultMode mode) {
            GlobalMethods.AddInstanceMethods(obj, aliasPrefix, mode);
        }

        public void Add(object obj, string aliasPrefix, bool simple) {
            GlobalMethods.AddInstanceMethods(obj, aliasPrefix, simple);
        }

        public void Add(object obj, string aliasPrefix, HproseResultMode mode, bool simple) {
            GlobalMethods.AddInstanceMethods(obj, aliasPrefix, mode, simple);
        }

        public void Add(object obj) {
            GlobalMethods.AddInstanceMethods(obj);
        }

        public void Add(object obj, HproseResultMode mode) {
            GlobalMethods.AddInstanceMethods(obj, mode);
        }

        public void Add(object obj, bool simple) {
            GlobalMethods.AddInstanceMethods(obj, simple);
        }

        public void Add(object obj, HproseResultMode mode, bool simple) {
            GlobalMethods.AddInstanceMethods(obj, mode, simple);
        }

        public void Add(Type type, string aliasPrefix) {
            GlobalMethods.AddStaticMethods(type, aliasPrefix);
        }

        public void Add(Type type, string aliasPrefix, HproseResultMode mode) {
            GlobalMethods.AddStaticMethods(type, aliasPrefix, mode);
        }

        public void Add(Type type, string aliasPrefix, bool simple) {
            GlobalMethods.AddStaticMethods(type, aliasPrefix, simple);
        }

        public void Add(Type type, string aliasPrefix, HproseResultMode mode, bool simple) {
            GlobalMethods.AddStaticMethods(type, aliasPrefix, mode, simple);
        }

        public void Add(Type type) {
            GlobalMethods.AddStaticMethods(type);
        }

        public void Add(Type type, HproseResultMode mode) {
            GlobalMethods.AddStaticMethods(type, mode);
        }

        public void Add(Type type, bool simple) {
            GlobalMethods.AddStaticMethods(type, simple);
        }

        public void Add(Type type, HproseResultMode mode, bool simple) {
            GlobalMethods.AddStaticMethods(type, mode, simple);
        }
#endif

        public void AddMissingMethod(string methodName, object obj) {
            GlobalMethods.AddMissingMethod(methodName, obj);
        }

        public void AddMissingMethod(string methodName, object obj, HproseResultMode mode) {
            GlobalMethods.AddMissingMethod(methodName, obj, mode);
        }

        public void AddMissingMethod(string methodName, object obj, bool simple) {
            GlobalMethods.AddMissingMethod(methodName, obj, simple);
        }

        public void AddMissingMethod(string methodName, object obj, HproseResultMode mode, bool simple) {
            GlobalMethods.AddMissingMethod(methodName, obj, mode, simple);
        }

        public void AddMissingMethod(string methodName, Type type) {
            GlobalMethods.AddMissingMethod(methodName, type);
        }

        public void AddMissingMethod(string methodName, Type type, HproseResultMode mode) {
            GlobalMethods.AddMissingMethod(methodName, type, mode);
        }

        public void AddMissingMethod(string methodName, Type type, bool simple) {
            GlobalMethods.AddMissingMethod(methodName, type, simple);
        }

        public void AddMissingMethod(string methodName, Type type, HproseResultMode mode, bool simple) {
            GlobalMethods.AddMissingMethod(methodName, type, mode, simple);
        }

        private MemoryStream ResponseEnd(MemoryStream data, HproseContext context) {
            data.Position = 0;
            for (int i = 0, n = filters.Count; i < n; ++i) {
#if (dotNET10 || dotNET11 || dotNETCF10 || dotNETMF)
                IHproseFilter filter = (IHproseFilter)filters[i];
                data = filter.OutputFilter(data, context);
#else
                data = filters[i].OutputFilter(data, context);
#endif
                data.Position = 0;
            }
            return data;
        }

        protected virtual object[] FixArguments(Type[] argumentTypes, object[] arguments, int count, HproseContext context) {
            if (argumentTypes.Length != count) {
                object[] args = new object[argumentTypes.Length];
                System.Array.Copy(arguments, 0, args, 0, count);
                Type argType = argumentTypes[count];
                if (argType == typeof(HproseContext)) {
                    args[count] = context;
                }
                return args;
            }
            return arguments;
        }

#if dotNETMF
        [CLSCompliantAttribute(false)]
#endif
        protected MemoryStream SendError(Exception e, HproseContext context) {
            if (OnSendError != null) {
                OnSendError(e, context);
            }
            string error = debugEnabled ? e.ToString(): e.Message;
#if !dotNETMF
            MemoryStream data = new MemoryStream(4096);
            HproseWriter writer = new HproseWriter(data, mode, true);
#else
            MemoryStream data = new MemoryStream();
            HproseWriter writer = new HproseWriter(data, true);
#endif
            data.WriteByte(HproseTags.TagError);
            writer.WriteString(error);
            data.WriteByte(HproseTags.TagEnd);
            return ResponseEnd(data, context);
        }

#if dotNETMF
        [CLSCompliantAttribute(false)]
#endif
        protected MemoryStream DoInvoke(MemoryStream istream, HproseMethods methods, HproseContext context) {
#if !dotNETMF
            HproseReader reader = new HproseReader(istream, mode);
            MemoryStream data = new MemoryStream(4096);
#else
            HproseReader reader = new HproseReader(istream);
            MemoryStream data = new MemoryStream();
#endif
            int tag;
            do {
                reader.Reset();
                string name = reader.ReadString();
                HproseMethod remoteMethod = null;
                int count = 0;
                object[] args = null;
                object[] arguments = null;
                bool byRef = false;
                tag = reader.CheckTags((char)HproseTags.TagList + "" +
                                       (char)HproseTags.TagEnd + "" +
                                       (char)HproseTags.TagCall);
                if (tag == HproseTags.TagList) {
                    reader.Reset();
                    count = reader.ReadInt(HproseTags.TagOpenbrace);
                    if (methods != null) {
                        remoteMethod = methods.GetMethod(name, count);
                    }
                    if (remoteMethod == null) {
                        remoteMethod = GlobalMethods.GetMethod(name, count);
                    }
                    if (remoteMethod == null) {
                        arguments = reader.ReadArray(count);
                    }
                    else {
                        arguments = new object[count];
                        reader.ReadArray(remoteMethod.paramTypes, arguments, count);
                    }
                    tag = reader.CheckTags((char)HproseTags.TagTrue + "" +
                                           (char)HproseTags.TagEnd + "" +
                                           (char)HproseTags.TagCall);
                    if (tag == HproseTags.TagTrue) {
                        byRef = true;
                        tag = reader.CheckTags((char)HproseTags.TagEnd + "" +
                                               (char)HproseTags.TagCall);
                    }
                }
                else {
                    if (methods != null) {
                        remoteMethod = methods.GetMethod(name, 0);
                    }
                    if (remoteMethod == null) {
                        remoteMethod = GlobalMethods.GetMethod(name, 0);
                    }
                    arguments = new object[0];
                }
                if (OnBeforeInvoke != null) {
                    OnBeforeInvoke(name, arguments, byRef, context);
                }
                if (remoteMethod == null) {
                    args = arguments;
                }
                else {
                    args = FixArguments(remoteMethod.paramTypes, arguments, count, context);
                }
                object result;
                if (remoteMethod == null) {
                    if (methods != null) {
                        remoteMethod = methods.GetMethod("*", 2);
                    }
                    if (remoteMethod == null) {
                        remoteMethod = GlobalMethods.GetMethod("*", 2);
                    }
                    if (remoteMethod == null) {
                        throw new MissingMethodException("Can't find this method " + name);
                    }
                    result = remoteMethod.method.Invoke(remoteMethod.obj, new object[] { name, args });
                }
                else {
                    result = remoteMethod.method.Invoke(remoteMethod.obj, args);
                }
                if (byRef) {
                    Array.Copy(args, 0, arguments, 0, count);
                }
                if (OnAfterInvoke != null) {
                    OnAfterInvoke(name, arguments, byRef, result, context);
                }
                if (remoteMethod.mode == HproseResultMode.RawWithEndTag) {
                    data.Write((byte[])result, 0, ((byte[])result).Length);
                    return ResponseEnd(data, context);
                }
                else if (remoteMethod.mode == HproseResultMode.Raw) {
                    data.Write((byte[])result, 0, ((byte[])result).Length);
                }
                else {
                    data.WriteByte(HproseTags.TagResult);
                    bool simple = remoteMethod.simple;
#if !dotNETMF
                    HproseWriter writer = new HproseWriter(data, mode, simple);
#else
                    HproseWriter writer = new HproseWriter(data, simple);
#endif
                    if (remoteMethod.mode == HproseResultMode.Serialized) {
                        data.Write((byte[])result, 0, ((byte[])result).Length);
                    }
                    else {
                        writer.Serialize(result);
                    }
                    if (byRef) {
                        data.WriteByte(HproseTags.TagArgument);
                        writer.Reset();
                        writer.WriteArray(arguments);
                    }
                }
            } while (tag == HproseTags.TagCall);
            data.WriteByte(HproseTags.TagEnd);
            return ResponseEnd(data, context);
        }

#if dotNETMF
        [CLSCompliantAttribute(false)]
#endif
        protected MemoryStream DoFunctionList(HproseMethods methods, HproseContext context) {
#if dotNETMF
            ArrayList names = new ArrayList();
            foreach (object name in GlobalMethods.AllNames) {
                names.Add(name);
            }
#elif !(dotNET10 || dotNET11 || dotNETCF10)
            List<string> names = new List<string>(GlobalMethods.AllNames);
#else
            ArrayList names = new ArrayList(GlobalMethods.AllNames);
#endif
            if (methods != null) {
#if dotNETMF
                foreach (object name in methods.AllNames) {
                    names.Add(name);
                }
#else
                names.AddRange(methods.AllNames);
#endif
            }
#if !dotNETMF
            MemoryStream data = new MemoryStream(4096);
            HproseWriter writer = new HproseWriter(data, mode, true);
#else
            MemoryStream data = new MemoryStream();
            HproseWriter writer = new HproseWriter(data, true);
#endif
            data.WriteByte(HproseTags.TagFunctions);
#if !(dotNET10 || dotNET11 || dotNETCF10 || dotNETMF)
            writer.WriteList((IList<string>)names);
#else
            writer.WriteList((IList)names);
#endif
            data.WriteByte(HproseTags.TagEnd);
            return ResponseEnd(data, context);
        }

        protected void FireErrorEvent(Exception e, HproseContext context) {
            if (OnSendError != null) {
                OnSendError(e, context);
            }
        }

#if dotNETMF
        [CLSCompliantAttribute(false)]
#endif
        protected MemoryStream Handle(MemoryStream istream, HproseMethods methods, HproseContext context) {
#if !(Smartphone || dotNETMF)
            currentContext = context;
#endif
            try {
                istream.Position = 0;
                for (int i = filters.Count - 1; i >= 0; --i) {
#if (dotNET10 || dotNET11 || dotNETCF10 || dotNETMF)
                    IHproseFilter filter = (IHproseFilter)filters[i];
                    istream = filter.InputFilter(istream, context);
#else
                    istream = filters[i].InputFilter(istream, context);
#endif
                    istream.Position = 0;
                }
                switch (istream.ReadByte()) {
                    case HproseTags.TagCall:
                        return DoInvoke(istream, methods, context);
                    case HproseTags.TagEnd:
                        return DoFunctionList(methods, context);
                    default:
                        return SendError(new HproseException("Wrong Request: \r\n" + HproseHelper.ReadWrongInfo(istream)), context);
                }
            }
            catch (Exception e) {
                return SendError(e, context);
            }
#if !(Smartphone || dotNETMF)
            finally {
                currentContext = null;
            }
#endif
        }
    }
}
#endif
