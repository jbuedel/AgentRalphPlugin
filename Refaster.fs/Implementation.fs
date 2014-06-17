module Refaster
open System.Collections.Generic
open ICSharpCode.NRefactory.Ast
open AgentRalph

let print (expr:INode) = 
  ICSharpCode.NRefactory.INodeExt.Print(expr)


type Pattern(Name, Expr, CaptureGroups) =
  /// The name of the pattern. Always the pattern function name.
  member this.Name = Name
  /// The ast of the expression to match against. Derived from the body of the pattern function.
  member this.Expr = Expr
  /// A name and type define a CaptureGroup. Derived from the parameters of the pattern function.
  member this.CaptureGroups = CaptureGroups
  override this.ToString() = sprintf "Name: %s \nExpr : %s" Name (print Expr)

type Coord(p1, p2) =
  member this.Start = p1
  member this.End = p2

type Match =
| Match of string * (string*INode) list

(*type Match(name, grps, coord) =
  member this.Name = name
  member this.CaptureGroups = grps
  member this.RepairCoords = coord*)

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
  let expr = match expr with 
             | :? ExpressionStatement as expr -> expr.Expression
             | :? ReturnStatement as stmt -> stmt.Expression
             | _                          -> failwithf "Patterns created from type %A are not supported." (expr.GetType())

  let capgrps = md.Parameters |> Seq.toList|> List.map (fun p -> p.ParameterName, p.TypeReference.ToString()) // ToString() does a decent job of getting a full type name
  let name = md.Name
  Some(Pattern(name, expr, capgrps))

let toPatterns (typeDef:TypeDeclaration) =
  let patterns = typeDef.FindAllMethods() |> Seq.map toPattern 
  seq { for p in patterns do match p with | Some(q) -> yield q | None -> () }
  
/// Applies the pattern against this single node. 
let applyPattern (pat:Pattern) exp : Match option =
  let visitor = new PatternMatchVisitor(pat.CaptureGroups)
  let success = pat.Expr.AcceptVisitor(visitor, exp)
  if visitor.Match then
    Some(Match(pat.Name, visitor.CaptureGroups))
  else None

let rec allSubNodes (node:INode) =
    seq { 
      yield node    
      for node in node.Chilluns do 
        for n in allSubNodes node do yield n
    }

/// Applies the pattern against this node and all subnodes.
let applyPatternG (pat:Pattern) (clazz:TypeDeclaration) : Match option seq =
  allSubNodes clazz |> Seq.map (applyPattern pat) 

let toReplacement mtch =
  // convert Match to a function call.  Like foo()
  match mtch with
  | Match(name, captureGroups) -> name + "(" + (captureGroups |> List.map (fun (_,y) -> print y) |> String.concat ",") + ")"
  | _        -> "" // not really sure what this should do...


  (* Next up I need tests that show Match objects return repair coordinates. 
  Then I may have everything necessary to start running against CloneCandidateDetectionTests.

  *)

let getCoordinates (node:INode) =
  Coord(node.StartLocation, node.EndLocation)