using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;
using NUnit.Framework;

namespace AgentRalph.Tests
{
    [TestFixture]
    public class FindSpecificCatchClauses
    {
        [Test]
        public void SimpleCase()
        {
            FindBadCatchBlock("simple case", @"
    class BadCatch
    {
        public void Foo()
        {
            try
            {
            }
            catch (Exception e)
            {
                string temp = e.ToString();
            }
        }
    }
");
        }

        [Test]
        public void DeclaringLocalStringIsNotGOodEnough()
        {
            DoNotFindBadCatchBlock("Declaring a local string is not enough", @"
    class BadCatch
    {
        public void Foo()
        {
            try
            {
            }
            catch (Exception e)
            {
                string temp;
            }
        }
    }
");
        }

        [Test]
        public void ExceptionVarNameIsArbitrary()
        {
            FindBadCatchBlock("exception variable name is arbitrary", @"
    class BadCatch
    {
        public void Foo()
        {
            try
            {
            }
            catch (Exception ex)
            {
                string temp = ex.ToString();
            }
        }
    }
");
        }

        [Test] // In this case, normal resharper gives warnings about unused variables and the like
        public void IfExceptionIsRethrownItStillGetsDeteted()
        {
            FindBadCatchBlock("if exception is rethrown then it's allowed to pass", @"
    class BadCatch
    {
        public void Foo()
        {
            try
            {
            }
            catch (Exception ex)
            {
                string temp = ex.ToString();
                throw ex;
            }
        }
    }
");
        }

        [Test]
        public void ToStringMustBeInvokedOnExceptionObject()
        {
            DoNotFindBadCatchBlock("ToString must be invoked on the exception object in particular.", @"
    class BadCatch
    {
        public void Foo()
        {
            int i;
            try
            {
            }
            catch (Exception ex)
            {
                string temp = i.ToString();
            }
        }
    }
");
        }

        [Test]
        public void OkIfVarHoldingToStringResultGetsUsed()
        {
// If it uses the local var, that's ok
            DoNotFindBadCatchBlock("if it uses the var holding the ToString result, that's ok", @"
    class BadCatch
    {
        public void Foo()
        {
            try
            {
            }
            catch (Exception ex)
            {
                string temp = ex.ToString();
                Console.WriteLine(temp);
            }
        }
    }
");
        }

        private void DoNotFindBadCatchBlock(string msg, string program)
        {
            FindBadCatchBlock(program, 0, msg);
        }

        private void FindBadCatchBlock(string msg, string program)
        {
            FindBadCatchBlock(program, 1, msg);
        }

        private void FindBadCatchBlock(string program, int findCount, string msg)
        {
            using (TextReader text = new StringReader(program))
            {
                IParser parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, text);
                parser.Parse();

                Assert.AreEqual(0, parser.Errors.Count, "expected no errors in the input code");

                CompilationUnit cu = parser.CompilationUnit;
                BadCatchBlockFinder visitor = new BadCatchBlockFinder();
                cu.AcceptVisitor(visitor, null);

                Assert.AreEqual(findCount, visitor.Found, msg);
            }
        }
    }

    internal class BadCatchBlockFinder : AbstractAstVisitor
    {
        public int Found;

        private bool LinqVersion(CatchClause catchClause)
        {
            List<INode> children = catchClause.StatementBlock.Children;

            var z = from c in children.OfType<LocalVariableDeclaration>()
                    from l in c.Variables
                    where c.TypeReference.Type == "System.String" && l.Initializer is InvocationExpression
                    let initializer = (InvocationExpression) l.Initializer
                    where initializer.TargetObject is MemberReferenceExpression
                    let mre = (MemberReferenceExpression) initializer.TargetObject
                    where mre.MemberName == "ToString" && mre.TargetObject is IdentifierExpression
                    let ie = (IdentifierExpression) mre.TargetObject
                    where ie.Identifier == catchClause.VariableName
                    let next_child = children.IndexOf(c) + 1
                    where next_child >= children.Count || IsNotUsedInAnyRemainingStatements(ie, children.GetRange(next_child, children.Count - next_child))
                    select c;

            return z.Any();
        }

        public override object VisitCatchClause(CatchClause catchClause, object data)
        {
            bool found = NonLinqVersion(catchClause);
//          bool found = LinqVersion(catchClause);
            if (found)
                Found++;
            return base.VisitCatchClause(catchClause, data);
        }

        private bool NonLinqVersion(CatchClause catchClause)
        {
            List<INode> children = catchClause.StatementBlock.Children;
            if (children.Count <= 0)
            {
                return false;
            }

            LocalVariableDeclaration lvd = children[0] as LocalVariableDeclaration;
            if (lvd == null || lvd.TypeReference.Type != "System.String")
            {
                return false;
            }

            InvocationExpression initializer = lvd.Variables[0].Initializer as InvocationExpression;
            if (initializer == null)
            {
                return false;
            }

            MemberReferenceExpression mre = initializer.TargetObject as MemberReferenceExpression;
            if (mre == null)
            {
                return false;
            }

            if (mre.MemberName != "ToString")
            {
                return false;
            }

            IdentifierExpression ie = mre.TargetObject as IdentifierExpression;
            if (ie == null || ie.Identifier != catchClause.VariableName)
            {
                return false;
            }

            // ToString is being called on the exception

            return children.Count == 1 || IsNotUsedInAnyRemainingStatements(ie, children.GetRange(1, children.Count - 1));
        }

        private bool IsNotUsedInAnyRemainingStatements(IdentifierExpression ie, IEnumerable<INode> list)
        {
            return (from node in list
                    where StatementUsesExpr(node, ie)
                    select node).Any();
        }

        private bool StatementUsesExpr(INode node, IdentifierExpression ie)
        {
            FindReferenceVisitor visitor = new FindReferenceVisitor(true, ie.Identifier, node.StartLocation, node.EndLocation);
            node.AcceptVisitor(visitor, null);
            return visitor.Identifiers.Count > 0;
        }
    }

    public class FindReferenceVisitor : AbstractAstVisitor
    {
        private readonly List<IdentifierExpression> identifiers;
        private readonly string name;
        private readonly Location rangeStart;
        private readonly Location rangeEnd;
        private readonly StringComparer comparer;

        public List<IdentifierExpression> Identifiers
        {
            get { return identifiers; }
        }

        /// <summary>
        /// Basically just says "are there any variables with the name <see cref="name"/> inbetween the
        /// section of document defined by <see cref="rangeStart"/> and <see cref="rangeEnd"/>?"
        /// </summary>
        /// <param name="caseSensitive"></param>
        /// <param name="name"></param>
        /// <param name="rangeStart"></param>
        /// <param name="rangeEnd"></param>
        public FindReferenceVisitor(bool caseSensitive, string name, Location rangeStart, Location rangeEnd)
        {
            this.identifiers = new List<IdentifierExpression>();
            this.name = name;
            this.rangeEnd = rangeEnd;
            this.rangeStart = rangeStart;
            this.comparer = (caseSensitive) ? StringComparer.InvariantCulture : StringComparer.InvariantCultureIgnoreCase;
        }

        public override object VisitIdentifierExpression(IdentifierExpression identifierExpression, object data)
        {
            if (Compare(identifierExpression))
            {
                identifiers.Add(identifierExpression);
            }
            return base.VisitIdentifierExpression(identifierExpression, data);
        }

        private bool Compare(IdentifierExpression ie)
        {
            return ((this.comparer.Compare(ie.Identifier, this.name) == 0) &&
                    Inside(ie.StartLocation, ie.EndLocation));
        }

        private bool Inside(Location start, Location end)
        {
            return start >= this.rangeStart && end <= this.rangeEnd;
        }
    }
}