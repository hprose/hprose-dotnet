/* IgnoreDataMemberAttribute class.
* This library is free. You can redistribute it and/or modify it.
*/

using System;

#if (dotNET10 || dotNET11 || dotNET2 || dotNETCF10 || dotNETCF20 || dotNETCF35 || dotNETCF39 || UNITY_WEBPLAYER)

namespace System.Runtime.Serialization {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class IgnoreDataMemberAttribute : Attribute {
        public IgnoreDataMemberAttribute() {
        }
    }
}

#endif