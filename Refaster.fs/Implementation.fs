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
  member public this.Locations = stuff
  override this.VisitIdentifierExpression(pat, obj) = printfn "%A" pat
                                                      match List.tryFind (fun p -> p = pat.Identifier) parms with
                                                      | Some(p) -> stuff <- List.append [(p, obj:?>INode)] stuff
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
    let locations = visitor.Locations 
    Some(Match(locations))
  else None
