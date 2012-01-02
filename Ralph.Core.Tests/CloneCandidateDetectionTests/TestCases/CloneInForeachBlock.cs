using System;

namespace AgentRalph.CloneCandidateDetectionTestData
{
    public class CloneInForeachBlock
    {
        void Foo()
        {
            foreach (int i in new int[] { })
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