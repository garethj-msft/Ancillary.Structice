using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ancillary.Structice
{
    public class PriorityQueue<TKey, TValue> 
    {
        const int defaultSize = 20;
        private IComparer<TKey> comparer;
        private KeyValuePair<TKey, TValue>[] content = null;
        private int size = 0;

        public PriorityQueue() : 
            this(Comparer<TKey>.Default)
        {
        }

        public PriorityQueue(IComparer<TKey> comparer)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            this.comparer = comparer;

            Queue<int> q;
        }

        public void Enqueue(TKey key, TValue value)
        {
            if (content == null)
            {
                content = new KeyValuePair<TKey, TValue>[defaultSize];
            }
            if (content.Length == size)
            {
                ReallocateUp();
            }
            content[size] = new KeyValuePair<TKey, TValue>(key, value);
            HeapifyUp(size);
            size++;
        }

        public KeyValuePair<TKey, TValue>? Dequeue()
        {
            if (size == 0)
            {
                return null;
            }
            else if (size == 1)
            {
                size = 0;
                return content[0];
            }
            else
            {
                KeyValuePair<TKey, TValue> retVal = content[0];
                size--;
                content[0] = content[size];
                HeapifyDown(0);
                if ((content.Length / 2) - size == 1)
                {
                    ReallocateDown();
                }
                return retVal;
            }
        }

        public KeyValuePair<TKey, TValue>? Peek()
        {
            if (size == 0)
            {
                return null;
            }
            else
            {
                return content[0];
            }
        }

        public void Clear()
        {
            size = 0;
            content = null;
        }

        public int Count { get { return size; } }

        private void HeapifyDown(int location)
        {
            int leftChildLocation = LeftChild(location);
            int rightChildLocation = RightChild(location);
            int leftComparison = 0;
            int rightComparison = 0;
            while (leftChildLocation < size) 
            {
                int moveLocation = location;
                if (leftChildLocation < size && (leftComparison = comparer.Compare(content[leftChildLocation].Key, content[location].Key)) > 0)
                {
                    moveLocation = leftChildLocation;
                }
                // Be sure to compare with the move location to give priority to the most different.
                if (rightChildLocation < size && (rightComparison = comparer.Compare(content[rightChildLocation].Key, content[moveLocation].Key)) > 0)
                {
                    moveLocation = rightChildLocation;
                }

                if (moveLocation != location)
                { 
                    Swap(location, moveLocation);
                    location = moveLocation;
                }
                else
                {
                    break;
                }
                leftChildLocation = LeftChild(location);
                rightChildLocation = RightChild(location);
            }
        }

        private void HeapifyUp(int location)
        {
            int parentLocation = Parent(location);
            while (comparer.Compare(content[location].Key, content[parentLocation].Key) > 0)
            {
                Swap(location, parentLocation);
                location = parentLocation;
                parentLocation = Parent(location); 
            } 
        }

        private static int Parent(int location)
        {
            return (location - 1) / 2;
        }

        private static int LeftChild(int location)
        {
            return ((location + 1) * 2) - 1;
        }

        private static int RightChild(int location)
        {
            return ((location + 1) * 2);
        }

        private void Swap(int location, int parentLocation)
        {
            KeyValuePair<TKey, TValue> temp = content[parentLocation];
            content[parentLocation] = content[location];
            content[location] = temp;
        }

        private void ReallocateUp()
        {
            KeyValuePair<TKey, TValue>[] old = content;
            content = new KeyValuePair<TKey, TValue>[size * 2];
            old.CopyTo(content, 0);
        }
        private void ReallocateDown()
        {
            KeyValuePair<TKey, TValue>[] old = content;
            int newSize = ((size / defaultSize) + 1) * defaultSize;
            if (newSize != content.Length)
            {
                content = new KeyValuePair<TKey, TValue>[newSize];
                Array.Copy(old, content, newSize);
            }
        }

#if DEBUG
        public string Dump()
        {
            int count = 0;
            int lineCount = 0;
            int rowCount = 0;
            string dump = string.Empty;
            while (count < size)
            {
                if ((count+1) == (Math.Pow(2,rowCount)))
                {
                    if (rowCount != 0)
                    {
                        dump += Environment.NewLine;
                    }
                    rowCount++;
                    lineCount = 0;
                }
                if (lineCount != 0)
                {
                    dump += ", ";
                }
                var p = content[count];
                dump += p.Key + ":" + p.Value;
                count++;
                lineCount++;
            }
            return dump;
        }
#endif
    }
}
