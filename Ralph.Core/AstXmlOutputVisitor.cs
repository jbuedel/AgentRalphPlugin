using System.Text;
using System.Xml;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;

namespace AgentRalph
{
	/// <summary>
	/// Writes the Ast out as xml.  I wanted to experiment with alternative formatting of c# code files.  Context
	/// sensitive stuff like display parameter defintions in italic, show block boundaries via background colors
	/// instead of braces, et.  I thought applying css to xml might be an easy way to rapidly experiment.
	/// 
	/// I stopped when I realized how much code I was going to have to write.  This class/implementation probably
	/// ought to be auto-generated.  
	/// </summary>
    class AstXmlOutputVisitor : AbstractAstVisitor
    {
        private readonly XmlTextWriter tw = new XmlTextWriter(@"c:\temp\output.xml", Encoding.ASCII);
        public override object VisitAssignmentExpression(AssignmentExpression assignmentExpression, object data)
        {
            tw.WriteStartElement("AssignmentExpression");
            tw.WriteAttributeString("Op", assignmentExpression.Op.ToString());
            base.VisitAssignmentExpression(assignmentExpression, data);
            tw.WriteEndElement();
            return null;
        }

        public override object VisitExpressionStatement(ExpressionStatement expressionStatement, object data)
        {
            tw.WriteStartElement("ExpressionStatement");
            
            base.VisitExpressionStatement(expressionStatement, data);
            tw.WriteEndElement();
            return null;
        }
    }
}
