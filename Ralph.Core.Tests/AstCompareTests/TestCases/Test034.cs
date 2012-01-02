// Match - Array creation expressions are properly matched.

namespace AgentRalph.AstCompareTestsData
{
    public class Test034
    {
        private void Foo()
        {
            int[] arr = new int[1];
        }

        private void Bar()
        {
            int[] arr = new int[1];
        }
    }
}