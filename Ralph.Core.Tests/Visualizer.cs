using System;
using System.IO;
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
      var json = Visualizer.ToJson(AstMatchHelper.ParseToExpression("3+2"));
      File.WriteAllText(@"C:\Users\jbuedel\Projects\agentralphplugin\bin\" + DateTime.Now.Ticks + ".json", json);
      Console.WriteLine(json);
    }
  }
}