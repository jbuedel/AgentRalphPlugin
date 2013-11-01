using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AgentRalph.CloneCandidateDetection;

namespace Visualizer.Models
{
  public class CloneFinderModel
  {
    public string Code { get; set; }
    public string Errors { get; set; }
    public int CloneCount { get; set; }
    public CloneDesc Largest { get; set; }
  }
}