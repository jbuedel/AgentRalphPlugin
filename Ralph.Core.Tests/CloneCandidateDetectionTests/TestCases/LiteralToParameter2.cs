// In this version of the test the clone is really the entire function.  This is necesary b/c of the current situation where
// the code handles 'top level' clones differently.
namespace AgentRalph.CloneCandidateDetectionTests.TestCases
{
    public class LiteralToParameter2
    {
        private void Target()
        {
            /* BEGIN Expected(7);*/
            int j = 7 + 7;
            /* END */
        }

        private void Expected(int i)
        {
            int j = i + i;
        }
    }
}