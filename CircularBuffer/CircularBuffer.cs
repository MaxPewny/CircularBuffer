using System;
using System.Collections.Generic;


namespace CircularBuffer
{
    class CircularBuffer<T> : ICircularBuffer<T>
    {
        public int Capacity => mArray.Length;
        public int Count { get; private set; } = 0;
        public bool IsEmpty => Count == 0;
        public bool IsFull => Count == mArray.Length;

        private T[] mArray;
        private object mutex = new object();

        public CircularBuffer(int pSize)
        {
            lock (mutex)
            {
                mArray = new T[pSize];
            }
        }
        
        public void Produce(T pT)
        {
            if (IsFull)
            {
                throw new BufferOverflowException("BUFFER_OVERFLOW: can't add Value because the Buffer is full");
            }
            lock (mutex)
            {
                mArray[Count] = pT;
                ++Count;
            }
            
        }

        public int ProduceAll<T2>(IEnumerable<T2> pEnumT) where T2 : T
        {
            int produceCounter = 0;
            foreach (var t in pEnumT)
            {
                try
                {
                    Produce(t);
                }
                catch (BufferOverflowException)
                {
                    return produceCounter;
                }

                ++produceCounter;
            }

            return produceCounter;
        }


        public T Consume()
        {
            if (IsEmpty)
            {
                throw new BufferUnderflowException("BUFFER_UNDERFLOW: can't consume from empty Buffer");
            }
            lock (mutex)
            {
                T t = mArray[0];
                for (int i = 1; i < Count; i++)
                {
                    mArray[i - 1] = mArray[i];
                }
                --Count;
                return t;
            }
        }

        public void ConsumeAll(Action<T> pAct)
        {
            if (IsEmpty)
            {
                throw new BufferUnderflowException("BUFFER_UNDERFLOW: can't consume from empty Buffer");
            }

            for (int i = 0; i < Count; i++)
            {
                pAct(Consume());
            }
            if (!IsEmpty)
            {
                ConsumeAll(pAct);
            }
        }

        public void Clear()
        {
            Count = 0;
        }
    }
}
