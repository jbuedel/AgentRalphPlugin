using System.Collections.Generic;
using AgentRalph.CloneCandidateDetection;
using ICSharpCode.NRefactory;
using NUnit.Framework;

namespace AgentRalph.Tests.CloneCandidateDetectionTests
{
    [TestFixture]
    public class RenameLocalVariableRefactoringTests
    {
        [Test]
        public void TestCase001()
        {
            const string codeText =
                @"void Bar()
                    {
                        string bar_str = ""zippy"";
                        Console.Write(bar_str);
                    }";

            RunRenameLocalVariableRefactoringTest(codeText, "bar_str", "jibberjab");
        }

        [Test]
        public void VarInInitializerExprBug()
        {
            const string codeText2 = @"void bar(int x) { int i = x; }";

            RunRenameLocalVariableRefactoringTest(codeText2, "x", "y");
        }

        private void RunRenameLocalVariableRefactoringTest(string codeText, string idStart, string idEnd)
        {
            var foo_meth = AstMatchHelper.ParseToMethodDeclaration(codeText);
            var renameTable = new Dictionary<string, string> {{idStart, idEnd}};
            RenameLocalVariableRefactoring r = new RenameLocalVariableRefactoring(renameTable);

            foo_meth.AcceptVisitor(r, null);

            var expectedCode = codeText.Replace(idStart, idEnd);
            Assert.IsTrue(AstMatchHelper.ParseToMethodDeclaration(expectedCode).Matches(foo_meth), "Expected code: " + expectedCode + " did not match actual code: " + foo_meth.Print());
        }

        [Test]
        [Description("Note that this one actually tests the Expansion (which uses the Refactoring internally).")]
        public void RenameLocalVariableExpansion_IncludesParameters()
        {
            const string codeText =
                @"void Bar(string bar_str)
                    {
                        bar_str += ""zippy"";
                        Console.Write(bar_str);
                    }";

            var foo_meth = AstMatchHelper.ParseToMethodDeclaration(codeText);

            var expected_method_text = codeText.Replace("bar_str", "jibberjab");
            var expected_method = AstMatchHelper.ParseToMethodDeclaration(expected_method_text);

            RenameLocalVariableExpansion r = new RenameLocalVariableExpansion();
            var candidates = r.FindCandidatesViaRefactoringPermutations(new TargetInfo(expected_method, foo_meth), foo_meth);

            Util.AssertExists(candidates, x => expected_method.MatchesPrint(x));
        }

        [Test]
        public void RenameLocalVariableExpansion_Other()
        {
            const string codeText1 = @"void Expected(int i, int i2) { foo(i, 7, i2, 8); }";
            const string codeText2 = @"void Target(System.Int32 i, System.Int32 i1) { foo(i, 7, i1, 8); }";

            var expected = AstMatchHelper.ParseToMethodDeclaration(codeText1);
            var target = AstMatchHelper.ParseToMethodDeclaration(codeText2);

            var r = new RenameLocalVariableExpansion();
            var permutations = r.FindCandidatesViaRefactoringPermutations(new TargetInfo(expected, target), target);

            Util.AssertExists(permutations, x => x.MatchesPrint(expected));
        }
    }
}