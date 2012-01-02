using System;

namespace Ralph.Test.Project
{
    internal abstract class RedundantTestClass : RedundantTestBase
    {
        public void Foo()
        {
            DoSomething();
            DoSomething();
            DoSomething();
            DoSomething();
            DoSomething();
            DoSomething();

            DoSomethingElse();

            DoOtherStuff();
            DoOtherStuff();
            DoOtherStuff();
            DoOtherStuff();
            DoOtherStuff();
            DoOtherStuff();
            DoOtherStuff();
            DoOtherStuff();
        }
    }

    internal abstract class OtherRedundantClass : RedundantTestBase
    {
        public void Bar()
        {
            DoSomething();
            DoSomething();
            DoSomething();
            DoSomething();
            DoSomething();
            DoSomething();

            DoSomethingAndAgain();

            DoOtherStuff();
            DoOtherStuff();
            DoOtherStuff();
            DoOtherStuff();
            DoOtherStuff();
            DoOtherStuff();
            DoOtherStuff();
            DoOtherStuff();
        }

        private void DoSomethingAndAgain()
        {
            throw new NotImplementedException();
        }
    }

    internal abstract class RedundantTestBase
    {
        protected abstract void DoSomethingElse();

        protected abstract void DoOtherStuff();

        protected abstract void DoSomething();
    }
}