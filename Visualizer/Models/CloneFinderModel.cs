using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AgentRalph.CloneCandidateDetection;
using ICSharpCode.NRefactory.Ast;

namespace Visualizer.Models
{
  public class CloneFinderModel
  {
    public string[] TestCaseCode { get; set; }
    public string Errors { get; set; }
    public int CloneCount { get; set; }
    public CloneDesc Largest { get; set; }
    public CloneDesc ExpectedMatchingMethod { get; set; }
    public Statement ExpectedCallText { get; set; }
    public Statement ActualCallText { get; set; }
  }

  public class CloneCandidateTestViewModel
  {
    public CloneCandidateTestViewModel(RefasterTests.CloneCandidateTestViewModel model)
    {
      this.Name = model.Name;
      this.CodeLines = model.CodeLines;
      this.MatchAttempts = model.MatchAttempts.ToList();
      this.Pattern = model.Pattern;
    }

    public Refaster.Pattern Pattern { get; set; }

    public List<Refaster.MatchAttempt> MatchAttempts { get; set; }

    public string[] CodeLines { get; set; }

    public string Name { get; set; }
  }
}