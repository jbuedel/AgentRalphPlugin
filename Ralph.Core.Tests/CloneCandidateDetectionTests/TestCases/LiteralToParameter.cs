using System;

namespace AgentRalph.CloneCandidateDetectionTests.TestCases
{
    public class LiteralToParameter
    {
        private void Target()
        {
            Console.WriteLine("stuff");
            /* BEGIN pattern(7);*/
            int j = 7 + 7;
            /* END */
            Console.WriteLine("more stuff");
        }

        private void pattern(int i)
        {
            int j = i + i;
        }
    }
}