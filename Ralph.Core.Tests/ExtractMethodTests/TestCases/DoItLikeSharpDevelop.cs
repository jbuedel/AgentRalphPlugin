namespace AgentRalph.CloneCandidateDetectionTestData
{
    class DoItLikeSharpDevelop
    {
        string Target()
		{
			string s = string.Empty;
			int i = 0;
			/* BEGIN s = Bar(s,i);*/
			s += "Hello ";
			s += "world.";
			s += i;
			/* END */
			return s;
		}

        void Expected(ref string s, int i)
		{
			s += "Hello ";
			s += "world.";
			s += i;
		}
    }
}