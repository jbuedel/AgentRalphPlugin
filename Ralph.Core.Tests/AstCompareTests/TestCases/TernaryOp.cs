// Match
using System;

namespace Ralph.Test.Project
{
    class TernaryOp
    {
        public void Foo()
        {
            string s = DateTime.Now.Ticks == 1000 ? "yes" : "no";
        }
        public void Bar()
        {
            string s = DateTime.Now.Ticks == 1000 ? "yes" : "no";
        }
    }
}