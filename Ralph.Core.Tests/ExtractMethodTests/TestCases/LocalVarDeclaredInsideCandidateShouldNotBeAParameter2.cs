// #D captures the local var properly.  I screwed it up somehow when porting.  Probably that post process visitor thing handles it.
using System;

namespace AgentRalph.ExtractMethodTests.TestCases
{
    class LocalVarDeclaredInsideCandidateShouldNotBeAParameter2
    {
        public int Target()
        {
            int i = 32;

            /* BEGIN */
            int k = 0;
            Console.Write(k);
            k = 9;
            /* END */

            return i + k;
        }
        // This is what #D returns.  But Ralph isn't doing it right for some reason.
        int Expected()
        {
            int k = 0;
            Console.Write(k);
            k = 9;
            return k;
        }
    }
}
