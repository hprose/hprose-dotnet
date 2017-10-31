/* OverflowException class.
 * This library is free. You can redistribute it and/or modify it.
 */
#if dotNETMF
using System;
using System.Runtime.InteropServices;

namespace System {
    [ComVisible(true)]
    [Serializable]
    public class OverflowException : ArithmeticException {
        public OverflowException() : base("Number overflow.") {
        }

        public OverflowException(string message) : base(message) {
        }

        public OverflowException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
#endif