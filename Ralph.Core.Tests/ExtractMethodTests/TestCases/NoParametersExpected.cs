using System;

namespace AgentRalph.ExtractMethodTests.TestData
{
    class NoParametersExpected
    {
        public void Target()
        {
            int i = 32 + DateTime.Now.Minute;
            string str = "zippy";
            Console.Write(str);
            Console.Write("i=" + i);

            /* BEGIN */
            if (DateTime.Now == DateTime.Today)
                throw new ApplicationException("That's just crazy.");
            /* END */
        }

        void Expected()
        {
            if (DateTime.Now == DateTime.Today)
                throw new ApplicationException("That's just crazy.");
        }
    }
}