// This match will only work if Rename Local and Extract Method are applied together.
using System;

namespace AgentRalph.ExtractMethodTests.TestCases
{
    public class WithRenameLocal
    {
        private void Target()
        {
            Console.WriteLine("Before");
            
            /* BEGIN Expected(); */
            Console.WriteLine();// At the time this test was created, this statement was necessary to work around a different bug involving extracted method params.
            string foo_str = "zippy";
            Console.Write(foo_str);
            /* END */

            Console.WriteLine("After");
        }

        private void Expected()
        {
            Console.WriteLine();
            string bar_str = "zippy";
            Console.Write(bar_str);
        }
    }
}