using System;

namespace AgentRalph.CloneCandidateDetectionTestData
{
    public class CloneInForBlock
    {
        void Target()
        {
            for (int i = 0; i < 10; i++)
            {
                /* BEGIN pattern();*/
                Console.WriteLine(7);
                /* END */
            }
        }

        private void pattern()
        {
            Console.WriteLine(7);
        }
    }
}