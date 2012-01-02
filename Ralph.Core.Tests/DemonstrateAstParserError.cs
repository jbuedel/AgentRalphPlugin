using ICSharpCode.NRefactory.Ast;
using NUnit.Framework;

namespace AgentRalph.Tests
{
    [TestFixture]
    [Description("Demonstrates a parser bug involving local variable declarations with modifiers.  To 'fix' this, I modified Parser.cs directly.  Really need to modify cs.ATG, which generates Parser.cs.")]
    public class DemonstrateAstParserError
    {
        [Test]
        public void WithoutModifier()
        {
            Test(@"  public class CloneInNestedBlock
                    {
                        private void Foo()
                        {
                            double w = 7;
                        }
                    }");
        }

        [Test] //[Ignore("Demonstrates the problem, but fails so is being ignored.")]
        public void WithModifier()
        {
            Test(@"  public class CloneInNestedBlock
                    {
                        private void Foo()
                        {
                            const double w = 7;
                        }
                    }");
        }

        private static void Test(string codeText)
        {
            var unit = AstMatchHelper.ParseToCompilationUnit(codeText);
            var meth = unit.FindMethod("Foo");
            LocalVariableDeclaration child = (LocalVariableDeclaration) meth.Body.Children[0];
            Assert.AreNotEqual(0, child.EndLocation.Line, "Location should be set properly, regardless of modifiers.");
        }
    }
}