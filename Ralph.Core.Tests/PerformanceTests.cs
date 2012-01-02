using System.IO;
using AgentRalph.CloneCandidateDetection;
using NUnit.Framework;

namespace AgentRalph.Tests
{
    [TestFixture]
    public class PerformanceTests
    {
        // To use dotTrace within VS, all projects needs to compile for x86 instead of any CPU.  Very annoying.
        [Test]
        [Ignore("Only used for performance testing.  Not run as part of the normal test suite.")]
        public void PerfTest()
        {
            // The big method in this file has 263 children which translates to > 13000 permutations.  And we do it 4 times.
            string codeText = File.ReadAllText(@"..\..\SharpRefactoring\Src\CSharpMethodExtractor.cs");

            var files = Directory.GetFiles(@"..\..", "*.cs", SearchOption.AllDirectories);
            using (ProgressForm form = new ProgressForm())
            {
                form.Show();

                foreach (var file in files)
                {
                    form.Text = Path.GetFileName(file);

                    codeText = File.ReadAllText(file);

                    MethodsOnASingleClassCloneFinder finder = new MethodsOnASingleClassCloneFinder(new OscillatingExtractMethodExpansionFactory());
                    finder.AddRefactoring(new LiteralToParameterExpansion());

                    finder.MethodStart += form.OnMethodStart;
                    finder.MethodProgress += form.OnMethodProgress;

                    finder.GetCloneReplacements(codeText);

                    finder.MethodStart -= form.OnMethodStart;
                    finder.MethodProgress -= form.OnMethodProgress;
                }
            }


//            var replacements = cloneFinder.GetCloneReplacements(codeText);
//            Console.WriteLine(replacements.Clones.Count + " clones found.");
        }
    }
}