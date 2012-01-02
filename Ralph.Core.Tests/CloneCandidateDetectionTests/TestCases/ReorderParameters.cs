// Ignore - Not implemented.  Requires a parameter reordering refactoring.
namespace AgentRalph.Tests.CloneCandidateDetectionTests.TestCases
{
    class ReorderParameters
    {
        void Foo(int a, int b)
        {
            /* BEGIN */
            int i = a + b;
            /* END */
        }

        void Bar(int j, int k)
        {
            int i = k + j;
        }
    }
}
