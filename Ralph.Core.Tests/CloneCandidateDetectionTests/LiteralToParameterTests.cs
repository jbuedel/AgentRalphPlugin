using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AgentRalph.CloneCandidateDetection;
using ICSharpCode.NRefactory;
using NUnit.Framework;
using System.Collections.Generic;
using AgentRalph.Visitors;

namespace AgentRalph.Tests.CloneCandidateDetectionTests
{
    [TestFixture]
    public class LiteralToParameterTests
    {
        [Test]
        public void SingleLiteralInstance()
        {
            const string codeText = @"void Target() { Console.WriteLine(7); }";
            var target = AstMatchHelper.ParseToMethodDeclaration(codeText);

            LiteralToParameterExpansion exp = new LiteralToParameterExpansion();
            var perms = exp.FindCandidatesViaRefactoringPermutations(target);

            Assert.IsTrue(perms.Any(p => p.PermutatedMethod.MatchesPrint(AstMatchHelper.ParseToMethodDeclaration(@"void Expected(int i) { Console.WriteLine(i); }"))));
        }

        [Test]
        public void SingleStringLiteralInstance()
        {
            const string codeText = @"void Target() { Console.WriteLine(""7""); }";
            var target = AstMatchHelper.ParseToMethodDeclaration(codeText);

            LiteralToParameterExpansion exp = new LiteralToParameterExpansion();
            var perms = exp.FindCandidatesViaRefactoringPermutations(target);


            Assert.IsTrue(perms.Any(p => p.PermutatedMethod.MatchesPrint(AstMatchHelper.ParseToMethodDeclaration(@"void Expected(string i) { Console.WriteLine(i); }"))));
        }

        [Test]
        public void MultipleCopiesOfALiteral()
        {
            const string codeText = @"void Target() { Console.WriteLine(7); Console.Write(7); }";
            var target = AstMatchHelper.ParseToMethodDeclaration(codeText);

            LiteralToParameterExpansion exp = new LiteralToParameterExpansion();
            var perms = exp.FindCandidatesViaRefactoringPermutations(target);

            AssertExists(perms, p => p.PermutatedMethod.MatchesPrint(AstMatchHelper.ParseToMethodDeclaration(@"void Expected(int i) { Console.WriteLine(i); Console.Write(i); }")), "Expected a permutation where all possible literals were replaced with a single parameter.");
        }

        private void AssertExists(IEnumerable<CloneDesc> perms, Func<CloneDesc, bool> o)
        {
            Util.AssertExists(perms, o);
        }

        private void AssertExists(IEnumerable<CloneDesc> perms, Func<CloneDesc, bool> o, string msg)
        {
            Util.AssertExists(perms, o, msg);
        }

        [Test]
        public void AllPossiblePermutationsOfASingleLiteral()
        {
            const string codeText = @"void Target() { Console.WriteLine(7); Console.Write(7); }";
            var target = AstMatchHelper.ParseToMethodDeclaration(codeText);

            LiteralToParameterExpansion exp = new LiteralToParameterExpansion();
            var perms = exp.FindCandidatesViaRefactoringPermutations(target);

            AssertExists(perms, p => p.MatchesIgnoreVarNames(@"void Expected(int i) { Console.WriteLine(i); Console.Write(i); }"), "Expected a permutation where all possible literals were replaced with a single parameter.");
            AssertExists(perms, p => p.MatchesIgnoreVarNames(@"void Expected(int i) { Console.WriteLine(i); Console.Write(7); }"), "Expected a permutation where the first literal was replaced, but not the second.");
            AssertExists(perms, p => p.MatchesIgnoreVarNames(@"void Expected(int i) { Console.WriteLine(7); Console.Write(i); }"), "Expected a permutation where the second literal was replaced, but not the first.");

            // Print out the permutations, just for reference.
            for (int i = 0; i < perms.ToArray().Length; i++)
            {
                var perm = perms.ToArray()[i];
                TestLog.EmbedPlainText("perm" + i, perm.PermutatedMethod.Print());
            }
        }

