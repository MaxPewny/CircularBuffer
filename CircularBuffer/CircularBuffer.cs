using System;
using System.Collections.Generic;


namespace CircularBuffer
{
    class CircularBuffer<T> : ICircularBuffer<T>
    {
        public int Capacity => mArray.Length;
        public int Count { get; private set; } = 0; 
        public bool IsEmpty => Count == 0; // Returns true when the Amount of Elements is 0
        public bool IsFull => Count == mArray.Length; // Returns true when the Amount of Elements is at the max Amount of Elements

        private T[] mArray; // array as buffer to write the Elements to
        private object mutex = new object(); // mutex for threadsafety-lock 

        public CircularBuffer(int pSize) // Constructor that initializes the array with the given Size
        {
            mArray = new T[pSize];
        }
        
        public void Produce(T pT)
        {
            if (IsFull) // throws exeption if the buffer is already full
            {
                throw new BufferOverflowException("BUFFER_OVERFLOW: can't add Value because the Buffer is full");
            }
            lock (mutex) // locked for threadsafety 
            {
                mArray[Count] = pT; // adds element to the array
                ++Count; // increases Element Count
            }
            
        }

        public int ProduceAll<T2>(IEnumerable<T2> pEnumT) where T2 : T
        {
            int produceCounter = 0;

            foreach (var t in pEnumT) // Produces all Elements of the given Enumerable
            {
                try
                {
                    Produce(t);
                }
                catch (BufferOverflowException)
                {
                    return produceCounter; // exits Function if Buffer is full and returns count of added Elements
                }

                ++produceCounter; // increases count of added Elements
            }

            return produceCounter; // returns count of added Elements
        }


        public T Consume()
        {
            if (IsEmpty) // throws exeption if the buffer is already empty
            {
                throw new BufferUnderflowException("BUFFER_UNDERFLOW: can't consume from empty Buffer");
            }
            lock (mutex) // locked for threadsafety 
            {
                T t = mArray[0]; // stores the oldest Element in local Variable
                for (int i = 1; i < Count; i++) // moves the Elements of the array so that the new oldest Element is at first position anadf all others are placed accordingly
                {
                    mArray[i - 1] = mArray[i];
                }
                --Count; // reduces Element Count
                return t; // returns the oldest Element
            }
        }

        public void ConsumeAll(Action<T> pAct)
        {
            if (IsEmpty) // throws exeption if the buffer is already empty
            {
                throw new BufferUnderflowException("BUFFER_UNDERFLOW: can't consume from empty Buffer");
            }

            while(!IsEmpty) // Consumes all Elements and executes the given Function until Buffer is empty
            {
                pAct(Consume());
            }
            
        }

        public void Clear()
        {
            mArray = new T[mArray.Length]; // reinitializes the array 
            Count = 0; // set Element Count to 0
        }
    }
}
