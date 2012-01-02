using System;

namespace AgentRalph.ExtractMethodTestCases
{
    class DoWhatSharpDevelopDoes2
    {
        public int Target()
        {
            int i = 32;
            int k = 0;

            /* BEGIN */
            Console.Write(i + k);
            i = 7;
            k = 9;
            /* END */

            return i + k;
        }

        void Expected(ref int i, ref int k)
        {
            Console.Write(i + k);
            i = 7;
            k = 9;
        }
    }
}