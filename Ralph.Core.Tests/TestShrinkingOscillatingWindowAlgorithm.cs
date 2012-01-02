using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpRefactoring;

namespace AgentRalph.Tests
{
    [TestFixture]
    public class TestShrinkingOscillatingWindowAlgorithm
    {
        [Test]
        public void ShrinkingOscillatingWindowTest()
        {
            const int cl = 4;

            IEnumerable<Window> windows = AstMatchHelper.OscillateWindows(cl);

            CollectionAssert.Contains(windows, new Window(0, 0));
            CollectionAssert.Contains(windows, new Window(0, 1));
            CollectionAssert.Contains(windows, new Window(0, 2));
            CollectionAssert.Contains(windows, new Window(0, 3), "This would be the entire function body.");

            CollectionAssert.Contains(windows, new Window(1, 1));
            CollectionAssert.Contains(windows, new Window(1, 2));
            CollectionAssert.Contains(windows, new Window(1, 3));

            CollectionAssert.Contains(windows, new Window(2, 2));
            CollectionAssert.Contains(windows, new Window(2, 3));

            CollectionAssert.Contains(windows, new Window(3, 3));

            Assert.AreEqual(10, windows.Count(), "There are more than expected.");

            Assert.AreEqual(5, new Window(0, 4).Size, "Size is determined inclusive with the end points.");

            Assert.Throws<ArgumentException>(() => { new Window(1, 0); }, "size parameters are out of order");
            Assert.Throws<ArgumentException>(() => { new Window(-2, 3); }, "zero is the minimum");
            Assert.Throws<ArgumentException>(() => { new Window(0, -3); }, "zero is the minimum");
        }

        [Test]
        [Description("The expected order is largest window first.  The order within a window size is irrelevant.")]
        public void TestOrder()
        {
            const int MaxSize = 2;
            var windows = AstMatchHelper.OscillateWindows(MaxSize);

            var last_size = MaxSize;
            foreach (var window in windows)
            {
                Assert.LessOrEqual(window.Size, last_size);
                last_size = window.Size;
            }
        }
    }
}