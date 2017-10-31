/* FormatException class.
 * This library is free. You can redistribute it and/or modify it.
 */
#if dotNETMF
using System;
using System.Runtime.InteropServices;

namespace System {
    [ComVisible(true)]
    [Serializable]
    public class FormatException : SystemException {
        public FormatException() : base("Invalid format.") {
        }

        public FormatException(string message) : base(message) {
        }

        public FormatException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
#endif