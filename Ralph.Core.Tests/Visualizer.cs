using System;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AgentRalph.Tests
{
  public class Visualizer
  {
    public static string ToJson(INode node)
    {
      // I think I'll need to manually write the json.
      return JsonConvert.SerializeObject(node.ToJson());
    }
  }

  [TestFixture]
  public class VisualizerTests
  {
    [Test]
    public void Print()
    {
      Console.WriteLine(Visualizer.ToJson(AstMatchHelper.ParseToExpression("3+2")));
    }
  }
}