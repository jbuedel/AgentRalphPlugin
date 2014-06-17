// This match will only work if Rename Local and Extract Method are applied together.
using System;

namespace AgentRalph.Tests.ShallowCloneFinderTests.TestCases
{
    public class WithRenameLocal
    {
        /* BEGIN pattern(); */
        private void Target()
        {
            Console.WriteLine();
            string foo_str = "zippy";
            Console.Write(foo_str);
        }
        /* END */

        private void pattern()
        {
            Console.WriteLine();
            string bar_str = "zippy";
            Console.Write(bar_str);
        }
    }
}