// Match
using System;

namespace Ralph.Test.Project
{
    class AnonymousMethod
    {
        public void Foo()
        {
            var q = new EventHandler(delegate(object x, EventArgs e) { x.ToString(); });
        }

        public void Bar()
        {
            var q = new EventHandler(delegate(object x, EventArgs e) { x.ToString(); });
        }
    }
}