using System;
using ICSharpCode.NRefactory.Ast;

namespace ICSharpCode.NRefactory.Visitors
{
    public class NodeCountingVisitor : NodeTrackingAstVisitor
    {
        protected override void BeginVisit(INode node)
        {
            NodeCount++;
        }

        public int NodeCount { get; private set; }
    }
}