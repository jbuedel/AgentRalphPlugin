using System;

namespace AgentRalph.CloneCandidateDetectionTests.TestCases
{
    class MethodBodyWithSingleChildStillRecursesIntoThoseChildren
    {
        private void Foo()
        {
            if (DateTime.Now.Day == 3)
            {
                /* BEGIN pattern(); */
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
