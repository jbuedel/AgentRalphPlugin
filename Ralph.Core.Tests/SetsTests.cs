using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AgentRalph.Tests
{
    [TestFixture]
    public class SetsTests
    {
        [Test]
        public void CartesianProductTest()
        {
            var list1 = new[] {'a', 'b', 'c'};
            var list2 = new[] {'d', 'e'};

            IEnumerable<IEnumerable<char>> en = Sets<char>.CartesianProduct(list1, list2);

            Print(en);

            MyContains(en, new[] {'a'});
            MyContains(en, new[] {'b'});
            MyContains(en, new[] {'c'});

            MyContains(en, new[] {'d'});
            MyContains(en, new[] {'e'});

            MyContains(en, new[] {'a', 'd'});
            MyContains(en, new[] {'a', 'e'});

            MyContains(en, new[] {'b', 'd'});
            MyContains(en, new[] {'b', 'e'});

            MyContains(en, new[] {'c', 'd'});
            MyContains(en, new[] {'c', 'e'});

            Assert.That(en.ToList(), Is.All.Unique);
        }

        [Test]
        public void CartesianProductOfMoreThanTwo()
        {
            var list1 = new[] {'a', 'b', 'c'};
            var list2 = new[] {'d', 'e'};
            var list3 = new[] {'x'};

            var en = Sets<char>.CartesianProduct(new[] {list1, list2, list3});

            Print(en);

            MyContains(en, new[] {'x'});

            MyContains(en, new[] {'a', 'x'});
            MyContains(en, new[] {'b', 'x'});
            MyContains(en, new[] {'c', 'x'});

            MyContains(en, new[] {'d', 'x'});
            MyContains(en, new[] {'e', 'x'});

            MyContains(en, new[] {'a', 'd', 'x'});
            MyContains(en, new[] {'a', 'e', 'x'});

            MyContains(en, new[] {'b', 'd', 'x'});
            MyContains(en, new[] {'b', 'e', 'x'});

            MyContains(en, new[] {'c', 'd', 'x'});
            MyContains(en, new[] {'c', 'e', 'x'});

            Assert.That(en.ToList(), Is.All.Unique);
        }

        private void Print(IEnumerable<IEnumerable<char>> en)
        {
            foreach (var e in en)
            {
                foreach (var c in e)
                {
                    Console.Write(" " + c);
                }
                Console.WriteLine();
            }
        }

        private void MyContains<T>(IEnumerable<IEnumerable<T>> en, T[] c)
        {
            if (!en.Any(x => Matches(x, c)))
            {
                throw new ApplicationException("The element is not in the sequence.");
            }
        }

        private bool Matches<T>(IEnumerable<T> enumerable, T[] c)
        {
            if (enumerable.Count() != c.Count())
                return false;

            for (int i = 0; i < enumerable.Count(); i++)
            {
                if (!Equals(enumerable.ToArray()[i], c[i]))
                    return false;
            }
            return true;
        }
    }
}