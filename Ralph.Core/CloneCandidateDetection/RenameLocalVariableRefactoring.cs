using System.Collections.Generic;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;

namespace AgentRalph.CloneCandidateDetection
{
    // TODO: Can this be replaced with a RenameIdentifierVisitor?
    public class RenameLocalVariableRefactoring : AbstractAstVisitor
    {
        /// Key is current name, and Value is the new name.
        private readonly IDictionary<string, string> RenameTable;

        public RenameLocalVariableRefactoring(IDictionary<string, string> renameTable)
        {
            RenameTable = renameTable;
        }

        public override object VisitIdentifierExpression(IdentifierExpression identifierExpression, object data)
        {
            if (RenameTable.ContainsKey(identifierExpression.Identifier))
            {
                identifierExpression.Identifier = RenameTable[identifierExpression.Identifier];
            }
            return null;
        }

        public override object VisitParameterDeclarationExpression(ParameterDeclarationExpression parameterDeclarationExpression, object data)
        {
            if (RenameTable.ContainsKey(parameterDeclarationExpression.ParameterName))
            {
                parameterDeclarationExpression.ParameterName = RenameTable[parameterDeclarationExpression.ParameterName];
            }
            return null;
        }

        public override object VisitLocalVariableDeclaration(LocalVariableDeclaration localVariableDeclaration, object data)
        {
            foreach (var vd in localVariableDeclaration.Variables)
            {
                if (RenameTable.ContainsKey(vd.Name))
                {
                    vd.Name = RenameTable[vd.Name];
                }
            }
            return base.VisitLocalVariableDeclaration(localVariableDeclaration, data);
        }
    }
}