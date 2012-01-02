// NotMatch - the foreach vars don't match
namespace AgentRalph.Tests.AstCompareTests.TestCases
{
    public class ForEach1
    {
        private void Foo()
        {
            foreach (var i1 in new int[] {})
            {
                
            }
        }

        private void Bar()
        {
            foreach (var i2 in new int[] { })
            {

            }
        }
    }
}