        [Test, Ignore("TODO: Giving each literal instance it's own param is not implemented, as opposed to a single variable for all instances of a literal.")]
        public void SingleLiteralCanBeMultipleParams()
        {
            const string codeText = @"void Target() { Console.WriteLine(7); Console.Write(7); }";
            var target = AstMatchHelper.ParseToMethodDeclaration(codeText);

            LiteralToParameterExpansion exp = new LiteralToParameterExpansion();
            var perms = exp.FindCandidatesViaRefactoringPermutations(target);

            AssertExists(perms, p => p.MatchesIgnoreVarNames(@"void Expected(int i, int i1) { Console.WriteLine(i); Console.Write(i1); }"), "Expected a permutation where each literal gets it's own parameter, regardless of redundancy.");
        }

        [Test]
        public void AllPossiblePermutationsOfMultipleLiterals()
        {
            const string codeText = @"void Target() { foo(7, 7, 8, 8); }";
            var target = AstMatchHelper.ParseToMethodDeclaration(codeText);

            LiteralToParameterExpansion exp = new LiteralToParameterExpansion();
            var perms = exp.FindCandidatesViaRefactoringPermutations(target);

            foreach (var perm in perms)
            {
                Debug.WriteLine(perm.PermutatedMethod.Print());
            }

            AssertExists(perms, p => p.MatchesIgnoreVarNames(@"void Expected(int i) { foo(i, 7, 8, 8); }"));
            AssertExists(perms, p => p.MatchesIgnoreVarNames(@"void Expected(int i) { foo(7, i, 8, 8); }"));
            AssertExists(perms, p => p.MatchesIgnoreVarNames(@"void Expected(int i) { foo(7, 7, i, 8); }"));
            AssertExists(perms, p => p.MatchesIgnoreVarNames(@"void Expected(int i) { foo(7, 7, 8, i); }"));

            AssertExists(perms, p => p.MatchesIgnoreVarNames(@"void Expected(int i) { foo(i, i, 8, 8); }"));
            AssertExists(perms, p => p.MatchesIgnoreVarNames(@"void Expected(int i) { foo(7, 7, i, i); }"));

            AssertExists(perms, p => p.MatchesIgnoreVarNames(@"void Expected(int i, int i2) { foo(i, 7, i2, 8); }"));
            AssertExists(perms, p => p.MatchesIgnoreVarNames(@"void Expected(int i, int i2) { foo(i, 7, 8, i2); }"));
            AssertExists(perms, p => p.MatchesIgnoreVarNames(@"void Expected(int i, int i2) { foo(7, i, i2, 8); }"));
            AssertExists(perms, p => p.MatchesIgnoreVarNames(@"void Expected(int i, int i2) { foo(7, i, 8, i2); }"));

            AssertExists(perms, p => p.MatchesIgnoreVarNames(@"void Expected(int i, int i2) { foo(7, i, 8, i2); }"));
            AssertExists(perms, p => p.MatchesIgnoreVarNames(@"void Expected(int i, int i2) { foo(i, 7, 8, i2); }"));
            AssertExists(perms, p => p.MatchesIgnoreVarNames(@"void Expected(int i, int i2) { foo(7, i, i2, 8); }"));
            AssertExists(perms, p => p.MatchesIgnoreVarNames(@"void Expected(int i, int i2) { foo(i, 7, i2, 8); }"));

            AssertExists(perms, p => p.MatchesIgnoreVarNames(@"void Expected(int i, int i2) { foo(i, i, i2, i2); }"));

            // I think that's all of them.  2^n-1 where n=4 is 15.
        }

        [Test]
        public void SingleInstanceOfDifferentLiterals()
        {
            const string codeText = @"void Target() { Console.WriteLine(7); Console.Write(8); }";
            var target = AstMatchHelper.ParseToMethodDeclaration(codeText);

            LiteralToParameterExpansion exp = new LiteralToParameterExpansion();
            var perms = exp.FindCandidatesViaRefactoringPermutations(target);

            AssertExists(perms, p => p.PermutatedMethod.MatchesPrint(AstMatchHelper.ParseToMethodDeclaration(@"void Expected(int i, int i1) { Console.WriteLine(i); Console.Write(i1); }")), "Expected each literal to get it's own parameter, since they were not the same literal.");
        }

