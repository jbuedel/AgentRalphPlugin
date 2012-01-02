// Match
using System;

namespace Ralph.Test.Project
{
    class Switch
    {
        public void Foo()
        {
            string s = "";
            switch(s)
            {
                case "josh":
                    break;
                case "foo":
                    goto case "foo";
                default:
                    break;
            }
        }

        public void Bar()
        {
            string s = "";
            switch (s)
            {
                case "josh":
                    break;
                case "foo":
                    goto case "foo";
                default:
                    break;
            }
        }
    }
}