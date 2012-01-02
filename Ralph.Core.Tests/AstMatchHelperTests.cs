using ICSharpCode.NRefactory.Ast;
using NUnit.Framework;

namespace AgentRalph.Tests
{
    [TestFixture]
    public class AstMatchHelperTests
    {
        [Test]
        public void ParameterNameSafe_PositiveCase()
        {
            const string reserved_word = "this";
            ParameterDeclarationExpression p = new ParameterDeclarationExpression(new TypeReference("foo"), reserved_word);
            Assert.AreEqual("@this", p.ParameterNameSafe());
        }

        [Test]
        public void ParameterNameSafe_NegativeCase()
        {
            const string not_a_reserverd_word = "not_a_reserverd_word";
            ParameterDeclarationExpression p = new ParameterDeclarationExpression(new TypeReference("foo"), not_a_reserverd_word);
            Assert.AreEqual("not_a_reserverd_word", p.ParameterNameSafe());
        }

        [Test]
        public void TestNodeCounts()
        {
            var target = AstMatchHelper.ParseToMethodDeclaration(@"void Target() { foo(7, 7, 8, 8); }");
            var target2 = AstMatchHelper.ParseToMethodDeclaration(@"void Target() { foo(8, 8, 7, 7); }");

            Assert.That(target.CountNodes(), Is.EqualTo(target2.CountNodes()));
        }
    }
}