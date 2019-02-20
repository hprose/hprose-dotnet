// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
