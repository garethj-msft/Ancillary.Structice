// <copyright file="PriorityQueue.cs" >
// © Gareth Jones. All rights reserved.
// </copyright>

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ancillary.Structice;

namespace Test.Ancillary.Structice;

/// <summary>
/// Tests for the Priority queue.
/// </summary>
[TestClass]
public class TestPriorityQueue
{
    /// <summary>
    /// Gets or sets the test context which provides
    /// information about and functionality for the current test run.
    /// </summary>
    public TestContext TestContext { get; set; }

    [TestMethod]
    public void RandomEnqueueCheckOrderedDequeue()
    {
        var queue = new PriorityQueue<int, string>();
        queue.Enqueue(100, "One Hundred");
        queue.Enqueue(1, "One");
        queue.Enqueue(5, "Five");
        queue.Enqueue(75, "Seventy Five");
        queue.Enqueue(32, "Thirty Two");
        queue.Enqueue(64, "Sixty Four");
        queue.Enqueue(18, "Eighteen");
#if DEBUG
            this.TestContext.WriteLine(@"Initial: " + queue.Dump());
#endif
        this.TestContext.WriteLine("");

        (int priority, string value)? dequeued;
        int priority = int.MaxValue;
        while ((dequeued = queue.Dequeue()).HasValue)
        {
            this.TestContext.WriteLine(dequeued.Value.value);
#if DEBUG
                this.TestContext.WriteLine(@"    " + queue.Dump());
#endif
            this.TestContext.WriteLine("");
            Assert.IsTrue(dequeued.Value.priority <= priority, "Priority violation.");
            priority = dequeued.Value.priority;
        }
    }
}
