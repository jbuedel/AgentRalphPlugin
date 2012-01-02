using System.Linq;
using AgentRalph.CloneCandidateDetection;
using NUnit.Framework;

namespace AgentRalph.Tests
{
    [TestFixture]
    public class ExtractMethodCounterTests
    {
        [Test]
        public void TestCounter()
        {
            const string codeText = @"
                        void foo() 
                        {
                            int a = 9 + 7;
                            int b = 9 + 7;
                            int c = 9 + 7;
                        }";

            TestCounterOnSnippet(codeText);
        }

        [Test]
        public void TestCounterWithNestedBlocks()
        {
            const string codeText = @"
                        void foo() 
                        {
                            if (true)
                            {
                                int a = 9 + 7;
                                int b = 9 + 7;
                                int c = 9 + 7;
                            }
                            else
                            {
                                int a = 7 + 9;
                            }
                        }";
            TestCounterOnSnippet(codeText);
        }

        private void TestCounterOnSnippet(string codeText)
        {
            var expansion = new OscillatingExtractMethodRefactoringExpansion();
            var counter = new OscillatingExtractMethodCounter();

            var md = AstMatchHelper.ParseToMethodDeclaration(codeText);

            var methods = expansion.GetAllPossibleExtractedMethods(md, 3);
            var count = counter.CountAllExtractedMethods(md, 3);

            Assert.AreEqual(methods.Count(), count);


            methods = expansion.GetAllPossibleExtractedMethods(md, 1);
            count = counter.CountAllExtractedMethods(md, 1);

            Assert.AreEqual(methods.Count(), count);
        }
    }
}