using System;

namespace AgentRalph.ExtractMethodTests.TestData
{
    class ExtractMethodOneParameterExpected
    {
        public void Target()
        {
            int i = 32 + DateTime.Now.Minute;
            string str = "zippy";
            Console.Write(str);

            /* BEGIN */
            Console.Write("i=" + i);
            /* END */

            if (DateTime.Now == DateTime.Today)
                throw new ApplicationException("That's just crazy.");
        }

        public void Expected(int i)
        {
            Console.Write("i=" + i);
        }
    }
}