module Refaster
open System.Collections.Generic
open ICSharpCode.NRefactory.Ast

type Pattern(Name, Expr, CaptureGroups) =
  /// The name of the pattern. Always the pattern function name.
  member this.Name = Name
  /// The ast of the expression to match against. Derived from the body of the pattern function.
  member this.Expr = Expr
  /// A name and type define a CaptureGroup. Derived from the parameters of the pattern function.
  member this.CaptureGroups = CaptureGroups

type Match =
| Match of string * (string*INode) list

type PatternMatchVisitor(parms : (string*string) list) = 
  inherit AgentRalph.Visitors.AstComparisonVisitor() 
  let mutable stuff : (string*INode) list = []
  let isIdentical cap (node:INode) = AgentRalph.AstMatchHelper.Matches(cap,node)
  member public this.CaptureGroups = stuff |> List.rev // the rev allows tests to depend on order.
  override this.VisitIdentifierExpression(pat, obj) = let obj = obj :?> INode
                                                      match parms |> List.tryFind (fun (pname,_) -> pname = pat.Identifier) with
                                                      | Some((pname,_)) -> match stuff |> List.tryFind (fun (name,_) -> name = pat.Identifier) with
                                                                           | Some(name,cap) -> isIdentical cap obj
                                                                           | None           -> stuff <- List.append [(pname, obj)] stuff 
                                                                                               true
                                                      | None -> base.VisitIdentifierExpression(pat,obj)

let toPattern (md:MethodDeclaration) : Pattern option =
  let expr = md.Body.Children.[0]
  let expr = (expr :?> ExpressionStatement).Expression
  let capgrps = md.Parameters |> Seq.toList|> List.map (fun p -> p.ParameterName, p.TypeReference.ToString()) // ToString() does a decent job of getting a full type name
  let name = md.Name
  Some(Pattern(name, expr, capgrps))
  
let applyPattern (pat:Pattern) exp : Match option =
  let visitor = new PatternMatchVisitor(pat.CaptureGroups)
  let success = pat.Expr.AcceptVisitor(visitor, exp)
  if success then
    Some(Match(pat.Name, visitor.CaptureGroups))
  else None

let toReplacement mtch =
  let print (expr:INode) = 
    ICSharpCode.NRefactory.INodeExt.Print(expr)

  // convert Match to a function call.  Like foo()
  match mtch with
  | Match(name, captureGroups) -> name + "(" + (captureGroups |> List.map (fun (_,y) -> print y) |> String.concat ",") + ")"
  | _        -> "" // not really sure what this should do...
