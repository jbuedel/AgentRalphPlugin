// Match
namespace AgentRalph.Tests.AstCompareTests.TestCases
{
    public class ForEach2
    {
        private void Foo()
        {
            foreach (var item in new int[] {})
            {
                
            }
        }

        private void Bar()
        {
            foreach (var item in new int[] { })
            {

            }
        }
    }
}