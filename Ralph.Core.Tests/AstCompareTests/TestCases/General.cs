// Match
using System;
using System.Collections.Generic;

namespace Ralph.Test.Project
{
    class General : Base2
    {
        public IEnumerable<int> Foo()
        {
            base.Foo();

            label:

            checked
            {
                
            }

            int j;
            Baz(out j);

            for (int i = 0; i < 10; i++)
            {
                
            }

            lock(new object()) {}

            goto label;

            int q = sizeof (int);

            var v = typeof(int);

            yield return 1;
        }

        public IEnumerable<int> Bar()
        {
            base.Foo();

        label:

            checked
            {

            }

            int j;
            Baz(out j);

            for (int i = 0; i < 10; i++)
            {

            }

            lock (new object()) { }

            goto label;

            int q = sizeof(int);

            var v = typeof(int);

            yield return 1;
        }
    }

    class Base2
    {
        protected void Foo() { }

        protected static void Baz(out int i)
        {
            i = 7;
        }
    }
}