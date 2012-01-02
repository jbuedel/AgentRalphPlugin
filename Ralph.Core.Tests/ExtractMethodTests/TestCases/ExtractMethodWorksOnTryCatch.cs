using System;

namespace AgentRalph.ExtractMethodTests.TestData
{
    class ExtractMethodWorksOnTryCatch
    {
        public void Target()
        {
            int i = 32 + DateTime.Now.Minute;
            string str = "zippy";
            Console.Write(str);

            /* BEGIN */
            try
            {
            }
            catch (Exception excep)
            {
                Console.WriteLine(excep);
            }
            /* END */

            if (DateTime.Now == DateTime.Today)
                throw new ApplicationException("That's just crazy.");
        }

        void Expected()
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