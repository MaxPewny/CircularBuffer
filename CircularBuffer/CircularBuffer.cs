using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CircularBuffer
{
    class CircularBuffer<T> : ICircularBuffer<T>
    {
        public int Capacity => mArray.Length;
        public int Count => mCounter;
        public bool IsEmpty => mCounter == 0;
        public bool IsFull => mCounter == mArray.Length;

        private T[] mArray;
        private int mCounter = 0;

        public CircularBuffer(int pSize)
        {
            mArray = new T[pSize];
        }
        
        public void Produce(T pT)
        {
            if (IsFull)
            {
                throw new BufferOverflowException("BUFFER_OVERFLOW: can't add Value because the Buffer is full");
            }
            
            mArray[mCounter] = pT;
            ++mCounter;
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

            T t = mArray[0];
            for (int i = 1; i < mCounter; i++)
            {
                mArray[i - 1] = mArray[i];
            }
            --mCounter;
            return t;

        }

        public void ConsumeAll(Action<T> pAct)
        {
            if (IsEmpty)
            {
                throw new BufferUnderflowException("BUFFER_UNDERFLOW: can't consume from empty Buffer");
            }

            for (int i = 0; i < mCounter; i++)
            {
                pAct(Consume());
            }
        }

        public void Clear()
        {
            mCounter = 0;
        }
    }
}
