using System;

namespace AgentRalph.CloneCandidateDetectionTestData
{
    public class ForEachDoesNotCancelOutAllOtherClones
    {
        void Foo()
        {
            Console.WriteLine(7);

            foreach (int i in new int[] { })
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