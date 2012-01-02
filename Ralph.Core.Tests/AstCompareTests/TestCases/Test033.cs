// Match - I think this one had to do with the += operator.
namespace AgentRalph.AstCompareTestsData
{
    public class Test033
    {
        private void Foo(FormNode node)
        {
            node.MyDelegateEvent += Handler;
        }

        private void Bar(FormNode node)
        {
            node.MyDelegateEvent += Handler;
        }


        private void Handler() {}

        private class FormNode
        {
            public event MyDelegate MyDelegateEvent;

            public delegate void MyDelegate();
        }
    }
}