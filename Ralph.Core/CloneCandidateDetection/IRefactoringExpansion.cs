using System.Collections.Generic;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;

namespace AgentRalph.CloneCandidateDetection
{
    public interface IRefactoringExpansion
    {
        IEnumerable<CloneDesc> FindCandidatesViaRefactoringPermutations(TargetInfo left, CloneDesc refactoredPermy);
    }

    /// <summary>
    /// Holds information about the method that a refactoring is attempting to match.
    /// Really, it's just a wrapper around a <see cref="MethodDeclaration"/> because passing those
    /// was getting confusing.  They kept getting modified.
    /// </summary>
    public class TargetInfo
    {
        public readonly MethodDeclaration Target;
        /// <summary>
        /// Formerly known as 'right'.
        /// </summary>
		public readonly MethodDeclaration CloneContainer;
		
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="cloneContainer">What I have been calling 'right'.  The original method that an extract was performed on.</param>
        public TargetInfo(MethodDeclaration target, MethodDeclaration cloneContainer)
        {
            Target = target;
            CloneContainer = cloneContainer;
        }
        
        public Dictionary<string, List<LocalLookupVariable>> GetLookupTableWithParams()
        {
            return Target.GetLookupTableWithParams();
        }
    }
}