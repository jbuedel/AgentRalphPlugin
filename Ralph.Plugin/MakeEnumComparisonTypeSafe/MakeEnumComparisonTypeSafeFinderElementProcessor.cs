using System.Collections.Generic;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Resolve;

namespace AgentRalph.MakeEnumComparisonTypeSafe
{
    internal class MakeEnumComparisonTypeSafeFinderElementProcessor : IRecursiveElementProcessor
    {
        private readonly IDaemonProcess process;

        public void ProcessAfterInterior(ITreeNode element)
        {
            var binexpr = element as IEqualityExpression;
            if(binexpr == null)
            {
                return;
            }

            if(IsInNeedOfFixing(binexpr))
            {
            	DocumentRange range_to_replace = element.GetDocumentRange();
            	this.Highlightings.Add(new HighlightingInfo(range_to_replace, new MakeEnumComparisonTypeSafeWarningHighlight(range_to_replace, this.ReplacementText)));
            }
        }

        private bool IsInNeedOfFixing(IEqualityExpression binexpr)
        {
            if(binexpr.EqualityType != EqualityExpressionType.EQEQ) return false;

            // Is the right hand side a string literal?
            var right = binexpr.RightOperand as ILiteralExpression;
            if (right == null || !right.ToTreeNode().GetTokenType().IsStringLiteral)
            {
                return false;
            }


            // Is the left hand side a call to Enum.ToString followed by a zero or more ToUpper,ToLower & Trim calls?
            var left = binexpr.LeftOperand as IInvocationExpression;
            if(left == null)
            {
                return false;
            }

            var visitor = new MyIsExpressionStringCallsOnAnEnum();
            left.Accept( visitor );

            if (visitor.Found)
            {
                var enumSymbolTable = visitor.FoundEnumReference.GetReferenceSymbolTable(true).Filter(new MyFilter());
                
                foreach (ISymbolInfo info in enumSymbolTable.AllSymbolInfos())
                {
                    if (string.Compare(info.ShortName, right.ConstantValue.Value.ToString(), true) == 0)
                    {
                    	ReplacementText = visitor.EnumReferenceName + " == " + visitor.EnumDeclaredName + "." + info.ShortName;
                        return true;
                    }
					// TODO: Technically, a case mismatch should result in a "conditional is always false" error.
                }
            }
            return false;
        }

        #region boiler plate 

        public MakeEnumComparisonTypeSafeFinderElementProcessor(IDaemonProcess process)
        {
            this.process = process;
        }

        public bool InteriorShouldBeProcessed(ITreeNode element)
        {
            return true;
        }

        public void ProcessBeforeInterior(ITreeNode element)
        {
        }

        public bool ProcessingIsFinished
        {
            get { return process.InterruptFlag; }
        }

        public readonly IList<HighlightingInfo> Highlightings = new List<HighlightingInfo>();
    	private string ReplacementText;

    	#endregion
    }
}