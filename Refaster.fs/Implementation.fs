module Refaster
open System.Collections.Generic
open ICSharpCode.NRefactory.Ast

type Pattern(Expr, Params) =
  member this.Expr = Expr
  member this.Params = Params

type Match =
| Match of (string*INode) list

type PatternMatchVisitor(parms : string list) = 
  inherit AgentRalph.Visitors.AstComparisonVisitor() 
  let mutable stuff : (string*INode) list = []
  let isIdentical cap (node:INode) = AgentRalph.AstMatchHelper.Matches(cap,node)
  member public this.CaptureGroups = stuff |> List.rev // the rev allows tests to depend on order.
  override this.VisitIdentifierExpression(pat, obj) = let obj = obj :?> INode
                                                      match List.tryFind (fun p -> p = pat.Identifier) parms with
                                                      | Some(p) -> match stuff |> List.tryFind (fun (name,cap) -> name = pat.Identifier) with
                                                                   | Some(name,cap) -> isIdentical cap obj
                                                                   | None           -> stuff <- List.append [(p, obj)] stuff 
                                                                                       true
                                                      | None -> base.VisitIdentifierExpression(pat,obj)

let toPattern (md:MethodDeclaration) : Pattern option =
  let expr = md.Body.Children.[0]
  let expr = (expr :?> ExpressionStatement).Expression
  let p's = md.Parameters |> Seq.toList|> List.map (fun p -> p.ParameterName)
  Some(Pattern(expr, p's))
  
let applyPattern (pat:Pattern) exp : Match option =
  let visitor = new PatternMatchVisitor(pat.Params)
  let success = pat.Expr.AcceptVisitor(visitor, exp)
  if success then
    let locations = visitor.CaptureGroups 
    Some(Match(locations))
  else None
