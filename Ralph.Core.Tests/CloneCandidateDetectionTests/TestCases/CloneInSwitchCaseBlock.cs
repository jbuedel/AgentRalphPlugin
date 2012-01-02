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
                    /* BEGIN */
                    Console.WriteLine(7);
                    /* END */
                    break;
            }
        }

        private void Bar()
        {
            Console.WriteLine(7);
        }
    }
}