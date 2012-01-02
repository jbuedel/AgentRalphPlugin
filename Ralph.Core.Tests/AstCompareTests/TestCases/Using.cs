// Match - using statement
namespace AgentRalph.Tests.AstCompareTests.TestCases
{
    public class Using
    {
        private void Foo()
        {
            using (UsingInheritor u = new UsingInheritor()) { }
        }

        private void Bar()
        {
            using (UsingInheritor u = new UsingInheritor()) { }
        }
    }

    public class UsingInheritor : IDispose 
    {
        public void Dispose() { }
    }
}