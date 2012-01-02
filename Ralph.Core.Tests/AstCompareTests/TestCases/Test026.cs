// NotMatch - A simple linq expression.  Bar is does not have a where clause.
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AgentRalph.RowTestsData
{
    public class Test026<T>
    {
        private int member;
        int Foo(IEnumerable<T> enumerable)
        {
            return (from e in enumerable where e.ToString().Length > 0 select e).Count();
        }
        int Bar(IEnumerable<T> enumerable)
        {
            return (from e in enumerable select e).Count();
        }
    }
}