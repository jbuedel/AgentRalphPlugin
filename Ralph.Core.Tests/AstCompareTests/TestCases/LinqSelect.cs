// Match
using System.Linq;

namespace Ralph.Test.Project
{
    class LinqSelect
    {
        public void Foo()
        {
            int[] i_ar = new int[]{};
            var e = from i in i_ar
                    let q = 1
                    where i != 0
                    orderby i ascending 
                    select i;
        }
        public void Bar()
        {
            int[] i_ar = new int[]{};
            var e = from i in i_ar
                    let q = 1
                    where i != 0
                    orderby i ascending 
                    select i;
        }
    }
}