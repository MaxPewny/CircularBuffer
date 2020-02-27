using System;
using System.Collections.Generic;
using System.Text;

namespace CircularBuffer
{
    public interface ICircularBuffer<T>
    {
        int Capacity { get; } // max amount of Elements in Buffer
        int Count { get; } // current Elements in Buffer
        bool IsEmpty { get; } // true if Buffer is empty
        bool IsFull { get; } // true if Buffer is full

        void Produce(T pT); // Adds a Element to the Bufffer
        int ProduceAll<T2>(IEnumerable<T2> pEnumT) where T2 : T; // Adds Elements from an Enumerable to the List until all Elements are added or the Buffer is full and returns the Amount of Elements added

        T Consume(); // Returns the oldest Element in the Buffer and deletes it from the Buffer
        void ConsumeAll(Action<T> pAct); // Consumes all Elements of the Buffer

        void Clear(); // Empties the Buffer
    }
}
