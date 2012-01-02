using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;
using SharpRefactoring;

namespace AgentRalph.CloneCandidateDetection
{
    public interface IInitialRefactoringExpansion
    {
        IEnumerable<CloneDesc> GetAllPossibleExtractedMethods(MethodDeclaration right, int targetChildCount);
    }

    public class OscillatingExtractMethodRefactoringExpansion : IInitialRefactoringExpansion
    {
        public IEnumerable<CloneDesc> GetAllPossibleExtractedMethods(MethodDeclaration right, int targetChildCount)
        {
            List<INode> children = right.Body.Children;

            var windows = AstMatchHelper.OscillateWindows(children.Count);

            return ProcessChildren(right, children, windows, targetChildCount);
        }

        private IEnumerable<CloneDesc> ProcessChildren(MethodDeclaration right, List<INode> children,
                                                                          IEnumerable<Window> windows, int targetChildCount)
        {
            foreach (var window in windows)
            {
                // Heuristic: Only use window sizes matching the size of the template.  This assumes that extracted
                // methods will not have their number of children change through some further refactoring.  For 
                // example, an Inline Variable might reduce the number of children.  But for now it works very well.
                if (targetChildCount == window.Size)
                {
                    var desc = ExtractMethodAsCloneDesc(window, right, children);
                    if(desc != null) yield return desc;
                }

                if (window.Size == 1)
                {
                    var inners = ProcessChild(right, children[window.Top], targetChildCount);
                    foreach (var inner in inners)
                    {
                        yield return inner;
                    }
                }
            }
        }

        internal static CloneDesc ExtractMethodAsCloneDesc(Window window, MethodDeclaration right, List<INode> children)
        {
            CSharpMethodExtractor extractor = new CSharpMethodExtractor();
            extractor.Extract(right, window, children);

            MethodDeclaration md = extractor.ExtractedMethod;
            if (md != null)
                return new CloneDesc(md, window, children) {ReplacementInvocation = extractor.GetCall(right, md, new VariableDeclaration("heyman"))};
            return null;
        }

        private IEnumerable<CloneDesc> ProcessChild(MethodDeclaration right, INode child, int targetChildCount)
        {
            var children1 = GetExtractableChildren(child);

            return ProcessChildren(right, children1, AstMatchHelper.OscillateWindows(children1.Count), targetChildCount);
        }

        public static List<INode> GetExtractableChildren(INode node)
        {
            var v = new GetImmediateChildrenVisitor();
            node.AcceptVisitor(v, null);
            return v.Children;
        }

        /// <summary>
        /// The INode.Children property is not reliably populated for some annoying reason.  Therefore we 
        /// must have a custom implementation of getting children for each node type.
        /// Recursive extract method won't work on node types that aren't handled in here.
        /// </summary>
        private class GetImmediateChildrenVisitor : AbstractAstVisitor
        {
            public List<INode> Children { get; private set; }

            public GetImmediateChildrenVisitor()
            {
                Children = new List<INode>();
            }

            public override object VisitBlockStatement(BlockStatement bs, object data)
            {
                Children = bs.Children;
                return null;
            }

            public override object VisitIfElseStatement(IfElseStatement ifElseStatement, object data)
            {
                Children = ifElseStatement.TrueStatement.ConvertAll(x => (INode) x);
                return null;
            }

            public override object VisitSwitchStatement(SwitchStatement switchStatement, object data)
            {
                Children = switchStatement.SwitchSections.ConvertAll(x => (INode) x);
                return null;
            }

            public override object VisitSwitchSection(SwitchSection switchSection, object data)
            {
                Children = switchSection.Children;
                return null;
            }

            public override object VisitTryCatchStatement(TryCatchStatement tryCatchStatement, object data)
            {
                Children = tryCatchStatement.StatementBlock.Children;
                return null;
            }
        }
    }
}