// #D captures the local var properly.  I screwed it up somehow when porting.  Probably that post process visitor thing handles it.
using System;

namespace AgentRalph.ExtractMethodTests.TestData
{
    class LocalVarDeclaredInsideCandidateShouldNotBeAParameter
    {
        public void Target()
        {
            int i = 32 + DateTime.Now.Minute;

            /* BEGIN */
            string str = "zippy";
            Console.Write(str);
            /* END */

            Console.Write("i=" + i);

            if (DateTime.Now == DateTime.Today)
                throw new ApplicationException("That's just crazy.");
        }

        public void Expected()
        {
            string str = "zippy";
            Console.Write(str);
        }
    }
}
