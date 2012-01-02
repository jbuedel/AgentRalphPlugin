using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Resolve;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using System.Collections.Generic;

namespace AgentRalph.MakeEnumComparisonTypeSafe
{
    /// <summary>
    /// Scans invocation expressions looking for enum.ToString() calls.
    /// </summary>
    internal class MyIsExpressionStringCallsOnAnEnum : ElementVisitor
    {
        // This will be called multiple times in the case of chained method calls.  
    	public override void VisitInvocationExpression(IInvocationExpression invocationExpressionParam)
        {
    		// TODO: Loop through here somehow and work up through the string calls like ToLower().ToUpper() et et.
            //do
//    	    {
				IReferenceExpression expression = invocationExpressionParam.InvokedExpression as IReferenceExpression;
    	        if (expression == null)
    	            return;

    	        // As I understand it this takes the abstract syntax element that is e.ToString() and resolves it such that
    	        // we know it's the System.Enum.ToString() method call.
    	        IResolveResult resolveResult = expression.Reference.Resolve();

    	        IDeclaredElement e = resolveResult.DeclaredElement;

    	        if (e == null)
    	            return;

    	        ITypeElement containingType = e.GetContainingType();


    	        // work up through the string calls 
                //invocationExpressionParam = invocationExpressionParam.InvocationExpressionReference as IInvocationExpression;
                //var allowed = new[] { "ToUpper", "ToLower", "ToString" };
                //if (!new List<string>(allowed).Contains(e.ShortName))
                //    return;
//    	    }
            //while (containingType != null && containingType.CLRName == "System.String");

            if(containingType == null)
                return;
 
            if(containingType.CLRName == "System.Enum" && e.ShortName == "ToString")
            {
            	Found = true;

            	// Save the enum declaration so we can implement the fix.
            	this.FoundEnumReference = invocationExpressionParam.Reference;
            	this.EnumReferenceName = expression.QualifierExpression.GetText();

				IExpressionType qe = expression.QualifierExpression.GetExpressionType();
				this.EnumDeclaredName = ((IDeclaredType)qe).GetPresentableName(PsiLanguageType.GetByProjectFile(invocationExpressionParam.GetProjectFile()));
            }
        }

    	public string EnumDeclaredName { get; set; }

    	public string EnumReferenceName { get; set; }

        public ICSharpInvocationReference FoundEnumReference { get; set; }

        public bool Found { get; set; }
    }

    internal class MyFilter : ISymbolFilter
    {
        public IList<ISymbolInfo> FilterArray(IList<ISymbolInfo> data)
        {
            return new List<ISymbolInfo>(Filter(data));
        }

        private IEnumerable<ISymbolInfo> Filter(IList<ISymbolInfo> data)
        {
            foreach (ISymbolInfo info in data)
            {
                if (info.GetDeclaredElement().GetElementType() == CLRDeclaredElementType.ENUM_MEMBER)
                {
                    yield return info;
                }
            }
        }

        public ResolveErrorType ErrorType
        {
            get { return ResolveErrorType.OK; }
        }

        public FilterRunType RunType
        {
            get { return FilterRunType.REGULAR; }
        }

        public bool MustRun
        {
            get { return true; }
        }
    }
}