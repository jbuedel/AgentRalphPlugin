using System.Collections.Generic;
using System.Linq;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.TextControl;
using JetBrains.Util;

namespace AgentRalph.CloneDetection
{
	// The QuickFix is tied to the Highlighting by this ctor param.  R# must have a highlight (created by my daemon)
	// and says "is there anything w/ a QuickFix attrib or a IQuickFix base that also has this type as a ctor param?".
	[QuickFix]
	public class CloneDetectionQuickFix : IQuickFix
	{
		private readonly CloneDetectionHighlighting suggestion;

	    public CloneDetectionQuickFix(CloneDetectionHighlighting suggestion)
		{
			this.suggestion = suggestion;
		}

	    public IEnumerable<IntentionAction> CreateBulbItems()
	    {
	        return suggestion.GetBulbItems().ToContextAction();
	    }

	    public bool IsAvailable(IUserDataHolder cache)
		{
			return true;
		}
	}

    internal class CloneDetectionReplaceContentsWithCallToBulbItem : IBulbAction
	{
		private readonly string TextToInsert;
		private DocumentRange DocRangeToReplace;
	    private readonly string BulbItemText;

	    public CloneDetectionReplaceContentsWithCallToBulbItem(DocumentRange docRangeToReplace, string textToInsert, string bulbItemText)
		{
	        DocRangeToReplace = docRangeToReplace;
	        TextToInsert = textToInsert;
		    BulbItemText = bulbItemText;
		}

		public void Execute(ISolution solution, ITextControl textControl)
		{
//			using (CommandCookie.Create(Text))
			{
//                using (ModificationCookie ensureWritable = DocumentManager.GetInstance(solution).EnsureWritable(DocRangeToReplace.Document))
				{
//					if (ensureWritable.EnsureWritableResult == EnsureWritableResult.SUCCESS)
					{
                        const string indent = "\t\t\t";// hokey, I know
					    textControl.Document.ReplaceText(DocRangeToReplace.TextRange, indent + TextToInsert);
					}
				}
			}
		}

		public string Text
		{
			get { return BulbItemText; }
		}
	}
}