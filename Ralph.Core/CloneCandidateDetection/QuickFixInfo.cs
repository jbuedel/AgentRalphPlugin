using System;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;
using SharpRefactoring;
using System.Collections.Generic;

namespace AgentRalph.CloneCandidateDetection
{
    [Serializable]
    public class QuickFixInfo 
    {
        private readonly MethodDeclaration left;

        private readonly MethodDeclaration right;

        private readonly MethodDeclaration extractedMethod;

        private readonly Window window;

        public QuickFixInfo(MethodDeclaration left, MethodDeclaration right, MethodDeclaration extractedMethod,
                                                            Window window, int replacementSectionEndLine, int replacementSectionStartLine, int highlightStartLocationColumn,
                                                            IList<object> literalParams)
        {
            if (literalParams == null) throw new ArgumentNullException("literalParams");

            this.left = left;
            this.right = right;
            this.extractedMethod = extractedMethod;
            this.window = window;
            ReplacementSectionEndLine = replacementSectionEndLine;
            ReplacementSectionStartLine = replacementSectionStartLine;
            HighlightStartLocationColumn = highlightStartLocationColumn;
 			LiteralParams = literalParams;
        }
        
        public QuickFixInfo(QuickFixInfo r, IList<object> literalParams)
        	: this(r.left, r.right, r.extractedMethod, r.window, r.ReplacementSectionEndLine, r.ReplacementSectionStartLine, r.HighlightStartLocationColumn, literalParams)
        { }
        
        public string TextForACallToJanga
        {
            get
            {
                var m  = Invocation as Statement;
                if( m != null)
                    return ChangeInvocationName(left.Name, m.DeepCopy()).Print();

                var q = Invocation as Expression;
                if (q != null)
                {
                    return ChangeInvocationName(left.Name, q.DeepCopy()).Print();
                }
                
                throw new ApplicationException("mmm mmm");
            }
        }

        private INode ChangeInvocationName(string newName, INode node)
        {
            RenameIdentifierVisitor v = new RenameIdentifierVisitor("NewMethod", newName, StringComparer.InvariantCulture);
            node.AcceptVisitor(v, null);
            return node;
        }

        public IList<object> LiteralParams { get; private set; }
        // TODO: Convert to a Statement.  Expression is not needed.
        public AbstractNode Invocation { get; set; }

        public int ReplacementSectionStartLine { get; private set; }

        public int ReplacementSectionEndLine { get; private set; }

        public int HighlightStartLocationColumn { get; private set; }

        public int HighlightStartLocationLine
        {
            get { return ReplacementSectionStartLine; }
        }

        public int HighlightLength
        {
            get { return 10; }
        }

        public string BulbItemText
        {
            get { return "Replace this code with a call to " + TextForACallToJanga + "."; }
        }

        public string ReplacementText
        {
            get
            {
                return "This section of code is a clone of " + TextForACallToJanga + ".";
            }
        }

        public override string ToString()
        {
            return
                string.Format(
                    "Left: \n{0} \nRight: \n{1} \nExtractedMethod: {2} \nWindow: {3} \nReplacementSectionStartLine: {4} \nReplacementSectionEndLine: {5}",
                    left.Print(), right.Print(), extractedMethod.Print(), window, ReplacementSectionStartLine, ReplacementSectionEndLine);
        }
    }
}