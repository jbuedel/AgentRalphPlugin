using AgentRalph.CloneCandidateDetection;
using NUnit.Framework;

namespace AgentRalph.Tests.ShallowCloneFinderTests
{
    [TestFixture]
    public class OtherShallowCloneFinderTests
    {
        [Test]
        public void EmptyMethodsDoNotCauseExceptionNorMatch()
        {
            var codeText = @"    
                class EmptyMethods
                {
                    public void Foo() { }
                    public void Bar() { }
                }";

            var factory = new ShallowExpansionFactory();
            var clonefinder = new MethodsOnASingleClassCloneFinder(factory);

            clonefinder.AddRefactoring(new LiteralToParameterExpansion());

            var scanResult = clonefinder.GetCloneReplacements(codeText);

            Assert.That(scanResult.Clones.Count, Is.EqualTo(0), "Empty methods should not match.");
        }

    }
}