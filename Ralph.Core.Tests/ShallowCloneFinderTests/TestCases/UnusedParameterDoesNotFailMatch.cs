// Ignore - Should match, but there's a bug where an unused parameter (exists on both methods - the are textually identical) causes the match to fail.  
// It's because I'm doing the extract method to create a provisional method, and of course that does not capture an unused identifier.
using System;

namespace AgentRalph.Tests.ShallowCloneFinderTests.TestCases
{
    class UnusedParameterDoesNotFailMatch
    {
        /* BEGIN Bar(j)*/
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
