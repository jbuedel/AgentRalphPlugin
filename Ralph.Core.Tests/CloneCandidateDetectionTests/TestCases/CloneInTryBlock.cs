using System;

namespace AgentRalph.CloneCandidateDetectionTestData
{
    public class CloneInTryBlock
    {
        void Foo()
        {
            try
            {
                /* BEGIN */
                Console.WriteLine(7);
                /* END */
            }
            catch 
            {
            }
        }

        private void Bar()
        {
            Console.WriteLine(7);
        }
    }
}