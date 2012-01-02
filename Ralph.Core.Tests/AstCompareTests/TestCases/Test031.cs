// NotMatch - Bug fix test where method candidates w/ different param lists caused an exception during comparison.
using System;
namespace AgentRalph.AstCompareTestsData
{
    class ClonedFunctionTestClass
    {
        public void Foo()
        {
        }

        public void Bar(DateTime t, int j)
        {
        }
    }
}
