module Refaster.fs
open NUnit.Framework
open FsUnit
open ICSharpCode.NRefactory.Ast

type Pattern =
  member this.X = "F#"
 
type Match =
  member this.X = "F#"

let toPattern (md:MethodDeclaration) : Pattern option =
  None
let getPattern pat = 
  match pat with 
  |Some(p) -> p 
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
      | Some(m) -> Assert.Pass("Got a match")
      | None    -> Assert.Fail()



    test "void pat(){Console.WriteLine(13);}" "Console.WriteLine(13)" "Simplest case"
    test "void pat(int x){Console.WriteLine(x);}" "Console.WriteLine(13)" "parameters can match expressions"
    test "void pat(int x, string y){Console.WriteLine(x,y);}" "Console.WriteLine(13, \"string\")" "multiple parameters match multiple expressions"
    testF "void pat(int x){Console.WriteLine(x);}" "Console.WriteLine(\"string\")" "parameter types must be compatible with the type of expression they match"
    test "void pat(int x, string y){Console.WriteLine(x,y);}" "Console.WriteLine(13)" "not all parameters need match"

    test "void pat(int x){Console.WriteLine(x, x);}" "Console.WriteLine(13, 13)" "parameters can match multiple expressions, but all matching expressions must be identical"
    testF "void pat(int x){Console.WriteLine(x, x);}" "Console.WriteLine(13, 14)" "parameters can match multiple expressions, but all matching expressions must be identical"


    // Now I need tests against matches. Assert I have all the parts to make a replacement. 