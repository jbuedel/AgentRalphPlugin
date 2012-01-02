using System.Collections.Generic;
using ICSharpCode.NRefactory.Ast;
using SharpRefactoring;

namespace AgentRalph.CloneCandidateDetection
{
    public class OscillatingExtractMethodCounter
    {
        public int CountAllExtractedMethods(MethodDeclaration right, int targetChildCount)
        {
            List<INode> children = right.Body.Children;

            var windows = AstMatchHelper.OscillateWindows(children.Count);

            return ProcessChildren(right, children, windows, targetChildCount);
        }

        private int ProcessChildren(MethodDeclaration right, List<INode> children, IEnumerable<Window> windows, int targetChildCount)
        {
            int counter = 0;
            foreach (var window in windows)
            {
                if (targetChildCount == window.Size)
                {
                    counter++;
                }

                if (window.Size == 1)
                {
                    counter += ProcessChild(right, children[window.Top], targetChildCount);
                }
            }
            return counter;
        }

        private int ProcessChild(MethodDeclaration right, INode child, int targetChildCount)
        {
            var children1 = OscillatingExtractMethodRefactoringExpansion.GetExtractableChildren(child);
            return ProcessChildren(right, children1, AstMatchHelper.OscillateWindows(children1.Count), targetChildCount);
        }
    }
}