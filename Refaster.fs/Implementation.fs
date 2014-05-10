module Refaster
open System.Collections.Generic
open ICSharpCode.NRefactory.Ast

type Pattern(Expr, Params) =
  member this.Expr = Expr
  member this.Params = Params

type Match =
| Match of (string*INode) list

type PatternMatchVisitor(parms : (string*string) list) = 
  inherit AgentRalph.Visitors.AstComparisonVisitor() 
  let mutable stuff : (string*INode) list = []
  let isIdentical cap (node:INode) = AgentRalph.AstMatchHelper.Matches(cap,node)
  member public this.CaptureGroups = stuff |> List.rev // the rev allows tests to depend on order.
  override this.VisitIdentifierExpression(pat, obj) = let obj = obj :?> INode
                                                      match List.tryFind (fun (pname,_) -> pname = pat.Identifier) parms with
                                                      | Some((pname,_)) -> match stuff |> List.tryFind (fun (name,cap) -> name = pat.Identifier) with
                                                                           | Some(name,cap) -> isIdentical cap obj
                                                                           | None           -> stuff <- List.append [(pname, obj)] stuff 
                                                                                               true
                                                      | None -> base.VisitIdentifierExpression(pat,obj)

let toPattern (md:MethodDeclaration) : Pattern option =
  let expr = md.Body.Children.[0]
  let expr = (expr :?> ExpressionStatement).Expression
  let capgrps = md.Parameters |> Seq.toList|> List.map (fun p -> p.ParameterName, p.TypeReference.ToString()) // ToString() does a decent job of getting a full type name
  Some(Pattern(expr, capgrps))
  
let applyPattern (pat:Pattern) exp : Match option =
  let visitor = new PatternMatchVisitor(pat.Params)
  let success = pat.Expr.AcceptVisitor(visitor, exp)
  if success then
    let locations = visitor.CaptureGroups 
    Some(Match(locations))
  else None
