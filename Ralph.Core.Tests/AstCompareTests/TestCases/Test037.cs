// Match - Tests reference to 'this'.
namespace AgentRalph.Tests.AstCompareTests.TestCases
{
    public class Test037
    {
        private void Foo()
        {
            Test037 t = this;
        }

        private void Bar()
        {
            Test037 t = this;
        }
    }
}