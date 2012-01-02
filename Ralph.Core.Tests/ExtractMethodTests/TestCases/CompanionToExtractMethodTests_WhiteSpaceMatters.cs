using System;

namespace AgentRalph.ExtractMethodTests.TestCases
{
    class CompanionToExtractMethodTests_WhiteSpaceMatters
    {
        public void Target()
        {
            int x = 7;
            int y = 8;

            /* BEGIN */
            Console.WriteLine(x * y);
            Console.WriteLine(x - y);
            /* END */
        }

        void Expected(int x, int y)
        {
            Console.WriteLine(x * y);
            Console.WriteLine(x - y);
        }
    }
}
