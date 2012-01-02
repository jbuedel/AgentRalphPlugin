// Match
using System;

namespace Ralph.Test.Project
{
    class Lambda
    {
        public void Foo()
        {
            var q = new EventHandler((x, e) => x.ToString());
        }

        public void Bar()
        {
            var q = new EventHandler((x, e) => x.ToString());
        }
    }
}