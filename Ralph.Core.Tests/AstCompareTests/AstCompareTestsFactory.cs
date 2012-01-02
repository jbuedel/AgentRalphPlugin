using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.NRefactory.Ast;
using NUnit.Framework;
using AgentRalph.Visitors;

namespace AgentRalph.Tests.AstCompareTests
{
    /// <summary>
    /// These tests exercise only the comparison visitor.
    /// 
    /// This class uses the MbUnit feature of dynamic test generation to create a test on the fly for each *.cs file in the RowTestsData directory.
    /// The advantage of having this test data included as buildable cs files (instead of strings or resources) is that we get R# analysis and 
    /// guaranteed compilation as we author the tests.  Ability to compile is a precondition of my AstCompare functionality.
    /// </summary>
    [TestFixture]
    public class AstCompareTestsFactory
    {
        public static IEnumerable<TestCaseData> Suite()
        {
            string[] files = Directory.GetFiles(@"..\..\Ralph.Core.Tests\AstCompareTests\TestCases", "*.cs");
            foreach (string codefile in files)
            {
                string codefile1 = Path.GetFullPath(codefile);
                var tcase = new TestCaseData(codefile1);
                tcase.SetName(Path.GetFileNameWithoutExtension(codefile));
                yield return tcase;
            }
        }

        [Test, TestCaseSource("Suite")]
        public void SimpleTest(string codefile)
        {
//            System.Diagnostics.Debugger.Break();
            using (TextReader text = File.OpenText(codefile))
            {
                Result expectedResult;

                string codeText = text.ReadToEnd();
                const string fail_prefix = "// NotMatch";
                const string pass_prefix = "// Match";
                int prefix_len;
                if (codeText.StartsWith(fail_prefix))
                {
                    expectedResult = Result.NotMatch;
                    prefix_len = fail_prefix.Length;
                }
                else if (codeText.StartsWith(pass_prefix))
                {
                    expectedResult = Result.Match;
                    prefix_len = pass_prefix.Length;
                }
                else if (codeText.StartsWith("// Ignore"))
                {
                    Assert.Ignore(); // TODO: Put the ignored message here.
                    return;
                }
                else
                {
                    Assert.Inconclusive("First line of the file must be a // comment followed by 'Match' or 'NotMatch', followed by an optional message (which will be displayed upon failure.");
                    return;
                }

                string msg = codeText.Substring(prefix_len, codeText.IndexOf(System.Environment.NewLine) - prefix_len);

                ExecuteDuplicationDetection(expectedResult, codeText, msg);
            }
        }

        private static void ExecuteDuplicationDetection(Result expectedResult, string codeText, string msgFromCodeComment)
        {
            TestLog.EmbedPlainText("The code", codeText);

            CompilationUnit cu = AstMatchHelper.ParseToCompilationUnit(codeText);

            // We require the first class in the file to have the Foo & Bar methods.
            var classes = cu.FindAllClasses();
            Assert.That(classes.Count(), Is.GreaterThan(0), "Expected at least one class to be in the test cs file.");

            // Expect two methods, Foo & Bar.
            IndexableMethodFinderVisitor visitor = new IndexableMethodFinderVisitor();
            classes.First().AcceptVisitor(visitor, null);

            Assert.AreEqual(visitor.FooMethod.Name, "Foo", "Expected a method named Foo.");
            Assert.AreEqual(visitor.BarMethod.Name, "Bar", "Expected a method named Bar.");

            AstComparisonVisitor cv = new AstComparisonVisitor();
            visitor.BarMethod.AcceptVisitor(cv, visitor.FooMethod);

            if (expectedResult == Result.Match)
                Assert.IsTrue(cv.Match, "Expected Foo & Bar to match: " + msgFromCodeComment);
            else
                Assert.IsFalse(cv.Match, "Expected Foo & Bar to not match: " + msgFromCodeComment);
        }
    }

    public enum Result
    {
        Match,
        NotMatch
    }
}