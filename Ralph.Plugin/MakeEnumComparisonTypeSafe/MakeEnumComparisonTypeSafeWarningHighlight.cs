using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;

namespace AgentRalph.MakeEnumComparisonTypeSafe
{
    [StaticSeverityHighlighting(Severity.WARNING, "AgentRalph")]
    public class MakeEnumComparisonTypeSafeWarningHighlight : IHighlighting
    {
		public string ReplacementText { get; private set; }
    	public DocumentRange ReplacementRange { get; private set;}

    	public MakeEnumComparisonTypeSafeWarningHighlight(DocumentRange range_to_replace, string replacement_text)
    	{
    		ReplacementText = replacement_text;
    		this.ReplacementRange = range_to_replace;
    	}

    	private const string tip = "This enum comparison can be accomplished in a type safe manner.";

        public bool IsValid()
        {
            return true;
        }

        public string ToolTip
        {
            get
            {
                return tip;
            }
        }

        public string ErrorStripeToolTip
        {
            get { return tip; }
        }

        public int NavigationOffsetPatch
        {
            get { return 0; }
        }
    }
}