using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Parser.CSharp;
using AgentRalph.Visitors;
using ICSharpCode.NRefactory.Visitors;
using SharpRefactoring;

namespace AgentRalph
{
    public static class AstMatchHelper
    {
        public static bool MatchesPrint(this MethodDeclaration md_this, MethodDeclaration md)
        {
            Console.WriteLine(new string('-', 25));
            Console.WriteLine("Comparing:");
            Console.WriteLine(md.Print());
            Console.WriteLine("To:");
            Console.WriteLine(md_this.Print());

            bool match = md_this.Matches(md);

            if (match)
            {
                Console.WriteLine("Match!");
            }
            return match;
        }
        
        public static bool MatchesPrint(this INode md_this, INode md)
        {
            Console.WriteLine(new string('-', 25));
            Console.WriteLine("Comparing:");
            Console.WriteLine(md.Print());
            Console.WriteLine("To:");
            Console.WriteLine(md_this.Print());

            bool match = md_this.Matches(md);

            if (match)
            {
                Console.WriteLine("Match!");
            }
            return match;
        }
        
        public static bool Matches(this MethodDeclaration md_this, MethodDeclaration md)
        {
            AstComparisonVisitor cv = new AstComparisonVisitor();
            md.AcceptVisitor(cv, md_this);
            return cv.Match;
        }

        public static bool Matches(this INode left, INode right)
        {
            AstComparisonVisitor cv = new AstComparisonVisitor();
            left.AcceptVisitor(cv, right);
            return cv.Match;
        }

        public static bool Matches2(this INode left, INode right) {

          var l_flat = left.Flatten().ToArray();
          var r_flat = right.Flatten().ToArray();

          if (l_flat.Count() != r_flat.Count())
            return false;

          for (int i = 0; i < l_flat.Count(); i++) {
            if(!l_flat[i].IsShallowMatch(r_flat[i])) {
              Console.WriteLine("Failing type was " + l_flat[i].GetType());
              return false;
            }
          }

          return true;
        }

      private static IEnumerable<INode> Flatten(this INode node)
        {
          IList<INode> all = new List<INode>();
          Action<INode> flatten = null; 
          flatten = n => {
              all.Add(n);
              foreach (var c in n.Chilluns) flatten(c);
          };
          flatten(node);
          return all;
        }
        public static Dictionary<string, List<LocalLookupVariable>> GetLookupTable(this MethodDeclaration md)
        {
            LookupTableVisitor v = new LookupTableVisitor(SupportedLanguage.CSharp);
            md.AcceptVisitor(v, null);
            return v.Variables;
        }

        public static Dictionary<string, List<LocalLookupVariable>> GetLookupTableWithParams(this MethodDeclaration md)
        {
            LookupTableVisitor v = new LookupTableVisitor(SupportedLanguage.CSharp, true);
            md.AcceptVisitor(v, null);
            return v.Variables;
        }

        public static IEnumerable<Window> OscillateWindows(int numChildren)
        {
            return BackwardsOscillateWindows(numChildren);
        }
        private static IEnumerable<Window> BackwardsOscillateWindows(int numChildren)
        {
            for (int top = 0; top < numChildren; top++)
            {
                for (int bot = numChildren - 1; bot >= top; bot--)
                {
                    yield return new Window(top, bot);
                }
            }
        }

        public static MethodDeclaration ExtractMethod(MethodDeclaration md, Window window, List<INode> children)
        {
            try
            {
                CSharpMethodExtractor extractor = new CSharpMethodExtractor();
                extractor.Extract(md, window, children);
                return extractor.ExtractedMethod;
            }
            catch (Exception ex)
            {
                throw new Exception("Window: " + window + " Method: " + md.Print(), ex);
            }
        }

        public static MethodDeclaration ExtractMethod(MethodDeclaration md, Window window)
        {
            CSharpMethodExtractor extractor = new CSharpMethodExtractor();
            extractor.Extract(md, window);
            return extractor.ExtractedMethod;
        }

        public static MethodDeclaration FindMethod(this INode expected_ast, string methodName)
        {
            IndexableMethodFinderVisitor v1 = new IndexableMethodFinderVisitor();
            expected_ast.AcceptVisitor(v1, null);
            if (v1.Methods.ContainsKey(methodName))
                return v1.Methods[methodName];

            throw new ApplicationException("Method '" + methodName + "' was not found.");
        }

        public static IEnumerable<MethodDeclaration> FindAllMethods(this TypeDeclaration unit)
        {
            var famv = new AllMethodsFinderVisitor();
            unit.AcceptVisitor(famv, null);
            return famv.AllMethods;
        }

        public static TypeDeclaration FindClass(this INode expected_ast, string methodName)
        {
            IndexableClassFinderVisitor v1 = new IndexableClassFinderVisitor();
            expected_ast.AcceptVisitor(v1, null);
            return v1.Classes[methodName];
        }

