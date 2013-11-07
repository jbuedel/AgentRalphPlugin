using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;

namespace AgentRalph.CloneCandidateDetection
{
    public class MethodsOnASingleClassCloneFinder
    {
        public event MethodStartEvent MethodStart;

        private void InvokeMethodStart(MethodStartEventArgs args)
        {
            MethodStartEvent start = MethodStart;
            if (start != null) start(this, args);
        }

        public event MethodProgressEvent MethodProgress;

        private void InvokeMethodProgress(MethodProgressEventArgs args)
        {
            MethodProgressEvent progress = MethodProgress;
            if (progress != null) progress(this, args);
        }

        public event OnExtractedCandidateEvent OnExtractedCandidate;

        private void InvokeOnExtractedCandidate(OnExtractedCandidateEventArgs args)
        {
            OnExtractedCandidateEvent candidate = OnExtractedCandidate;
            if (candidate != null) candidate(this, args);
        }

        public ScanResult GetCloneReplacements(string codeText)
        {
            IParser parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, new StringReader(codeText));
            parser.Parse();

            if (parser.Errors.Count > 0)
            {
                return new ScanResult();
            }

            CompilationUnit unit = parser.CompilationUnit;

            return GetCloneReplacements(unit);
        }

        public ScanResult GetCloneReplacements(CompilationUnit unit)
        {
            var instances = (from typeDec1 in unit.FindAllClasses()
                             from inf1 in ScanForClones(typeDec1)
                             select inf1).ToArray();

            return new ScanResult(instances);
        }

        private IEnumerable<QuickFixInfo> ScanForClones(TypeDeclaration unit)
        {
            var all_methods = unit.FindAllMethods();


            var e = from left in all_methods
                    from right in all_methods
                    where !ReferenceEquals(left, right)
                    select new OscillatingExtractMethodCounter().CountAllExtractedMethods(right, left.Body.Children.Count);

            var expectedMethodsCount = e.Sum();
            DoStartMethodEvent(expectedMethodsCount, "????");


            var enumerable = from left in all_methods
                             from right in all_methods
                             where !ReferenceEquals(left, right)
                             from method in FindMatchingPermutations(left, right)
                             select method;
            return enumerable;
        }

        // TODO: Refactor this out into a method matching specific class.
        private IEnumerable<QuickFixInfo> FindMatchingPermutations(MethodDeclaration left, MethodDeclaration right)
        {
            var method_extractor = GetExpansion();

            var extractedCandidates = method_extractor.GetAllPossibleExtractedMethods(right, left.Body.Children.Count);

            foreach (var candidate in extractedCandidates)
            {
                var replacementSectionStartLine = candidate.Children[candidate.Window.Top].StartLocation.Line;
                var replacementSectionEndLine = candidate.Children[candidate.Window.Bottom].EndLocation.Line;
                var highlightStartLocationColumn = candidate.Children[candidate.Window.Top].StartLocation.Column;

                candidate.ReplacementInvocationInfo = new QuickFixInfo(left, right, candidate.PermutatedMethod, candidate.Window,
                                                                       replacementSectionEndLine,
                                                                       replacementSectionStartLine, highlightStartLocationColumn,
                    // The parameter names of the extracted method match the original context it was extracted from.  IOW, we get the right names for free.
                                                                       candidate.PermutatedMethod.Parameters.Select(x => x.ParameterName as object).ToList());

                DoExtractedCandidateEvent(candidate);

                if (MatchesIgnoreVarNames(left, candidate.PermutatedMethod))
                {
                    yield return candidate.ReplacementInvocationInfo;
                }
                else
                {
                    foreach (var expansion in refactoringExpansions)
                    {
                        IEnumerable<CloneDesc> permutations = expansion.FindCandidatesViaRefactoringPermutations(new TargetInfo(left, right), candidate);
                        foreach (var permutation in permutations)
                        {
                            if (MatchesIgnoreVarNames(left, permutation.PermutatedMethod))
                            {
                                yield return permutation.ReplacementInvocationInfo;
                            }
                        }
                    }
                }

                DoPermutationCompleteEvent(right.Name);
            }
        }

        private IInitialRefactoringExpansion GetExpansion()
        {
            return myExpansionFactory.GetExpansion();
        }

        private static bool MatchesIgnoreVarNames(MethodDeclaration expected, MethodDeclaration target)
        {
            RenameLocalVariableExpansion rename = new RenameLocalVariableExpansion();

            foreach (var candidate in rename.FindCandidatesViaRefactoringPermutations(new TargetInfo(expected, null), target))
            {
                if (expected.Matches(candidate))
                {
                    return true;
                }
            }
            return false;
        }

        private void DoPermutationCompleteEvent(string name)
        {
            InvokeMethodProgress(new MethodProgressEventArgs() { Name = name });
        }

        private void DoStartMethodEvent(int expectedMethodsCount, string name)
        {
            InvokeMethodStart(new MethodStartEventArgs { Name = name, ExpectedMethodsCount = expectedMethodsCount });
        }

        private void DoExtractedCandidateEvent(CloneDesc candidate)
        {
            InvokeOnExtractedCandidate(new OnExtractedCandidateEventArgs { Candidate = candidate });
        }

        private readonly IList<IRefactoringExpansion> refactoringExpansions = new List<IRefactoringExpansion>();
        private readonly IExpansionFactory myExpansionFactory;

        public MethodsOnASingleClassCloneFinder(IExpansionFactory expansionFactory)
        {
            myExpansionFactory = expansionFactory;
        }

        public void AddRefactoring(IRefactoringExpansion re)
        {
            refactoringExpansions.Add(re);
        }
    }

    public interface IExpansionFactory
    {
        IInitialRefactoringExpansion GetExpansion();
    }

    public class OscillatingExtractMethodExpansionFactory : IExpansionFactory
    {
        public IInitialRefactoringExpansion GetExpansion()
        {
            return new OscillatingExtractMethodRefactoringExpansion();
        }
    }

    public delegate void OnExtractedCandidateEvent(object sender, OnExtractedCandidateEventArgs args);

    public class OnExtractedCandidateEventArgs
    {
        public CloneDesc Candidate;
    }

    public delegate void MethodStartEvent(object sender, MethodStartEventArgs args);

    public class MethodStartEventArgs
    {
        public int ExpectedMethodsCount { get; set; }
        public string Name { get; set; }
    }

    public delegate void MethodProgressEvent(object sender, MethodProgressEventArgs args);

    public class MethodProgressEventArgs
    {
        public string Name { get; set; }
    }
}