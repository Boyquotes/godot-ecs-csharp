using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;

namespace GdEcs
{

    public class PriorityList<T> : List<T>
    {

        public delegate int CompareDelegate(T a, T b);

        private CompareDelegate compareFunc;

        public PriorityList(CompareDelegate compareFunc)
        {
            this.compareFunc = compareFunc;
        }

        public void AddPrioritized(T value)
        {
            int lowIdx = 0;
            int highIdx = Count - 1;
            int midIdx = lowIdx;
            while (lowIdx <= highIdx)
            {
                midIdx = (lowIdx + highIdx) / 2;
                var compared = compareFunc(this[midIdx], value);
                if (compared < 0)
                {
                    lowIdx = midIdx + 1;
                }
                else if (compared > 0)
                {
                    highIdx = midIdx - 1;
                }
                else
                {
                    Insert(midIdx, value);
                    return;
                }
            }
            midIdx = midIdx >= Count ? midIdx : midIdx + 1;
            Insert(midIdx, value);
        }

    }

}