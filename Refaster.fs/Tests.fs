module Tests
open NUnit.Framework
open FsUnit
open ICSharpCode.NRefactory.Ast
open Refaster

let toPattern (md:MethodDeclaration) : Pattern option =
  Refaster.toPattern md

let getPattern pat = 
  match pat with 
  | Some(p) -> p 
  | None -> failwith "Must be a pattern"

let applyPattern (pat:Pattern) exp : Match option =
  Refaster.applyPattern pat exp

[<TestFixture>]
type RefasterTests() = 
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

  let print (expr:INode) = 
    ICSharpCode.NRefactory.INodeExt.Print(expr)

  let doMatch patText exprText =
    printfn "Pattern text %A" patText
    printfn "Target  text %A" exprText
    
    let pat = toMethod patText |> toPattern |> getPattern 
    let expr = toExpr exprText

    printfn "Pattern expr: %A" (print pat.Expr)
    printfn "Target  expr: %A " (print expr)

    printfn "Pattern AST: %A" pat.Expr
    printfn "Expr    AST: %A" expr
    
    pat |> applyPattern <| expr

  let testF patText exprText = 
    let mtch = doMatch patText exprText 
    match mtch with
    | Some(m) -> Assert.Fail("Got a match")
    | None    -> Assert.Pass()

  let test patText exprText = 
    let mtch = doMatch patText exprText 
    match mtch with
    | Some(m) -> m
    | None    -> failwith "Expected a match"
  
  [<Test>]
  member this.``MethodDeclarations become patterns``() =
    let md = toMethod "void foo(){Console.WriteLine(13);}"
    isSome toPattern md

  [<Test>]
  member this.``simplest case``() =
    let result = test "void pat(){Console.WriteLine(13);}" "Console.WriteLine(13)" 
    match result with
    | Match([]) -> () 
    | _ -> Assert.Fail("Expected a match")

  [<Test>]
  member this.``parameters can match expressions``() =
    let result = test "void pat(int x){Console.WriteLine(x);}" "Console.WriteLine(13)" 
    match result with
    | Match((name,exp) :: []) -> name |> should equal "x"
                                 exp |> print |> should equal "13" 
    | _ -> Assert.Fail("Expected a match")

  [<Test>]
  member this.``multiple parameters match multiple expressions``() =
    let result = test "void pat(int x, string y){Console.WriteLine(x,y);}" "Console.WriteLine(13, \"string\")" 
    match result with
    | Match((name1,expr1) :: (name2,expr2) :: []) -> name1 |> should equal "x"
                                                     expr1 |> print |> should equal "13" 
                                                     name2 |> should equal "y"
                                                     expr2 |> print |> should equal "\"string\"" 
    | _ -> Assert.Fail("Expected a match")

  [<Test>]
  member this.``parameter types must be compatible with the type of expression they match``() =
    testF "void pat(int x){Console.WriteLine(x);}" "Console.WriteLine(\"string\")" 

  [<Test>]
  member this.``parameters that do not match are not included in the result``() =
    let result = test "void pat(int x, string y){Console.WriteLine(x,y);}" "Console.WriteLine(13)" 
    match result with
    | Match((name,exp) :: []) -> name |> should equal "x"
                                 exp |> print |> should equal "13" 
    | _ -> Assert.Fail("Expected a match")

  [<Test>]
  member this.``a single parameter can match multiple expressions``() =
    let result = test "void pat(int x){Console.WriteLine(x, x);}" "Console.WriteLine(13, 13)" 
    match result with
    | Match((name,expr) :: []) -> name |> should equal "x"
                                  expr |> print |> should equal "13"
    | _ -> Assert.Fail("Expected a match")

  [<Test>]
  member this.``a single parameter can match multiple expressions, but the expressions must be identical``() =
    testF "void pat(int x){Console.WriteLine(x, x);}" "Console.WriteLine(13, 14)" 