        [Test]
        public void MultipleInstancesOfDifferentLiterals()
        {
            const string codeText = @"void Target() { Console.WriteLine(7); Console.Write(8); Console.WriteLine(7); Console.Write(8); }";
            var target = AstMatchHelper.ParseToMethodDeclaration(codeText);

            LiteralToParameterExpansion exp = new LiteralToParameterExpansion();
            var perms = exp.FindCandidatesViaRefactoringPermutations(target);

            AssertExists(perms, p => p.PermutatedMethod.MatchesPrint(AstMatchHelper.ParseToMethodDeclaration(@"void Expected(int i, int i1) { Console.WriteLine(i); Console.Write(i1); Console.WriteLine(i); Console.Write(i1);}")), "Expected literals with same value to get the same parameter.");
        }

        [Test, Description("The PrimitiveExpression object for null was causing us to get a NullReferenceException.")]
        public void PrimitiveExpressionForNullBug()
        {
            const string codeText = @"public void Foo()
                                {
                                    if (member != null)
                                    {
                                        
                                    }
                                }";
            var exp = new LiteralToParameterExpansion();
            var md = AstMatchHelper.ParseToMethodDeclaration(codeText);
            exp.FindCandidatesViaRefactoringPermutations(md).ToList();
        }

        [Test][Ignore]
        public void PerformanceTest()
        {
            const string codeFile = @"D:\Projects\AgentRalph.hg\Ralph.Test.Project\Ralph.Test.Project\Menus.cs";
            string codeText = File.ReadAllText(codeFile);

            IParser parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, new StringReader(codeText));
            parser.Parse();

            if (parser.Errors.Count > 0)
                throw new ApplicationException(string.Format("Expected no errors in the input code. Code was: {0} ErrorOutput: {1}", codeText,
                                                             parser.Errors.ErrorOutput));

            MethodsOnASingleClassCloneFinder cloneFinder = new MethodsOnASingleClassCloneFinder(new OscillatingExtractMethodExpansionFactory());
            cloneFinder.AddRefactoring(new LiteralToParameterExpansion());

            var replacements = cloneFinder.GetCloneReplacements(parser.CompilationUnit);

            TestLog.EmbedPlainText(replacements.Clones.Count + " clones found.");
            foreach (var clone in replacements.Clones)
            {
                TestLog.EmbedPlainText(string.Format("{0}-{1}: {2}", clone.ReplacementSectionStartLine, clone.ReplacementSectionEndLine, clone.TextForACallToJanga));
            }

        }

        [Test]
        public void AstComparisonIgnoreLiteralsVisitorTest()
        {
            var visitor = new AstComparisonIgnoreLiteralsVisitor();

            var md = AstMatchHelper.ParseToMethodDeclaration(@"void foo() { int i = 1 + 2; }");

            var md_different = AstMatchHelper.ParseToMethodDeclaration(@"void foo() { int i = 2 + 1; }");

            md.AcceptVisitor(visitor, md_different);
            
            Assert.That(visitor.Match, Is.True);
        }
    }

    internal static class LiteralToParameterTestsHelper
    {
        public static bool MatchesIgnoreVarNames(this CloneDesc p, string expectedCode)
        {
            RenameLocalVariableExpansion rename = new RenameLocalVariableExpansion();

            var expected = AstMatchHelper.ParseToMethodDeclaration(expectedCode);

            foreach (var prd in rename.FindCandidatesViaRefactoringPermutations(new TargetInfo(expected, p.PermutatedMethod), p.PermutatedMethod))
            {
                if (expected.Matches(prd))
                {
                    return true;
                }
            }
            return false;
        }
    }
}