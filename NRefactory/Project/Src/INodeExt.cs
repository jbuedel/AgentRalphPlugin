using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.PrettyPrinter;

namespace ICSharpCode.NRefactory
{
    public static class INodeExt
    {
        public static string Print(this INode md)
        {
            return GenerateCode(md);
        }

        private static string GenerateCode(INode unit/*, bool installSpecials*/)
        {
            CSharpOutputVisitor visitor = new CSharpOutputVisitor();

            //			if (installSpecials)
            //			{
            //				SpecialNodesInserter.Install(this.specialsList, visitor);
            //			}

            unit.AcceptVisitor(visitor, null);
            return visitor.Text;
        }
    }
}