// <copyright file="PriorityQueue.cs" company="Gareth Jones">
// © Gareth Jones. All rights reserved.
// </copyright>

namespace Ancillary.Structice
{
    using System;
    using System.Text;

    /// <summary>
    /// Heap-based priority queue.
    /// </summary>
    /// <typeparam name="TPriority">Type to prioritize upon.</typeparam>
    /// <typeparam name="TValue">Type contained in the queue.</typeparam>
    public class PriorityQueue<TPriority, TValue>
        where TPriority : IComparable<TPriority>
    {
        private const int DefaultSize = 20;
        private (TPriority priority, TValue value)[] content;

        /// <summary>
        /// Gets the number of entries in the queue.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Add an entry to the queue with the given priority.
        /// </summary>
        /// <param name="priority">The priority to provide.</param>
        /// <param name="value">The entry to add.</param>
        public void Enqueue(TPriority priority, TValue value)
        {
            if (this.content == null)
            {
                this.content = new (TPriority, TValue)[DefaultSize];
            }

            if (this.content.Length == this.Count)
            {
                this.ReallocateUp();
            }

            this.content[this.Count] = (priority, value);
            this.HeapifyUp(this.Count);
            this.Count++;
        }

        /// <summary>
        /// Take an entry from the queue.
        /// </summary>
        /// <returns>The entry.</returns>
        public (TPriority priority, TValue value)? Dequeue()
        {
            if (this.Count == 0)
            {
                return null;
            }
            else if (this.Count == 1)
            {
                this.Count = 0;
                return this.content[0];
            }
            else
            {
                (TPriority, TValue) retVal = this.content[0];
                this.Count--;
                this.content[0] = this.content[this.Count];
                this.HeapifyDown(0);
                if ((this.content.Length / 2) - this.Count == 1)
                {
                    this.ReallocateDown();
                }

                return retVal;
            }
        }

        /// <summary>
        /// Look at the next entry on the queue without removing it.
        /// </summary>
        /// <returns>The entry.</returns>
        public (TPriority priority, TValue value)? Peek()
        {
            if (this.Count == 0)
            {
                return null;
            }
            else
            {
                return this.content[0];
            }
        }

        /// <summary>
        /// Clear the queue.
        /// </summary>
        public void Clear()
        {
            this.Count = 0;
            this.content = null;
        }

#if DEBUG
        /// <summary>
        /// Write a verbose string representation of the queue.
        /// </summary>
        /// <returns>The representation.</returns>
        public string Dump()
        {
            int count = 0;
            int lineCount = 0;
            int rowCount = 0;
            var dump = new StringBuilder();
            while (count < this.Count)
            {
                if (count + 1 == (int)Math.Pow(2, rowCount))
                {
                    if (rowCount != 0)
                    {
                        dump.AppendLine();
                    }

                    rowCount++;
                    lineCount = 0;
                }

                if (lineCount != 0)
                {
                    dump.Append(", ");
                }

                var (priority, value) = this.content[count];
                dump.Append($"{priority}:{value}");
                count++;
                lineCount++;
            }

            return dump.ToString();
        }
#endif

        private static int Parent(int location) => (location - 1) / 2;

        private static int LeftChild(int location) => ((location + 1) * 2) - 1;

        private static int RightChild(int location) => (location + 1) * 2;

        private void HeapifyDown(int location)
        {
            int leftChildLocation = LeftChild(location);
            int rightChildLocation = RightChild(location);
            while (leftChildLocation < this.Count)
            {
                int moveLocation = location;
                if (leftChildLocation < this.Count && this.content[leftChildLocation].priority.CompareTo(this.content[location].priority) > 0)
                {
                    moveLocation = leftChildLocation;
                }

                // Be sure to compare with the move location to give priority to the most different.
                if (rightChildLocation < this.Count && this.content[rightChildLocation].priority.CompareTo(this.content[moveLocation].priority) > 0)
                {
                    moveLocation = rightChildLocation;
                }

                if (moveLocation != location)
                {
                    this.Swap(location, moveLocation);
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
            while (this.content[location].priority.CompareTo(this.content[parentLocation].priority) > 0)
            {
                this.Swap(location, parentLocation);
                location = parentLocation;
                parentLocation = Parent(location);
            }
        }

        private void Swap(int location, int parentLocation)
        {
            (TPriority, TValue) temp = this.content[parentLocation];
            this.content[parentLocation] = this.content[location];
            this.content[location] = temp;
        }

        private void ReallocateUp()
        {
            (TPriority priority, TValue value)[] old = this.content;
            this.content = new (TPriority priority, TValue value)[this.Count * 2];
            old.CopyTo(this.content, 0);
        }

        private void ReallocateDown()
        {
            (TPriority priority, TValue value)[] old = this.content;
            int newSize = ((this.Count / DefaultSize) + 1) * DefaultSize;
            if (newSize != this.content.Length)
            {
                this.content = new (TPriority priority, TValue value)[newSize];
                Array.Copy(old, this.content, newSize);
            }
        }
    }
}
