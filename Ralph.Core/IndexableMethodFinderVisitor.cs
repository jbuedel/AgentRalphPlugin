using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;

namespace AgentRalph
{
    /// <summary>
    /// Finds all methods on a compilation unit, and specifically provides an accessor for a Foo and a Bar.
    /// </summary>
    public class IndexableMethodFinderVisitor : AbstractAstVisitor
    {
        public override object VisitMethodDeclaration(MethodDeclaration md, object data)
        {
            Methods.Add(md.Name, md);
            return base.VisitMethodDeclaration(md, data);
        }

        public Dictionary<string, MethodDeclaration> Methods { get; private set; }

        public IndexableMethodFinderVisitor()
        {
            Methods = new Dictionary<string, MethodDeclaration>();
        }

        public MethodDeclaration FooMethod
        {
            get { return new List<MethodDeclaration>(Methods.Values)[0]; }
        }

        public MethodDeclaration BarMethod
        {
            get { return new List<MethodDeclaration>(Methods.Values)[1]; }
        }

        public IEnumerable<MethodDeclaration> AllMethods
        {
            get { return Methods.Select(x=>x.Value); }
        }
    }

    public class IndexableClassFinderVisitor : AbstractAstVisitor
    {
        public Dictionary<string, TypeDeclaration> Classes { get; private set; }

        public IEnumerable<TypeDeclaration> AllClasses
        {
            get { return Classes.Select(x => x.Value); }
        }

        public IndexableClassFinderVisitor()
        {
            Classes = new Dictionary<string, TypeDeclaration>();
        }

        public override object VisitTypeDeclaration(TypeDeclaration typeDeclaration, object data)
        {
            if (typeDeclaration.Type == ClassType.Class || typeDeclaration.Type == ClassType.Struct)
                Classes.Add(typeDeclaration.Name, typeDeclaration);

            return base.VisitTypeDeclaration(typeDeclaration, data);
        }
    }
}