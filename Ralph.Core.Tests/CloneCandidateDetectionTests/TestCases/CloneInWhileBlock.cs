using System;

namespace AgentRalph.CloneCandidateDetectionTestData
{
    public class CloneInWhileBlock
    {
        void Foo()
        {
            while (DateTime.Now < DateTime.Today)
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