        public static IEnumerable<TypeDeclaration> FindAllClasses(this CompilationUnit inode)
        {
            IndexableClassFinderVisitor v = new IndexableClassFinderVisitor();
            inode.AcceptVisitor(v, null);
            return v.AllClasses;
        }

        /// <summary>
        /// Prepends a @ to a parameter name in case the name chosen is also a keyword.
        /// </summary>
        /// <param name="pde"></param>
        /// <returns></returns>
        public static string ParameterNameSafe(this ParameterDeclarationExpression pde)
        {
            var p_name = pde.ParameterName;
            if (Keywords.IsNonIdentifierKeyword(p_name))
                return "@" + p_name;
            return p_name;
        }

        public static CompilationUnit ParseToCompilationUnit(string codeText)
        {
            IParser parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, new StringReader(codeText));
            parser.Parse();

            if (parser.Errors.Count != 0)
                throw new ApplicationException(string.Format("Expected no errors in the input code. Code was: {0} ErrorOutput: {1}", codeText, parser.Errors.ErrorOutput));

            return parser.CompilationUnit;
        }
        public static T ParseTo<T>(string codeText) where T : Statement
        {
            IParser parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, new StringReader(codeText));
            parser.Parse();

            if (parser.Errors.Count != 0)
                throw new ApplicationException(string.Format("Expected no errors in the input code. Code was: {0} ErrorOutput: {1}", codeText, parser.Errors.ErrorOutput));

            return (T)parser.CompilationUnit.CurrentBock;
        }

        public static IList<PrimitiveExpression> AllPrimitiveExpressions(this MethodDeclaration md)
        {
            PrimitiveExpressionFinderVisitor pefv = new PrimitiveExpressionFinderVisitor();
            md.AcceptVisitor(pefv, null);
            return pefv.ExpressionsFound;
        }

        public static MethodDeclaration DeepCopy(this MethodDeclaration right)
        {
            var codeText = right.Print();
            return ParseToMethodDeclaration(codeText);
        }
        
        public static Statement DeepCopy(this Statement right)
        {
            MethodDeclaration md = ParseToMethodDeclaration("void A() { " + right.Print() + " }");
            if (md.Body.Children.Count == 0)
                throw new ApplicationException("There were no statements.");
            
            Type type = typeof(Statement);
            if(!type.IsAssignableFrom(md.Body.Children[0].GetType()))
                throw new ApplicationException(String.Format("Parsed expression was {0} instead of {1} ({2}).", md.GetType(), type, md));

            return (Statement)md.Body.Children[0];
        }

        public static Expression DeepCopy(this Expression right)
        {
            // SEMICOLON HACK : without a trailing semicolon, parsing expressions does not work correctly
            IParser parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, new StringReader(right.Print() + ";"));
            object parsedExpression = parser.ParseExpression();
            
            if(parser.Errors.Count > 0)
                throw new ApplicationException("Unable to parse the expression.  Parse errors:" + parser.Errors.ErrorOutput);

            Type type = typeof(Expression);
            if (type.IsAssignableFrom(parsedExpression.GetType()))
                throw new ApplicationException(String.Format("Parsed expression was {0} instead of {1} ({2}).", parsedExpression.GetType(), type, parsedExpression));

            return (Expression)parsedExpression;
        }

        public static MethodDeclaration ParseToMethodDeclaration(string codeText)
        {
            IParser parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, new StringReader("class MyClass {" + codeText + "}"));
            parser.ParseMethodBodies = true;
            parser.Parse();
            
            TypeDeclaration td = (TypeDeclaration)parser.CompilationUnit.Children[0];

            return (MethodDeclaration)td.Children[0];
        }

        public static int CountNodes(this INode ast)
        {
            var visitor = new NodeCountingVisitor();
            ast.AcceptVisitor(visitor, null);
            return visitor.NodeCount;
        }

        public static bool MatchesIgnorePrimitives<T>(this T expected, T target) where T:INode
        {
            var visitor = new AstComparisonIgnoreLiteralsVisitor();
            expected.AcceptVisitor(visitor, target);
            return visitor.Match;
        }
    }

    public class PrimitiveExpressionFinderVisitor : AbstractAstVisitor
    {
        public PrimitiveExpressionFinderVisitor()
        {
            ExpressionsFound = new List<PrimitiveExpression>();
        }

        public override object VisitPrimitiveExpression(PrimitiveExpression primitiveExpression, object data)
        {
            ExpressionsFound.Add(primitiveExpression);
            return null;
        }

        public IList<PrimitiveExpression> ExpressionsFound { get; private set; }
    }

    public class AllMethodsFinderVisitor : AbstractAstVisitor
    {
        private readonly IList<MethodDeclaration> methods = new List<MethodDeclaration>();

        public IEnumerable<MethodDeclaration> AllMethods
        {
            get { return methods; }
        }

        public override object VisitMethodDeclaration(MethodDeclaration methodDeclaration, object d)
        {
            methods.Add(methodDeclaration);
            base.VisitMethodDeclaration(methodDeclaration, d);
            return null;
        }
    }
}

