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
        public void RandomInsertCheckOrderedExtract()
        {
            var queue = new PriorityQueue<int, string>();
            queue.Insert(100, "One Hundred");
            queue.Insert(1, "One");
            queue.Insert(5, "Five");
            queue.Insert(75, "Seventy Five");
            queue.Insert(32, "Thirty Two");
            queue.Insert(64, "Sixty Four");
            queue.Insert(18, "Eighteen");
            TestContext.WriteLine(@"Initial: " + queue.Dump());
            TestContext.WriteLine("");

            KeyValuePair<int, string>? value;
            int priority = Int32.MaxValue;
            while ( (value = queue.Extract()).HasValue)
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
