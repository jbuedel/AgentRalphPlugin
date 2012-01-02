// Ignore - Will fix later when we start deep matching again.
using System;

namespace AgentRalph.Tests.CloneCandidateDetectionTests.TestCases
{
    class UnusedParameterDoesNotFailMatch
    {
        /* BEGIN */
        public int Foo(int j)
        {
            return DateTime.Now.Second;
        }
        /* END */

        public int Bar(int j)
        {
            return DateTime.Now.Second;
        }
    }
}
