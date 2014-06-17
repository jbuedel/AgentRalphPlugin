using System;

namespace AgentRalph.CloneCandidateDetectionTestData
{
    public class CloneInTryBlock
    {
        void Foo()
        {
            try
            {
                /* BEGIN pattern();*/
                Console.WriteLine(7);
                /* END */
            }
            catch 
            {
            }
        }

        private void pattern()
        {
            Console.WriteLine(7);
        }
    }
}