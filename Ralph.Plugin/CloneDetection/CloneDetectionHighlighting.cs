using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Intentions.Extensibility;
using QuickFixInfo = AgentRalph.CloneCandidateDetection.QuickFixInfo;

namespace AgentRalph.CloneDetection
{
	[StaticSeverityHighlighting(Severity.WARNING, "AgentRalph")]
	public class CloneDetectionHighlighting : IHighlighting
	{
        private readonly QuickFixInfo clone;
	    private readonly DocumentRange bodyRange;

        public CloneDetectionHighlighting(QuickFixInfo clone, DocumentRange bodyRange)
		{
            this.clone = clone;
            this.bodyRange = bodyRange;
		}


	    public bool IsValid()
	    {
	        return true;
	    }

	    public string ToolTip
		{
			get { return ErrorStripeToolTip; }
		}

		public string ErrorStripeToolTip
		{
			get { return clone.ReplacementText; }
		}

		public int NavigationOffsetPatch
		{
			get { return 0; }
		}

        // Create a BulbItem for each possible way we can do something with the clone we've found.
        // That is, each one of these is a member of the drop down list.
        public IBulbAction[] GetBulbItems()
	    {
            return new IBulbAction[]
	                   {
	                       new CloneDetectionReplaceContentsWithCallToBulbItem(bodyRange, 
	                                                                           clone.TextForACallToJanga, clone.BulbItemText)
	                   };
	    }
	}
}