module Refaster
open ICSharpCode.NRefactory.Ast

type Pattern(expr) =
  member this.Expr = expr
  member this.Params = []

type Match =
| Match of (string*Expression) list

type PatternMatchVisitor(parms : string list) = 
  inherit AgentRalph.Visitors.AstComparisonVisitor() 
  override this.VisitIdentifierExpression(pat, obj) = printf "%A" pat
                                                      true

let toPattern (md:MethodDeclaration) : Pattern option =
  let expr = md.Body.Children.[0]
  Some(Pattern(expr))
  
let applyPattern (pat:Pattern) exp : Match option =
  let visitor = new PatternMatchVisitor(pat.Params)
  let success = pat.Expr.AcceptVisitor(visitor, exp)
  if success then
    None
  else None
