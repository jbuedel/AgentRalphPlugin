using System;

namespace AgentRalph.ExtractMethodTests.TestData
{
    class TwoParametersExpected
    {
        public void Target()
        {
            int i = 32 + DateTime.Now.Minute;
            string str = "zippy";

            /* BEGIN */
            Console.Write(str);
            Console.Write("i=" + i);
            /* END */

            if (DateTime.Now == DateTime.Today)
                throw new ApplicationException("That's just crazy.");
        }

        public void Expected(int i, string str)
        {
            Console.Write(str);
            Console.Write("i=" + i);
        }
    }
}