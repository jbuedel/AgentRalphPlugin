// Match - Casting should not stop matching.
namespace AgentRalph.AstCompareTestsData
{
    public class Test032
    {
        private void Foo()
        {
            int i = (int) 7;
        }
        private void Bar()
        {
            int i = (int) 7;
        }
    }
}