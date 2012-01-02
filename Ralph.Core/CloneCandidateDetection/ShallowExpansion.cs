using System.Collections.Generic;
using ICSharpCode.NRefactory.Ast;
using SharpRefactoring;

namespace AgentRalph.CloneCandidateDetection
{
    class ShallowExpansion : IInitialRefactoringExpansion
    {
        public IEnumerable<CloneDesc> GetAllPossibleExtractedMethods(MethodDeclaration right, int targetChildCount)
        {
            var children = right.Body.Children;

            if(children.Count == 0)
                yield break;

            var window = new Window(0, children.Count - 1);


            // This way fixes the unused parameter problem, but causes the Target and Expected to be reversed.
/*
            MethodDeclaration md = right.DeepCopy();
            if (md != null)
                yield return new CloneDesc(md, window, children) { ReplacementInvocation = new CSharpMethodExtractor().GetCall(right, right, new VariableDeclaration("heyman")) };
*/


            // But this way fixes the reversed problem, but leaves the unused param problem.
            var desc = OscillatingExtractMethodRefactoringExpansion.ExtractMethodAsCloneDesc(window, right, children);
            if (desc != null) yield return desc;           

        }
    }

    public class ShallowExpansionFactory : IExpansionFactory
    {
        public IInitialRefactoringExpansion GetExpansion()
        {
            return new ShallowExpansion();
        }
    }
}