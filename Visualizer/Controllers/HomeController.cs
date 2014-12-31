using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AgentRalph;
using AgentRalph.CloneCandidateDetection;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Tests.Ast;
using Microsoft.Ajax.Utilities;
using Visualizer.Models;

namespace Visualizer.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Refaster(string testname)
    {
      var testCaseDir = @"C:\Users\jbuedel\Projects\AgentRalphPlugin\Ralph.Core.Tests\CloneCandidateDetectionTests\TestCases\";
      if (string.IsNullOrEmpty(testname))
      {
        var m = new UnknownTestViewModel {
          Tests = Directory.GetFiles(testCaseDir).Select(Path.GetFileNameWithoutExtension).ToList()
        };
        return View("UnknownTest", m);
      }

      var model = RefasterTests.DoCloneCandidateTest(testCaseDir + testname + ".cs");
      if(model == null) throw new Exception();


      return View(new CloneCandidateTestViewModel(model));
    }

    public ActionResult Index()
    {
//          var file = System.IO.Directory.EnumerateFiles(@"C:\Users\jbuedel\Projects\agentralphplugin\bin\", "*.json").First();
//          var json = System.IO.File.ReadAllText(file);
      return View();
    }

    public static JNode build(JNode tree, int targetid, JNode addition)
    {
      if (tree.id == targetid)
        return new JNode {name = "divergence", children = new[] {tree, addition}, id = 0};

      var children = tree.children.Select(v => build(v, targetid, addition)).ToList();
      return new JNode {name = tree.name, data = tree.data, children = children, id = 0};
    }

    public JsonResult Data()
    {
      var code = "2 + 3 * (5-7)";
      var code2 = "2+3 * (5+7)";

      var expr = AstMatchHelper.ParseToExpression(code);
      var expr2 = AstMatchHelper.ParseToExpression(code2);

      var matchResult = AstMatchHelper.MatchesWithState(expr, expr2);
      var r = build(matchResult.Root.ToJson(),
        matchResult.FailNodeLeft.ToJson().id,
        matchResult.FailNodeRight.ToJson());
      return Json(r, JsonRequestBehavior.AllowGet);
    }

    // This represents the CloneCandidateDetectionTests, which seem to be a 10000 foot view.  That is,
    // it tests from the file level, all the way down through classes, extracting permutations and matching
    // against existing methods on the class.
    // I may want to start with something more specific, for this visualization.  Or maybe, really my problem is
    // often bad extract method operations.   I should set this up to inspect that perhaps?
    public ActionResult CloneFinder()
    {
      var model = new CloneFinderModel();

      string codeText =
        System.IO.File.ReadAllText(
          @"c:\users\jbuedel\Projects\AgentRalphPlugin\Ralph.Core.Tests\CloneCandidateDetectionTests\TestCases\MenuTest.cs");
      model.TestCaseCode = codeText.Split(new[] {System.Environment.NewLine}, StringSplitOptions.None);

      //            System.Diagnostics.Debugger.Break();

      IParser parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, new StringReader(codeText));
      parser.Parse();

      if (parser.Errors.Count > 0)
      {
        model.Errors = parser.Errors.ErrorOutput;
      }

      var comments = parser.Lexer.SpecialTracker.RetrieveSpecials().OfType<Comment>();

      if (comments.Any(x => x.CommentText.TrimStart().StartsWith("Ignore")))
        throw new ApplicationException("Ignored.");

      var begin_comment = comments.FirstOrDefault(x => x.CommentText.Contains("BEGIN"));
      var end_comment = comments.FirstOrDefault(x => x.CommentText.Contains("END"));

      if (begin_comment == null)
        throw new ApplicationException("There was no comment containing the BEGIN keyword.");
      if (end_comment == null)
        throw new ApplicationException("There was no comment containing the END keyword.");

      MethodsOnASingleClassCloneFinder cloneFinder =
        new MethodsOnASingleClassCloneFinder(new OscillatingExtractMethodExpansionFactory());

      // Find the 'largest' extracted method that falls within the comment markers.
      cloneFinder.OnExtractedCandidate += (finder, args) =>
      {
        if (model.Largest == null)
          model.Largest = args.Candidate;
        else if (args.Candidate.ReplacementInvocationInfo.ReplacementSectionStartLine >= begin_comment.StartPosition.Line
                 && args.Candidate.ReplacementInvocationInfo.ReplacementSectionEndLine <= end_comment.EndPosition.Line
                 && args.Candidate.PermutatedMethod.CountNodes() > model.Largest.PermutatedMethod.CountNodes())
          model.Largest = args.Candidate;
      };

      var replacements = cloneFinder.GetCloneReplacements(parser.CompilationUnit);

      model.CloneCount = replacements.Clones.Count;
      for (int i = 0; i < replacements.Clones.Count; i++)
      {
        //TestLog.EmbedPlainText("   ***** Clone #" + i + " *****", replacements.Clones[i].ToString());
      }


      var quickFixInfos = replacements.Clones.Where(IsBetween(begin_comment, end_comment));

      if (!quickFixInfos.Any())
      {
        ModelState.AddModelError("",
          "None of the clones found (there were " + replacements.Clones.Count +
          ") fell inbetween the BEGIN/END markers.");
        // so in this case, match the largest permutation against the Expected or Foo method and print the tree(s)
      }
      else
      {
        var expected_call_snippet =
          begin_comment.CommentText.Substring(begin_comment.CommentText.IndexOf("BEGIN") + 5).Trim();
        if (!string.IsNullOrEmpty(expected_call_snippet))
        {
          var expected_call = ParseUtilCSharp.ParseStatement<Statement>(expected_call_snippet);
          model.ExpectedCallText = expected_call;
          var actual_cal = ParseUtilCSharp.ParseStatement<Statement>(quickFixInfos.First().TextForACallToJanga);
          model.ActualCallText = actual_cal;
          if (expected_call.MatchesPrint(actual_cal))
          {
            ModelState.AddModelError("",
              "The expected call did not match the actual call.  \n\tExpected Call: " + expected_call.Print() + "\n\t" +
              "Actual Call: " + actual_cal.Print());
          }
        }
      }

      return View(model);
    }

    private Func<QuickFixInfo, bool> IsBetween(Comment begin_comment, Comment end_comment)
    {
      return x =>
        x.ReplacementSectionStartLine > begin_comment.StartPosition.Line &&
        x.ReplacementSectionEndLine < end_comment.EndPosition.Line;
    }

    public ActionResult Prototype()
    {
      return View();
    }
  }
}