// NotMatch
using System.Threading;

namespace AgentRalph.AstCompareTestsData
{
    public class Test035
    {
        public void Foo()
        {
            startEvent.Set();
        }
        public void Bar()
        {
            cancelEvent.Set();
        }

        private ManualResetEvent startEvent;
        private ManualResetEvent cancelEvent;
    }
}