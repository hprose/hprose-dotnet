#if (dotNET10 || dotNET11 || PocketPC || Smartphone || WindowsCE) && !dotNETCF35 && !dotNETCF39 && !MONO
using System;

namespace System.IO.Compression {

    public enum CompressionMode {
        Decompress,
        Compress
    }
}
#endif