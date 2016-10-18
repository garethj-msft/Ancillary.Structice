using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ancillary.Structice;
using System.Collections.Generic;

namespace Test.Ancillary.Structice
{
    [TestClass]
    public class TestPriorityQueue
    {
        private TestContext testContextInstance;

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

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
            TestContext.WriteLine(@"Initial: " + queue.Dump());
            TestContext.WriteLine("");

            KeyValuePair<int, string>? value;
            int priority = Int32.MaxValue;
            while ( (value = queue.Dequeue()).HasValue)
            {
                TestContext.WriteLine(value.Value.Value);
                TestContext.WriteLine(@"    " + queue.Dump());
                TestContext.WriteLine("");
                Assert.IsTrue(value.Value.Key <= priority, "Priority violation.");
                priority = value.Value.Key;
            }
        }
    }
}
