// NotMatch - different ordering
using System.Linq;

namespace Ralph.Test.Project
{
    class LinqSelect2
    {
        public void Foo()
        {
            int[] i_ar = new int[]{};
            var e = from i in i_ar
                    where i != 0
                    orderby i descending 
                    select i;
        }
        public void Bar()
        {
            int[] i_ar = new int[]{};
            var e = from i in i_ar
                    where i != 0
                    orderby i ascending 
                    select i;
        }
    }
}