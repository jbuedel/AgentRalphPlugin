using System;

namespace AgentRalph.CloneCandidateDetectionTestData
{
    public class CloneInSwitchCaseBlock
    {
        void Foo()
        {
            switch (DateTime.Today.Hour)
            {
                case 1:
                    /* BEGIN pattern();*/
                    Console.WriteLine(7);
                    /* END */
                    break;
            }
        }

        private void pattern()
        {
            Console.WriteLine(7);
        }
    }
}