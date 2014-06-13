using System;

namespace AgentRalph.CloneCandidateDetectionTestData
{
    public class CloneInDoWhileBlock
    {
        void Target()
        {
            do
            {
                /* BEGIN pattern(); */
                Console.WriteLine(7);
                /* END */
            }
            while (DateTime.Now < DateTime.Today);
        }

        private void pattern()
        {
            Console.WriteLine(7);
        }
    }
}