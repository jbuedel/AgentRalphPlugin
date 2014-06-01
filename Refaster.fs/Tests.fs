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

let toTypeDef code =
  ICSharpCode.NRefactory.Tests.Ast.ParseUtilCSharp.ParseGlobal<TypeDeclaration>(code)

let toExpr code =
  AgentRalph.AstMatchHelper.ParseToE<Expression>(code) 
let toMethod code =
  AgentRalph.AstMatchHelper.ParseToMethodDeclaration(code)
let print (expr:INode) = 
  ICSharpCode.NRefactory.INodeExt.Print(expr)

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

  let assertMatch node1 strnode2 =   
    let strnode1 = print node1
    Assert.That(strnode1, Is.EqualTo(strnode2))

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
    | Match(_, []) -> () 
    | _ -> Assert.Fail("Expected a match")

  [<Test>]
  member this.``parameters are capture groups and can match expressions``() =
    let result = test "void pat(int x){Console.WriteLine(x);}" "Console.WriteLine(13)" 
    match result with
    | Match(_,(name,cap) :: []) -> name |> should equal "x"
                                   cap |> print |> should equal "13" 
    | _ -> Assert.Fail("Expected a match")

  [<Test>]
  member this.``multiple capture groups match multiple expressions``() =
    let result = test "void pat(int x, string y){Console.WriteLine(x,y);}" "Console.WriteLine(13, \"string\")" 
    match result with // This test depends on the order, and should not.
    | Match(_, (name1,capt1) :: (name2,capt2) :: []) -> name1 |> should equal "x"
                                                        capt1 |> print |> should equal "13" 
                                                        name2 |> should equal "y"
                                                        capt2 |> print |> should equal "\"string\"" 
    | _ -> Assert.Fail("Expected a match with two captures")

  [<Test>][<Ignore("The parser does not supply type information.")>]
  member this.``capture group type must be compatible with the type of expression it matches``() =
    testF "void pat(int x){Console.WriteLine(x);}" "Console.WriteLine(\"string\")" 

  [<Test>][<Ignore("Not sure we need to bother to filter out the unused capture groups.")>]
  member this.``capture groups that do not match are not included in the result``() =
    let result = test "void pat(int x, string y){Console.WriteLine(x,y);}" "Console.WriteLine(13)" 
    match result with
    | Match(_,(name,cap) :: []) -> name |> should equal "x"
                                   cap |> print |> should equal "13" 
    | _ -> Assert.Fail("Expected a match")

  [<Test>]
  member this.``a single capture group can match multiple expressions``() =
    let result = test "void pat(int x){Console.WriteLine(x, x);}" "Console.WriteLine(13, 13)" 
    match result with
    | Match(_,(name,expr) :: []) -> name |> should equal "x"
                                    expr |> print |> should equal "13"
    | _ -> Assert.Fail("Expected a match")

  [<Test>]
  member this.``a single capture group can match multiple expressions, but the expressions must be identical``() =
    testF "void pat(int x){Console.WriteLine(x, x);}" "Console.WriteLine(13, 14)" 

  [<Test>]
  member this.``replacement expression is a call to the method that the pattern drew from``() =
    let result = test "void pat(){Console.WriteLine(13);}" "Console.WriteLine(13)" 
    let replacement = Refaster.toReplacement result
    Assert.That(replacement, Is.EqualTo "pat()")

  [<Test>]
  member this.``replacement expression is a call to the method that the pattern drew from (with parameters)``() =
    let result = test "void foo(int x){Console.WriteLine(x, x);}" "Console.WriteLine(13, 13)" 
    let replacement = Refaster.toReplacement result
    Assert.That(replacement, Is.EqualTo "foo(13)")

[<TestFixture>] 
type PatternNormalizationTests() =
  [<Test>]
  member this.``'this' references are added to all member references in a pattern``() =
    let patternClass = "class PatternClass { int IntMeth() {return 1;} void pat() { IntMeth(); }}" |> toTypeDef 
    // There will be two patterns as there is two methods, but we only care about the one.
    let pat = Refaster.toPatterns patternClass |> Seq.find (fun p -> p.Name = "pat") 
    // now assert that the pattern's expression matches this.IntMethod()
    Assert.That(print pat.Expr, Is.EqualTo("this.IntMethod()"))
    // TODO: Create a AddThisToAllMemberReferencesVisitor(), and use it here.  Any further unit tests concerning 
    // different kinds of members ought to be against that directly.
