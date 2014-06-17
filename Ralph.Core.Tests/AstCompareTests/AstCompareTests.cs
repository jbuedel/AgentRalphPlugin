using System;
using System.IO;
using ICSharpCode.NRefactory.Ast;
using NUnit.Framework;

namespace AgentRalph.Tests.AstCompareTests
{
    [TestFixture]
    public class AstCompareTests
    {
        [Test][Ignore("Contains C# 4.0 constructs, which we don't support yet.")]
        public void ExhaustiveCsParsesAndMatches()
        {
            // This file from http://blogs.msdn.com/b/kirillosenkov/archive/2010/05/11/updated-c-all-in-one-file.aspx
            var codeText = File.ReadAllText(@"..\..\Ralph.Core.Tests\AstCompareTests\ExhaustiveSampleCode.cs");

            var cu = AstMatchHelper.ParseToCompilationUnit(codeText);

            Assert.That(cu.Matches(cu), Is.True);
        }

        [Test]
        public void ExhaustiveCs2_0ParsesAndMatches()
        {
            var codeText = File.ReadAllText(@"..\..\Ralph.Core.Tests\AstCompareTests\ExhaustiveSampleCode.2.0.cs");

            var cu = AstMatchHelper.ParseToCompilationUnit(codeText);

            Assert.That(cu.Matches(cu), Is.True);            
        }

    }

  [TestFixture]
  public class AstMatchHelperTests
  {
    [Test]
    public void Test()
    {
      var statement = AstMatchHelper.ParseToMethodDeclaration("void pat(){Console.WriteLine(13);}").Body.Children[0];
      statement = ((ExpressionStatement) statement).Expression;
      var expression = AstMatchHelper.ParseToE<Expression>("Console.WriteLine(13)");

      Console.WriteLine(statement);
      Console.WriteLine();
      Console.WriteLine(expression);

      statement.MatchesPrint(expression);
    }
  }
}