using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using NUnit.Framework;
using SharpRefactoring;

namespace AgentRalph.Tests.ExtractMethodTests
{
    [TestFixture]
    public class ExtractMethodTestFactory
    {
        public static IEnumerable<TestCaseData> ExtractMethodTestFactoryMethod()
        {
            string[] files = Directory.GetFiles(@"..\..\Ralph.Core.Tests\ExtractMethodTests\TestCases", "*.cs");
            foreach (var file in files)
            {
                string codefile = Path.GetFullPath(file);
                string codeText = File.ReadAllText(codefile);


                var testCaseData = new TestCaseData(codeText);
                testCaseData.SetName(Path.GetFileNameWithoutExtension(codefile));
                testCaseData.SetDescription(ParseDesc(codeText));


                yield return testCaseData;
            }
        }

        [Test, TestCaseSource("ExtractMethodTestFactoryMethod")]
        public static void ExtractMethodTest(string codeText)
        {
            if (ParseDesc(codeText).StartsWith("Ignore", true, null)) // TODO: Would be better to assign TestOutcome.Ignored to the TestCase instance directly.
                Assert.Ignore();

            var parser = ParseCSharp(codeText);

            var targetMethod = parser.CompilationUnit.FindMethod("Target");
            var expectedMethod = parser.CompilationUnit.FindMethod("Expected");

            var markedUpSection = FindMarkedUpAstSection(targetMethod, parser.Lexer.SpecialTracker.RetrieveSpecials());

            CSharpMethodExtractor extractor = new CSharpMethodExtractor();
            var success = extractor.Extract(targetMethod, markedUpSection.Window);
            Assert.IsTrue(success, "The extract method operation failed.");

            TestLog.EmbedPlainText("Target", targetMethod.Print());
            TestLog.EmbedPlainText("Expected", expectedMethod.Print());
            TestLog.EmbedPlainText("Actual extracted", extractor.ExtractedMethod.Print());

            Assert.IsTrue(expectedMethod.Matches(extractor.ExtractedMethod), "Expected the extracted method to match method named 'Expected'");
        }

        private static string ParseDesc(string codeText)
        {
            IEnumerable<string> enumerable =
                codeText.Split(Environment.NewLine.ToArray()).TakeWhile(line => line.TrimStart().StartsWith("//")).Select(line => line.TrimStart(' ', '/'));
            return string.Join(" ", enumerable.ToArray());
        }

        [Test]
        public void TestFindMarkedUpAst1()
        {
            const string codeText =
                @"	
                            class Test001
	                        {
		                        string Foo()
		                        {
			                        string s = string.Empty;
			                        int i = 0;
			                        /* BEGIN s = Bar(s,i);*/
			                        s += ""Hello "";
			                        s += ""world."";
			                        s += i;
			                        /* END */
			                        return s;
		                        }
                            }";

            Test_FindMarkedUpAstSection(codeText, 2, 4);
        }

        [Test]
        public void TestFindMarkedUpAst2()
        {
            const string codeText =
                @"	
                            class Test001
	                        {
		                        string Foo()
		                        {
			                        string s = string.Empty;
                                    /* BEGIN */
			                        int i = 0;
			                        s += ""Hello "";
			                        s += ""world."";
                                    /* END */
			                        s += i;
			                        return s;
		                        }
                            }";

            Test_FindMarkedUpAstSection(codeText, 1, 3);
        }

        private static void Test_FindMarkedUpAstSection(string codeText, int startIndex, int endIndex)
        {
            IParser parser = ParseCSharp(codeText);

            var code_text_ast = parser.CompilationUnit;
            var method = code_text_ast.FindMethod("Foo");

            var p = FindMarkedUpAstSection(method, parser.Lexer.SpecialTracker.RetrieveSpecials());

            AssertForAll(p.Marked, x => method.Body.Children.IndexOf(x) >= startIndex && method.Body.Children.IndexOf(x) <= endIndex,
                         "All elements of 'Marked' should have fallen within the markup.");
            Assert.AreEqual(new Window(startIndex, endIndex), p.Window);
        }

        private static void AssertForAll(IEnumerable<INode> marked, Func<INode, bool> func, string s)
        {
            foreach (var node in marked)
            {
                Assert.IsTrue(func(node), s);
            }
        }

        private static IParser ParseCSharp(string codeText)
        {
            IParser parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, new StringReader(codeText));
            parser.Parse();

            if (parser.Errors.Count != 0)
            {
                TestLog.EmbedPlainText("CodeText", codeText);
                TestLog.EmbedPlainText("Parse errors:", parser.Errors.ErrorOutput);
                Assert.Fail("Expected no parse errors.");
            }
            return parser;
        }

        private static MarkedUpSection FindMarkedUpAstSection(MethodDeclaration target_method, List<ISpecial> specials)
        {
            Location markupStart = CommentContaining(specials, "BEGIN").EndPosition;
            Location markupEnd = CommentContaining(specials, "END").StartPosition;

            var nodes = from c in target_method.Body.Children
                        where c.StartLocation >= markupStart && c.EndLocation <= markupEnd
                        select c;

            return new MarkedUpSection(target_method.Body.Children, nodes.ToList());
        }

        private static Comment CommentContaining(List<ISpecial> specials, string value)
        {
            var comment = specials.OfType<Comment>().FirstOrDefault(x => x.CommentText.Contains(value));
            if (comment == null)
                Assert.Inconclusive("There was no comment containing the text '" + value + "'.");
            return comment;
        }

        private class MarkedUpSection
        {
            public MarkedUpSection(IList<INode> children, IList<INode> marked)
            {
                Marked = marked;
                Window = new Window(children.IndexOf(marked.First()), children.IndexOf(marked.Last()));
            }

            /// <summary>
            /// The set of nodes contained in BEGIN/END tags.
            /// </summary>
            public IList<INode> Marked { get; private set; }

            public Window Window { get; private set; }
        }
    }
}