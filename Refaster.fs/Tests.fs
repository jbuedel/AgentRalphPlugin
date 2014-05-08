module Refaster.fs
open NUnit.Framework
open FsUnit
open ICSharpCode.NRefactory.Ast

type Pattern =
  member this.X = "F#"
 
type Match =
  | Match of (string*Expression) list

let toPattern (md:MethodDeclaration) : Pattern option =
  None

let getPattern pat = 
  match pat with 
  | Some(p) -> p 
  | None -> failwith "Must be a pattern"

let applyPattern (pat:Pattern) exp : Match option =
  None

[<TestFixture>]
type Class1() = 
  let isSome fopt md =
    match fopt md with
    | Some(p) -> Assert.Pass()
    | None    -> Assert.Fail()

  let isNone fopt md =
    match fopt md with
    | Some(p) -> Assert.Fail()
    | None    -> Assert.Pass()

  let toExpr code =
    AgentRalph.AstMatchHelper.ParseToE<Expression>(code) 
  let toMethod code =
    AgentRalph.AstMatchHelper.ParseToMethodDeclaration(code)

  let print (expr:Expression) = 
    ICSharpCode.NRefactory.INodeExt.Print(expr)
   
  [<Test>]
  member this.MethodDeclarationsBecomePatterns() =
    let md = toMethod "void foo(){Console.WriteLine(13);}"
    isSome toPattern md

  [<Test>]
  member this.``Patterns match against expressions``() =
    let doMatch patText exprText note =
      toMethod patText |> toPattern |> getPattern |> applyPattern <| toExpr exprText

    let testF patText exprText note = 
      let mtch = doMatch patText exprText note
      match mtch with
      | Some(m) -> Assert.Fail("Got a match")
      | None    -> Assert.Pass()

    let test patText exprText note = 
      let mtch = doMatch patText exprText note
      match mtch with
      | Some(m) -> m
      | None    -> failwith "Expected a match"



    let result = test "void pat(){Console.WriteLine(13);}" "Console.WriteLine(13)" "Simplest case"
    match result with
    | Match([]) -> () 
    | _ -> Assert.Fail("Expected a match")

    let result = test "void pat(int x){Console.WriteLine(x);}" "Console.WriteLine(13)" "parameters can match expressions"
    match result with
    | Match((name,exp) :: []) -> name |> should equal "x"
                                 exp |> print |> should equal "13" 
    | _ -> Assert.Fail("Expected a match")

    let result = test "void pat(int x, string y){Console.WriteLine(x,y);}" "Console.WriteLine(13, \"string\")" "multiple parameters match multiple expressions"
    match result with
    | Match((name1,expr1) :: (name2,expr2) :: []) -> name1 |> should equal "x"
                                                     expr1 |> print |> should equal "13" 
                                                     name2 |> should equal "y"
                                                     expr2 |> print |> should equal "\"string\"" 
    | _ -> Assert.Fail("Expected a match")

    testF "void pat(int x){Console.WriteLine(x);}" "Console.WriteLine(\"string\")" "parameter types must be compatible with the type of expression they match"

    let result = test "void pat(int x, string y){Console.WriteLine(x,y);}" "Console.WriteLine(13)" "parameters that do not match are not included in the result"
    match result with
    | Match((name,exp) :: []) -> name |> should equal "x"
                                 exp |> print |> should equal "13" 
    | _ -> Assert.Fail("Expected a match")

    let result = test "void pat(int x){Console.WriteLine(x, x);}" "Console.WriteLine(13, 13)" "a single parameter can match multiple expressions"
    match result with
    | Match((name,expr) :: []) -> name |> should equal "x"
                                  expr |> print |> should equal "13"
    | _ -> Assert.Fail("Expected a match")

    testF "void pat(int x){Console.WriteLine(x, x);}" "Console.WriteLine(13, 14)" "a single parameter can match multiple expressions, but the expressions must be identical"

