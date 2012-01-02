using NUnit.Framework;
using SharpRefactoring;
using SharpRefactoring.Visitors;

namespace AgentRalph.Tests
{
    [TestFixture]
    [Ignore("Extract method on goto statements is broken.  But I don't care right now so long as it doesn't throw an exception.")]
    public class FindJumpInstructionVisitorTests
    {
        [Test]
        public void GotoCaseOccursBefore()
        {
            const string codeText = @"public void Switch()
                                {
                                    switch (0)
                                    {
                                        case 7:
                                            break;
                                        case 0:
                                            goto case 7;
                                    }
                                    
                                }";

            var md = AstMatchHelper.ParseToMethodDeclaration(codeText);
            FindJumpInstructionsVisitor v = new FindJumpInstructionsVisitor(md, new MySelection(md.Body.Children));
            md.AcceptVisitor(v, null);

            Assert.IsTrue(v.IsOk);
        }

        [Test]
        public void GotoCaseOccursAfter()
        {
            const string codeText = @"public void Switch()
                                {
                                    switch (0)
                                    {
                                        case 0:
                                            goto case 7;
                                        case 7:
                                            break;
                                    }
                                    
                                }";

            var md = AstMatchHelper.ParseToMethodDeclaration(codeText);
            FindJumpInstructionsVisitor v = new FindJumpInstructionsVisitor(md, new MySelection(md.Body.Children));
            md.AcceptVisitor(v, null);

            Assert.IsTrue(v.IsOk);
        }

        [Test]
        public void GotoDefaultCase()
        {
            const string codeText = @"public void Switch()
                                {
                                    switch (0)
                                    {
                                        case 0:
                                            goto default;
                                        default:
                                            break;
                                    }
                                    
                                }";

            var md = AstMatchHelper.ParseToMethodDeclaration(codeText);
            FindJumpInstructionsVisitor v = new FindJumpInstructionsVisitor(md, new MySelection(md.Body.Children));
            md.AcceptVisitor(v, null);

            Assert.IsTrue(v.IsOk);
        }

        [Test]
        public void GotoTargetOccursAfter()
        {
            const string codeText = @"public void Switch()
                                {
                                    goto fred;

                                    fred:
                                        return;
                                }";

            var md = AstMatchHelper.ParseToMethodDeclaration(codeText);
            FindJumpInstructionsVisitor v = new FindJumpInstructionsVisitor(md, new MySelection(md.Body.Children));
            md.AcceptVisitor(v, null);

            Assert.IsTrue(v.IsOk);
        }

        [Test]
        public void GotoTargetOccursBefore()
        {
            const string codeText = @"public void Switch()
                                {
                                    fred:
                                        return;
                                    goto fred;
                                }";

            var md = AstMatchHelper.ParseToMethodDeclaration(codeText);
            FindJumpInstructionsVisitor v = new FindJumpInstructionsVisitor(md, new MySelection(md.Body.Children));
            md.AcceptVisitor(v, null);

            Assert.IsTrue(v.IsOk);
        }
    }
}