#if !(dotNETCF10 || dotNETMF)
using System;
using System.Runtime.InteropServices;

namespace System.Numerics {

    [StructLayout(LayoutKind.Explicit)]
    internal struct DoubleUlong {
        [FieldOffset(0)]
        public double dbl;
        [FieldOffset(0)]
        public ulong uu;
    }
}
#endif
