using JetBrains.Application;
using JetBrains.DocumentManagers;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.Util;

namespace AgentRalph.MakeEnumComparisonTypeSafe
{
	public class MakeEnumComparisonTypeSafeBulbItem : IBulbItem
	{
		public MakeEnumComparisonTypeSafeBulbItem(string callText, DocumentRange _documentRange)
		{
			this._functionCallReplacementText = callText;
			this._documentRange = _documentRange;
		}

		public void Execute(ISolution solution, JetBrains.TextControl.ITextControl textControl)
		{
			using (CommandCookie.Create(Text))
			{
                using (ModificationCookie ensureWritable = DocumentManager.GetInstance(solution).EnsureWritable(_documentRange.Document))
				{
					if (ensureWritable.EnsureWritableResult == EnsureWritableResult.SUCCESS)
					{
						textControl.Document.ReplaceText(_documentRange.TextRange, _functionCallReplacementText);
					}
				}
			}
		}

		public string Text
		{
			get { return string.Format("Replace with a {0} call.", _functionCallReplacementText); }
		}

		private readonly string _functionCallReplacementText;
		private readonly DocumentRange _documentRange;
	}
}