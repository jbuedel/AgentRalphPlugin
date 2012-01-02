// NotMatch
using System.Collections.Generic;

class FooBar017
        {
            private void Foo()
            {
                List<int> l = new List<int>();
                l.Add(1);
            }

            private void Bar()
            {
                IList<int> l = new List<int>();
                l.Add(2);            
            }
        }
        