using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AgentRalph.Tests
{
    public static class Util
    {
        public static void AssertExists<T>(IEnumerable<T> perms, Func<T, bool> o)
        {
            Assert.IsTrue(perms.Any(o));
        }

        public static void AssertExists<T>(IEnumerable<T> perms, Func<T, bool> o, string msg)
        {
            Assert.IsTrue(perms.Any(o), msg);
        }
    }
}