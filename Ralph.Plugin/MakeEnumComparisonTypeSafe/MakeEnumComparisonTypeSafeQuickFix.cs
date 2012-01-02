using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.Util;

namespace AgentRalph.MakeEnumComparisonTypeSafe
{
	[QuickFix]
	public class MakeEnumComparisonTypeSafeQuickFix : IQuickFix
	{
		private readonly MakeEnumComparisonTypeSafeWarningHighlight suggestion;

		public MakeEnumComparisonTypeSafeQuickFix(MakeEnumComparisonTypeSafeWarningHighlight suggestion)
		{
			this.suggestion = suggestion;
		}

		public bool IsAvailable(IUserDataHolder cache)
		{
			return true;
		}

		public IBulbItem[] Items
		{
			get { return new IBulbItem[]{ new MakeEnumComparisonTypeSafeBulbItem(suggestion.ReplacementText, suggestion.ReplacementRange)}; }
		}
	}
}