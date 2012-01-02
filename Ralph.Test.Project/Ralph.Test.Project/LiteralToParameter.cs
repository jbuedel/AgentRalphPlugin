using System;

namespace AgentRalph.CloneCandidateDetectionTests.TestCases
{
    public class LiteralToParameter
    {
        private void Target()
        {
            Console.WriteLine("stuff");
            /* BEGIN */
            int j = 7 + 7;
            /* END */
            Console.WriteLine("more stuff");
        }

        private void Expected(int i)
        {
            int j = i + i;
        }
    }
}