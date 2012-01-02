using ICSharpCode.NRefactory.Ast;

namespace AgentRalph.Visitors
{
    public class AstComparisonIgnoreLiteralsVisitor : AstComparisonVisitor
    {
        public override bool VisitPrimitiveExpression(PrimitiveExpression primitiveExpression, object d)
        {
            return true;
        }
    }
}