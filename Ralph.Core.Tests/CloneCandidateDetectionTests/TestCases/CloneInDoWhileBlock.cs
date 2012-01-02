using System;

namespace AgentRalph.CloneCandidateDetectionTestData
{
    public class CloneInDoWhileBlock
    {
        void Foo()
        {
            do
            {
                /* BEGIN */
                Console.WriteLine(7);
                /* END */
            }
            while (DateTime.Now < DateTime.Today);
        }

        private void Bar()
        {
            Console.WriteLine(7);
        }
    }
}