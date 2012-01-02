using System;
using System.Collections.Generic;
using System.Linq;
using AgentRalph.Visitors;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;
using SharpRefactoring;

namespace AgentRalph.CloneCandidateDetection
{
    public class LiteralToParameterExpansion : IRefactoringExpansion
    {
        public IEnumerable<CloneDesc> FindCandidatesViaRefactoringPermutations(TargetInfo left, CloneDesc refactoredPermy)
        {
            MethodDeclaration right = refactoredPermy.PermutatedMethod;

            // HEURISTIC:  This refactoring will not change the node count, so don't bother if they don't already match.
            if(right.Body.CountNodes() != left.Target.Body.CountNodes())
//            if(!right.MatchesIgnorePrimitives(left.Target))
                yield break;

            // HEURISTIC:  No point in adding any more parameters than the target method has.
            int parms_to_add = left.Target.Parameters.Count - right.Parameters.Count;
            if (parms_to_add > 0)
            {
                var md = FindCandidates(right, parms_to_add);
                foreach (var h in md)
                {
                    yield return new CloneDesc(h.md, refactoredPermy, new QuickFixInfo(refactoredPermy.ReplacementInvocationInfo, refactoredPermy.ReplacementInvocationInfo.LiteralParams.Concat(h.parameters).ToList()))
                                     {
                                         ReplacementInvocation = AddParms(refactoredPermy.ReplacementInvocation, h.parameters)
                                     };
                }
            }
        }

        private AbstractNode AddParms(AbstractNode invocation, IList<object> parameters)
        {
            if (invocation == null)
            {
                throw new ArgumentNullException("invocation");
            }

            var m = invocation as Statement;
            if (m != null)
                invocation = m.DeepCopy();
            else
            {

                var q = invocation as Expression;
                if (q != null)
                {
                    invocation = q.DeepCopy();
                }
                else
                {
                    throw new Exception("Found a " + invocation.GetType());
                }
            }
            
            InvocationExpression ie = FirstInvocationExpression(invocation);

            foreach (var o in parameters)
            {
                ie.AddArg(o);
            }

            return invocation;
        }

        private InvocationExpression FirstInvocationExpression(AbstractNode invocation)
        {
            var v = new FindInvocationExpressionVisitor();
            invocation.AcceptVisitor(v, null);
            return v.Expr;
        }

        private class FindInvocationExpressionVisitor : AbstractAstVisitor
        {
            public override object VisitInvocationExpression(InvocationExpression invocationExpression, object d)
            {
                Expr = invocationExpression;
                return null;
            }

            public InvocationExpression Expr { get; private set; }
        }

        /// <summary>
        /// Shows that the other two parameters are not necessary to this object.  Exposed for testing.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public IEnumerable<CloneDesc> FindCandidatesViaRefactoringPermutations(MethodDeclaration right)
        {
            var md = FindCandidates(right, -1);
            foreach (Holder h in md)
            {
                yield return new CloneDesc(h.md, Window.Null, new List<INode>());
            }
        }

        private IEnumerable<Holder> FindCandidates(MethodDeclaration right, int count)
        {
            var rights_primitives = right.AllPrimitiveExpressions();

            // Find all PrimitiveExpressions except the keyword 'null'
            IEnumerable<PrimitiveExpression> primitives = rights_primitives.Where(x => x.ValueType != null);

            // find the position of each primitive
            var prims_with_position = from p in primitives
                                      select new {p.Value, p.ValueType, Index = rights_primitives.IndexOf(p)};

            // Group them together by literal value.  The groups then are candidates for replacement by a single param.
            var prim_groups = from p in prims_with_position
                              group p by new {p.Value, p.ValueType}
                              into g select new PrimExpSet {Value = g.Key.Value, ValueType = g.Key.ValueType, Positions = g.Select(x => x.Index)};

            // The powerset of the primitive positions is all the possible ways a single primitive value can be converted to a param.
            var all_param_possibles = from pes in prim_groups
                                      let expanded = from positions in Sets<int>.PowerSet(pes.Positions.ToArray())
                                                     select new PrimExpSet {Value = pes.Value, ValueType = pes.ValueType, Positions = positions}
                                      select expanded;

            foreach (var e1 in Sets<PrimExpSet>.CartesianProduct(all_param_possibles, count))
            {
                // If count is neg, let anything through.
                if (count < 0 || e1.Count() == count)
                {
                    var md = MakePermutation(right, e1);
                    const string QUOTE = "\"";
                    var parameters = e1.Select(x => x.ValueType == typeof (string) ? QUOTE + x.Value + QUOTE : x.Value).ToList();
                    yield return new Holder {md = md, parameters = parameters};
                }
            }
        }

        private class Holder
        {
            public MethodDeclaration md;
            public IList<object> parameters;
        }

        private MethodDeclaration MakePermutation(MethodDeclaration right, IEnumerable<PrimExpSet> prim_groups)
        {
            ResetNameCount();

            right = right.DeepCopy();
            var rights_primitives = right.AllPrimitiveExpressions();

            foreach (var prim_grp in prim_groups)
            {
                var param_name = NextName();

                var typeRef = new TypeReference(prim_grp.ValueType.FullName);
                right.Parameters.Add(new ParameterDeclarationExpression(typeRef, param_name));

                var replacer = new PrimitiveReplacer();
                foreach (var pos in prim_grp.Positions)
                {
                    replacer.AddReplacement(rights_primitives[pos], new IdentifierExpression(param_name));
                }
                right.AcceptVisitor(replacer, null);
            }

            return right;
        }

        private void ResetNameCount()
        {
            nameCounter = 0;
        }

        private int nameCounter;

        private string NextName()
        {
            var name = "i" + (nameCounter != 0 ? nameCounter.ToString() : "");
            nameCounter++;
            return name;
        }

        private class PrimitiveReplacer : AbstractAstTransformer
        {
            private readonly IList<ReplacePair> Replacements = new List<ReplacePair>();

            public override object VisitPrimitiveExpression(PrimitiveExpression primitiveExpression, object data)
            {
                foreach (var r in Replacements)
                {
                    if (ReferenceEquals(primitiveExpression, r.Target))
                    {
                        ReplaceCurrentNode(r.Replacement);
                    }
                }
                return base.VisitPrimitiveExpression(primitiveExpression, data);
            }

            public void AddReplacement(INode target, INode replacement)
            {
                Replacements.Add(new ReplacePair(target, replacement));
            }
        }

        private class ReplacePair
        {
            public readonly INode Target;
            public readonly INode Replacement;

            public ReplacePair(INode target, INode replacement)
            {
                Target = target;
                Replacement = replacement;
            }
        }

        private class PrimExpSet
        {
            public object Value;
            public IEnumerable<int> Positions;

            public Type ValueType { get; set; }
        }
    }
}