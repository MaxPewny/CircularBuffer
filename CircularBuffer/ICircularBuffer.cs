using System;
using System.Collections.Generic;
using System.Text;

namespace CircularBuffer
{
    public interface ICircularBuffer<T>
    {
        int Capacity { get; }
        int Count { get; }
        bool IsEmpty { get; }
        bool IsFull { get; }

        void Produce(T pT);
        int ProduceAll<T2>(IEnumerable<T2> pEnumT) where T2 : T;

        T Consume();
        void ConsumeAll(Action<T> pAct);

        void Clear();
    }
}
