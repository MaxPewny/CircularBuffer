namespace CircularBuffer
{
    class BufferOverflowException : System.ApplicationException // Custom Exception for full Buffer
    {
        public BufferOverflowException(string message) : base(message) { }
    }

    class BufferUnderflowException : System.ApplicationException // Custom Exception for empty Buffer
    {
        public BufferUnderflowException(string message) : base(message) { }
    }
}
