using System;

namespace AgentRalph.CloneCandidateDetectionTestData
{
    public class CloneInForBlock
    {
        void Foo()
        {
            for (int i = 0; i < 10; i++)
            {
                /* BEGIN Bar();*/
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