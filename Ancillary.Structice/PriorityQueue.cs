// <copyright file="PriorityQueue.cs" company="Gareth Jones">
// © Gareth Jones. All rights reserved.
// </copyright>

namespace Ancillary.Structice;

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

    /// <summary>
    /// The array that the queue's values are stored in.
    /// </summary>
    /// <remarks>Valid values are in elements from 0 to Count - 1.</remarks>
    private QueueContentItem[] content;

    /// <summary>
    /// Initializes a new instance of the <see cref="PriorityQueue{TPriority, TValue}"/> class.
    /// </summary>
    public PriorityQueue()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PriorityQueue{TPriority, TValue}"/> class.
    /// </summary>
    /// <param name="initialSize">The starting size of the storage that the queue is based on.</param>
    public PriorityQueue(int initialSize)
    {
        this.content = new QueueContentItem[initialSize];
    }

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
            this.content = new QueueContentItem[DefaultSize];
        }

        // Local functions
        int parent(int location) => (location - 1) / 2;

        void reallocateUp()
        {
            QueueContentItem[] old = this.content;
            this.content = new QueueContentItem[this.Count * 2];
            old.CopyTo(this.content, 0);
        }

        void heapifyUp(int location)
        {
            int parentLocation = parent(location);
            while (this.content[location].priority.CompareTo(this.content[parentLocation].priority) > 0)
            {
                this.Swap(location, parentLocation);
                location = parentLocation;
                parentLocation = parent(location);
            }
        }

        if (this.content.Length == this.Count)
        {
            reallocateUp();
        }

        this.content[this.Count] = new QueueContentItem(priority, value);
        heapifyUp(this.Count);
        this.Count++;
    }

    /// <summary>
    /// Take an entry from the queue.
    /// </summary>
    /// <returns>The entry.</returns>
    public (TPriority Priority, TValue Value)? Dequeue()
    {
        // Local functions
        int leftChild(int location) => ((location + 1) * 2) - 1;

        int rightChild(int location) => (location + 1) * 2;

        void reallocateDown()
        {
            QueueContentItem[] old = this.content;
            int newSize = this.content.Length / 2;
            if (newSize != this.content.Length)
            {
                this.content = new QueueContentItem[newSize];
                Array.Copy(old, this.content, newSize);
            }
        }

        void heapifyDown(int location)
        {
            int leftChildLocation = leftChild(location);
            int rightChildLocation = rightChild(location);
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

                leftChildLocation = leftChild(location);
                rightChildLocation = rightChild(location);
            }
        }

        switch (this.Count)
        {
            case 0:
                return null;
            case 1:
                this.Count = 0;
                return (this.content[0].priority, this.content[0].content);
            default:
                {
                    (TPriority, TValue) retVal = (this.content[0].priority, this.content[0].content);
                    this.Count--;
                    this.content[0] = this.content[this.Count];
                    heapifyDown(0);
                    if ((this.content.Length / 2) - this.Count == 1)
                    {
                        reallocateDown();
                    }

                    return retVal;
                }
        }
    }

    /// <summary>
    /// Look at the next entry on the queue without removing it.
    /// </summary>
    /// <returns>The entry.</returns>
    public (TPriority Priority, TValue Value)? Peek()
    {
        if (this.Count == 0)
        {
            return null;
        }
        else
        {
            return (this.content[0].priority, this.content[0].content);
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
    internal string Dump()
    {
        int count = 0;
        int lineCount = 0;
        int rowCount = 0;
        StringBuilder dump = new();
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

            (TPriority priority, TValue value) = this.content[count];
            dump.Append($"{priority}:{value}");
            count++;
            lineCount++;
        }

        return dump.ToString();
    }
#endif

    /// <summary>
    /// Swap two elements withing the content array.
    /// </summary>
    /// <param name="firstLocation">Location of first element to swap.</param>
    /// <param name="secondLocation">Location of second element to swap.</param>
    private void Swap(int firstLocation, int secondLocation)
    {
        QueueContentItem temp = this.content[secondLocation];
        this.content[secondLocation] = this.content[firstLocation];
        this.content[firstLocation] = temp;
    }

    /// <summary>
    /// Private record type to hold the raw queue data.
    /// </summary>
    /// <param name="priority">The priority of the item.</param>
    /// <param name="content">The value of the item.</param>
    private record struct QueueContentItem(TPriority priority, TValue content);
}
