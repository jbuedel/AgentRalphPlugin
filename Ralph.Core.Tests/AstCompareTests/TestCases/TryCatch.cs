// Match
using System;

namespace AgentRalph.AstCompareTests.TestCases
{
    class TryCatch
    {
        void Foo()
        {
            try
            {
            }
            catch (Exception excep)
            {
                Console.WriteLine(excep);
            }
        }
        void Bar()
        {
            try
            {
            }
            catch (Exception excep)
            {
                Console.WriteLine(excep);
            }
        }
    }
}
