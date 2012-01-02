using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;

namespace AgentRalph.CloneCandidateDetection
{
    public class RenameLocalVariableExpansion : IRefactoringExpansion
    {
        public IEnumerable<CloneDesc> FindCandidatesViaRefactoringPermutations(TargetInfo left, CloneDesc refactoredPermy)
        {
            MethodDeclaration right = refactoredPermy.PermutatedMethod;
            
            var permutations = FindCandidatesViaRefactoringPermutations(left, right);

            return permutations.Select(x => new CloneDesc(x, refactoredPermy, new QuickFixInfo(refactoredPermy.ReplacementInvocationInfo,
                                                                                                                                                                          left.CloneContainer.Parameters.Select(x1 => x1.ParameterName as object).ToList()))
                                                                                         {
                                                                                             ReplacementInvocation = refactoredPermy.ReplacementInvocation
                                                                                         });
        }

        public IEnumerable<MethodDeclaration> FindCandidatesViaRefactoringPermutations(TargetInfo left, MethodDeclaration right)
        {
            right = right.DeepCopy();
            /* 
             * Steps to change one to match the other.
             *      Get the lookup table for left.
             *      Get the lookup table for right.
             *      Loop through left, renaming the corresponding right var as you go (don't worry about collisions yet).
             * Compare.
             * 
             * This is essentially a normalization of one to the other.
             */
            Dictionary<string, List<LocalLookupVariable>> left_table = left.GetLookupTableWithParams();
            Dictionary<string, List<LocalLookupVariable>> right_table = right.GetLookupTableWithParams();

            if (left_table.Keys.Count == right_table.Keys.Count)
            {
                IDictionary<string, string> renames = new Dictionary<string, string>();

                for (int i = 0; i < left_table.Count; i++)
                {
                    var left_var_name = left_table.Keys.ToArray()[i];
                    var right_var_name = right_table.Keys.ToArray()[i];

                    // current name => new name
                    renames.Add(right_var_name, left_var_name);
                }

                RenameLocalVariableRefactoring r = new RenameLocalVariableRefactoring(renames);
                right.AcceptVisitor(r, null);

                yield return right;
            }
        }
    }
}