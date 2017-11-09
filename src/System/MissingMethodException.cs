/* MissingMethodException class.
 * This library is free. You can redistribute it and/or modify it.
 */
#if (Core || PORTABLE || dotNETMF) && !WINDOWS_UWP

namespace System {
    public class MissingMethodException : 
#if dotNETMF
    SystemException {
#else
    MissingMemberException {
#endif
        public MissingMethodException() : base()  {
        }

        public MissingMethodException(string message) : base(message) {
        }

        public MissingMethodException(string message, Exception inner) : base(message, inner) {
        }
    }
}
#endif