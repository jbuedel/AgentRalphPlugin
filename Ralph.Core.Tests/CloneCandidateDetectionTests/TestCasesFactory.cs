using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AgentRalph.CloneCandidateDetection;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Tests.Ast;
using NUnit.Framework;

namespace AgentRalph.Tests.CloneCandidateDetectionTests
{
    /// <summary>
    /// These tests confirm that the clone detection finds an expected match by confirming that at 
    /// least one clone was found that fell within the BEGIN/END tags.
    /// </summary>
    [TestFixture]
    public class TestCasesFactory : CloneCandidateDetectionTestBase 
    {
        public static IEnumerable<TestCaseData> MyCases
        {
            get
            {
                var files = Directory.GetFiles(@"..\..\Ralph.Core.Tests\CloneCandidateDetectionTests\TestCases", "*.cs");

                MethodsOnASingleClassCloneFinder clonefinder = new MethodsOnASingleClassCloneFinder(new OscillatingExtractMethodExpansionFactory());
                clonefinder.AddRefactoring(new LiteralToParameterExpansion());

                return ConvertCodeFileToTestCaseData(files,clonefinder);
            }
        }
    }

    public abstract class CloneCandidateDetectionTestBase 
    {
        protected static IEnumerable<TestCaseData> ConvertCodeFileToTestCaseData(string[] files, MethodsOnASingleClassCloneFinder clonefinder)
        {
            foreach (var file in files)
            {
                var codefile = Path.GetFullPath(file);
                var codeText = File.ReadAllText(codefile);
                var tcase = new TestCaseData(codefile, clonefinder);

                var desc = string.Join(" ", (from str11 in codeText.Split(Environment.NewLine.ToCharArray())
                                             where str11.TrimStart().StartsWith("//")
                                             select str11.Trim().TrimStart('/')).ToArray()).Trim();
                tcase.SetDescription(desc);
                tcase.SetName(Path.GetFileNameWithoutExtension(file));
                    
                if(desc.StartsWith("Ignore"))
                    tcase.Ignore(desc);

                yield return tcase;
            }
        }

        [Test, TestCaseSource("MyCases")]
        public void DoCloneDetection(string codefile, MethodsOnASingleClassCloneFinder cloneFinder)
        {
            string codeText = File.ReadAllText(codefile);
            TestLog.EmbedPlainText("The Code", codeText);

//            System.Diagnostics.Debugger.Break();

            IParser parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, new StringReader(codeText));
            parser.Parse();

            if (parser.Errors.Count > 0)
                throw new ApplicationException(string.Format("Expected no errors in the input code. Code was: {0} ErrorOutput: {1}", codeText,
                                                             parser.Errors.ErrorOutput));

            var comments = parser.Lexer.SpecialTracker.RetrieveSpecials().OfType<Comment>();

            if (comments.Any(x => x.CommentText.TrimStart().StartsWith("Ignore")))
                throw new ApplicationException("Ignored.");

            var begin_comment = comments.FirstOrDefault(x => x.CommentText.Contains("BEGIN"));
            var end_comment = comments.FirstOrDefault(x => x.CommentText.Contains("END"));

            if (begin_comment == null)
                throw new ApplicationException("There was no comment containing the BEGIN keyword.");
            if (end_comment == null)
                throw new ApplicationException("There was no comment containing the END keyword.");

            CloneDesc largest = null;

            cloneFinder.OnExtractedCandidate += (finder, args)=>
                                                    {
                                                        if (largest == null)
                                                            largest = args.Candidate;
                                                        else if (args.Candidate.ReplacementInvocationInfo.ReplacementSectionStartLine >= begin_comment.StartPosition.Line
                                                             && args.Candidate.ReplacementInvocationInfo.ReplacementSectionEndLine <= end_comment.EndPosition.Line
                                                            && args.Candidate.PermutatedMethod.CountNodes() > largest.PermutatedMethod.CountNodes())
                                                            largest = args.Candidate;

                                                    };


            var replacements = cloneFinder.GetCloneReplacements(parser.CompilationUnit);

            int clone_count = replacements.Clones.Count;

            TestLog.EmbedPlainText(clone_count + " clones found.");
            for (int i = 0; i < replacements.Clones.Count; i++)
            {
                TestLog.EmbedPlainText("   ***** Clone #" + i + " *****", replacements.Clones[i].ToString());
            }

            TestLog.EmbedPlainText("The largest permutation found within the markers was:", largest.PermutatedMethod.Print());

            var quickFixInfos = replacements.Clones.Where(Predicate(begin_comment, end_comment));

            Assert.IsTrue(quickFixInfos.Count() > 0, "None of the clones found (there were " + clone_count + ") fell inbetween the BEGIN/END markers.");

            var expected_call_snippet = begin_comment.CommentText.Substring(begin_comment.CommentText.IndexOf("BEGIN")+5).Trim();
            if(!string.IsNullOrEmpty(expected_call_snippet))
            {
                var expected_call = ParseUtilCSharp.ParseStatement<Statement>(expected_call_snippet);
                Assert.That(expected_call_snippet, Is.StringContaining("pattern("), "The pattern (replacement) function must be named 'pattern'");
                var actual_cal = ParseUtilCSharp.ParseStatement<Statement>(quickFixInfos.First().TextForACallToJanga);
                Assert.IsTrue(expected_call.MatchesPrint(actual_cal), "The expected call did not match the actual call.  \n\tExpected Call: " + expected_call.Print() + "\n\t" + "Actual Call: " + actual_cal.Print());
            }
        }

        private Func<QuickFixInfo, bool> Predicate(Comment begin_comment, Comment end_comment)
        {
            return x =>
                   x.ReplacementSectionStartLine > begin_comment.StartPosition.Line &&
                   x.ReplacementSectionEndLine < end_comment.EndPosition.Line;
        }
    }
}