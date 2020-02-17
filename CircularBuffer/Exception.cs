using System;
using System.Collections.Generic;
using System.Text;

namespace CircularBuffer
{
    class BufferOverflowException : System.ApplicationException
    {
        public BufferOverflowException(string message) : base(message) { }
    }

    class BufferUnderflowException : System.ApplicationException
    {
        public BufferUnderflowException(string message) : base(message) { }
    }
}
