using System;
using AgentRalph.CloneCandidateDetection;
using ICSharpCode.NRefactory.Tests.Ast;
using NUnit.Framework;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory;

namespace AgentRalph.Tests.CloneCandidateDetectionTests
{
    // TODO: These may be obsolete now that we support declaring the replacement invocation snippet as part of the marked up test code.
    [TestFixture]
    [Obsolete("Obsolete now that we support declaring the replacement invocation snippet as part of the marked up test code.")]
    public class CloneReplacementInvocationTests
    {
        [Test]
        public void CloneReplacementInvocation_NoParams()
        {
            const string codeText = @"class Test { public int Target() { Console.WriteLine(7); return 9; } 	public void Expected() { Console.WriteLine(7); } }";

            TestCloneReplacementInvocation(codeText, "Expected();");
        }

        [Test]
        public void CloneReplacementInvocation_OneParam_Literal()
        {
            const string codeText = @"class Test { public int Target() { Console.WriteLine(7); return 9; } 	public void Expected(int z) { Console.WriteLine(z); } }";
            const string expected_code = "Expected(7);";

            TestCloneReplacementInvocation(codeText, expected_code);
        }

        [Test]
        public void CloneReplacementInvocation_OneParam_Variable()
        {
            const string codeText = @"class Test { public int Target() { int x = 7; Console.WriteLine(x); return 9; } 	public void Expected(int z) { Console.WriteLine(z); } }";
            const string expected_code = "Expected(x);";

            TestCloneReplacementInvocation(codeText, expected_code);
        }

        [Test]
        public void CloneReplacementInvocation_TwoParam_Literal()
        {
            const string codeText = @"class Test { public int Target() { Console.WriteLine(7 + 3); return 9; } 	public void Expected(int z, int y) { Console.WriteLine(z + y); } }";
            const string expected_code = "Expected(7, 3);";

            TestCloneReplacementInvocation(codeText, expected_code);
        }

        [Test]
        public void CloneReplacementInvocation_TwoParam_Variable()
        {
            const string codeText = @"class Test { public int Target() { int a = 7; int b = 3; Console.WriteLine(a + b); return 9; } 	public void Expected(int z, int y) { Console.WriteLine(z + y); } }";
            const string expected_code = "Expected(a, b);";

            TestCloneReplacementInvocation(codeText, expected_code);
        }

        [Test]
        [Description("When the clone replacement invocation gets some parameters from a literal to param refactoring, it was wiping out any params from the extract method.")]
        public void CloneReplacementInvocation_ParamsFromLiteralDoNotReplaceExistingParams()
        {
            const string codeText = @"class Test { public int Target() { int a = 7; Console.WriteLine(a + 3); return 9; } 	public void Expected(int z, int y) { Console.WriteLine(z + y); } }";
            const string expected_code = "Expected(a, 3);";

            TestCloneReplacementInvocation(codeText, expected_code);
        }

        [Test]
        [Description("Demonstrates a bug where string literals were being passed to invocation sites without quotes.")]
        public void Bug_LiteralStringsNeedQuotes()
        {
            const string codeText = @"class Test { public int Target() { Console.WriteLine(""7"" + ""3""); return 9; } public void Expected(string z, string y) { Console.WriteLine(z + y); } }";
            const string expected_code = @"Expected(""7"", ""3"");";

            TestCloneReplacementInvocation(codeText, expected_code);
        }

        [Test, Description("If the original clone returned a value, then it's repair invocation should as well.")]
        public void IncludeReturnKeywordInRepairIfNecessary()
        {
            const string codeText = @"class Test
                                {
                                    int Target()
                                    {
                                        Console.WriteLine(7);
                                        return 7;
                                    }

                                    int Expected(int i)
                                    {
                                        Console.WriteLine(i);
                                        return i;
                                    }
                                }";

            const string expected_code = @"return Expected(7);";
            TestCloneReplacementInvocation(codeText, expected_code);
        }

        private void TestCloneReplacementInvocation(string codeText, string expected_code)
        {
            var cloneFinder = new MethodsOnASingleClassCloneFinder(new OscillatingExtractMethodExpansionFactory());
            cloneFinder.AddRefactoring(new LiteralToParameterExpansion());
            ScanResult sr = cloneFinder.GetCloneReplacements(codeText);
            Assert.AreEqual(1, sr.Clones.Count, "Expected exactly one clone to be found.");
            Console.WriteLine(sr.Clones[0]);
            var c = sr.Clones[0];
            var expected_call = ParseUtilCSharp.ParseStatement<Statement>(expected_code);
            var actual_cal = ParseUtilCSharp.ParseStatement<Statement>(c.TextForACallToJanga);
            Assert.IsTrue(expected_call.MatchesPrint(actual_cal), "Expected Call: " + expected_call.Print() + "\n" + "Actual Call: " + actual_cal.Print());
        }
    }
}