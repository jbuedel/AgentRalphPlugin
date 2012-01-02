using System;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;
using NUnit.Framework;
using SharpRefactoring;
using System.Collections.Generic;

namespace AgentRalph.Tests.ExtractMethodTests
{
    [TestFixture]
    public class OtherExtractMethodTests
    {
        [Test]
        [Description("For some reason the blank line between the final node and the closing brace mattered.   If it wasn't there the test failed.  I removed the -1's from MethodExtractorBase.IsInSel which fixed it because then the parameter was properly included.  I couldn't duplicate this problem in #D.")]
        // This test can't go in the generated tests b/c the markup actually hides the flaw.
        public void WhiteSpaceMatters()
        {
            RunWhitespaceTest("\n");
            RunWhitespaceTest("");
        }

        private void RunWhitespaceTest(string whitespace)
        {
            var target = AstMatchHelper.ParseToMethodDeclaration(
                string.Format(@"public void Target()
                    {{
                        int x = 7;
                        Console.WriteLine(x);
                        Console.WriteLine(x);
                    {0}}}", whitespace));

            CSharpMethodExtractor extractor = new CSharpMethodExtractor();
            extractor.Extract(target, new Window(1, 2));

            Assert.AreEqual(1, extractor.ExtractedMethod.Parameters.Count, "Expected a new method with exactly one parameter.");
        }

        [Test]
        public void CanBePerformedOnArbitrarilyDeepNodes()
        {
            const string codeText = @"                
                using System;
                public class One
                {
                    void Foo()
                    {
                        double w = 7;
                        double l = 8;

                        if (DateTime.Now.Day == 3)
                        {
                            Console.WriteLine(""stuff"");
                            Console.WriteLine(""stuff"");
                        }
                        double area = l*w;
                    }
                    void Bar()
                    {
                        Console.WriteLine(""stuff"");                        
                    }
                }";
            var code_text_ast = AstMatchHelper.ParseToCompilationUnit(codeText);

            IndexableMethodFinderVisitor v = new IndexableMethodFinderVisitor();
            code_text_ast.AcceptVisitor(v, null);
            MethodDeclaration method = v.Methods["Foo"];

            FindFirstIfElseVisitor v2 = new FindFirstIfElseVisitor();
            method.AcceptVisitor(v2, null);
            IfElseStatement ifelse_stmt = v2.IfElse;

            List<Statement> statements = ifelse_stmt.TrueStatement;
            Assert.AreEqual(1, statements.Count,
                            "Expect TrueStatement to always return a single element, and it's a block.");
            Assert.IsInstanceOf<BlockStatement>(statements[0], "Expect TrueStatement to always return a single element, and it's a block.");

            BlockStatement block = (BlockStatement) statements[0];

            CSharpMethodExtractor extractor = new CSharpMethodExtractor();
            var success = extractor.Extract(method, new Window(0, 0), block.Children);
            Assert.IsTrue(success);

            Console.WriteLine(extractor.ExtractedMethod.Print());

            MethodDeclaration expected_method = v.BarMethod;
            Assert.IsTrue(expected_method.Matches(extractor.ExtractedMethod),
                          "The expected AST did not match the actual extracted AST.\nExpected: {0} \nActual:{1}",
                          expected_method.Print(), extractor.ExtractedMethod.Print());
        }

        private class FindFirstIfElseVisitor : AbstractAstVisitor
        {
            public override object VisitIfElseStatement(IfElseStatement ifElseStatement, object data)
            {
                IfElse = ifElseStatement;
                return null;
            }

            public IfElseStatement IfElse { get; private set; }
        }
    }
}