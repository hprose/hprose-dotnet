/* ArithmeticException class.
 * This library is free. You can redistribute it and/or modify it.
 */
#if dotNETMF
using System;
using System.Runtime.InteropServices;

namespace System {
    [ComVisible(true)]
    [Serializable]
    public class ArithmeticException : SystemException {
        public ArithmeticException() : base("Overflow or underflow in the arithmetic operation.") {
        }

        public ArithmeticException(string message) : base(message) {
        }

        public ArithmeticException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
#endif