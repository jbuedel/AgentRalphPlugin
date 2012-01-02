using System;

namespace AgentRalph.CloneCandidateDetectionTestData
{
    public class CloneInWhileBlock
    {
        void Foo()
        {
            while (DateTime.Now < DateTime.Today)
            {
                /* BEGIN */
                Console.WriteLine(7);
                /* END */
            }
        }

        private void Bar()
        {
            Console.WriteLine(7);
        }
    }
}