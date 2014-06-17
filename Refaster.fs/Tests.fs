module RefasterTests
open NUnit.Framework
open FsUnit
open ICSharpCode.NRefactory
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

let applyPatternG (pat:Pattern) exp : Match seq =
  Refaster.applyPatternG pat exp |> Seq.choose (fun m -> match m with | Some(m) -> Some(m) | None -> None)

let toExpr code =
  AgentRalph.AstMatchHelper.ParseToE<Expression>(code) 
let toMethod code =
  AgentRalph.AstMatchHelper.ParseToMethodDeclaration(code)
let toTypeDef code =
  ICSharpCode.NRefactory.Tests.Ast.ParseUtilCSharp.ParseGlobal<TypeDeclaration>(code)
let toCompilationUnit code =
  AgentRalph.AstMatchHelper.ParseToCompilationUnit(code)


/// Visits all nodes in the INode tree, and returns all MethodDeclarations found.
let getMethods (node:INode) =
  let visitor = new AgentRalph.AllMethodsFinderVisitor()
  node.AcceptVisitor(visitor, null) |> ignore
  visitor.AllMethods 

/// Returns the first method with a given name, or throws an error.
let getMethod name (node:INode) =
  getMethods node |> Seq.find (fun m -> m.Name = name)

let getClasses cu =
  AgentRalph.AstMatchHelper.FindAllClasses(cu)


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

  let doMatch patText targetText =
    let pat = toMethod patText |> toPattern |> getPattern 
    
    printfn "Pattern text %A" patText
    printfn "\texpr : %A" (print pat.Expr)
    for (nm,tipe) in pat.CaptureGroups do printfn "\t '%s' : %s" nm tipe
   
    let expr = toExpr targetText
    printfn "Target expr: %s" (print expr)

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
    | Some(Match(name, captures)) -> for (cname,cnode) in captures do printfn "'%s' => %s" cname (print cnode)
                                     Match(name,captures)
    | None    -> failwith "Expected a match"
    
  // Expects there to be exactly one match
  let doApplyPatternToClass pat classText = 
    let clazz = toTypeDef classText
    let result = applyPatternG pat clazz
    if Seq.length result > 0 then result 
    else printfn "pattern %A \nnot found in target class \n%A" pat (print clazz)
         failwith "Fail" 
  
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
    printfn "Replacement: %s" replacement
    Assert.That(replacement, Is.EqualTo "pat()")

  [<Test>]
  member this.``replacement expression is a call to the method that the pattern drew from (with parameters)``() =
    let result = test "void foo(int x){Console.WriteLine(x, x);}" "Console.WriteLine(13, 13)" 
    let replacement = Refaster.toReplacement result
    printfn "Replacement: %s" replacement
    Assert.That(replacement, Is.EqualTo "foo(13)")
    
  [<Test>]
  member this.``basic find a pattern in a class``() =
    let classCode = "class foo { public void bar() {Console.WriteLine(13);}}"
    let pattern = Pattern("pat", toExpr "Console.WriteLine(x)", [("x","string")])
    let result = doApplyPatternToClass pattern classCode 
    if Seq.length result <> 1 then Assert.Fail "Expected exactly one match."

  [<Test>]
  member this.``basic find a pattern twice in a class``() =
    let classCode = "class foo { public void bar() {Console.WriteLine(13);} private void foo() {Console.WriteLine(19);}}"
    let pattern = Pattern("pat", toExpr "Console.WriteLine(x)", [("x","string")])
    let result = doApplyPatternToClass pattern classCode  
    if Seq.length result <> 2 then Assert.Fail "Expected exactly two matches"

  [<Test>]
  member this.``basic find a pattern in a class that is not in the topmost block of a method``() =
    let classCode = "class foo { public void bar() { if(true) { Console.WriteLine(13); } }}"
    let pattern = Pattern("pat", toExpr "Console.WriteLine(x)", [("x","string")])
    let result = doApplyPatternToClass pattern classCode 
    if Seq.length result <> 1 then Assert.Fail "Expected exactly one match."

  [<Test>]
  member this.``INodes can be converted to coordinates``() =
    let assertCoords exptdStart exptdEnd node =
      let coords = Refaster.getCoordinates node 
      Assert.That(coords.Start, Is.EqualTo(exptdStart), sprintf "Start location of \n%A was wrong" (print node))
      Assert.That(coords.End, Is.EqualTo(exptdEnd), sprintf "End location of \n%A was wrong" (print node))
      
    let clazz = 
       "class foo { 
           public void bar() { 
	      if(true) { 
	         Console.WriteLine(13); 
	      } 
	   }
	}" |> toTypeDef

    assertCoords (Location(1,1)) (Location(2,7)) <| clazz
    assertCoords (Location(12,2)) (Location(29,2)) <| getMethod "bar" clazz
    assertCoords (Location(8,3)) (Location(9,5)) <| (Refaster.allSubNodes clazz |> Seq.find (fun n -> n.GetType().Name = "IfElseStatement"))

  [<Test>]
  member this.``allSubNodes follows expected paths (aka test INode.Chilluns)``() =
    let classCode = "class foo { public void bar() { if(true) { Console.WriteLine(13); } }}"
    let x = toTypeDef classCode |> Refaster.allSubNodes 
    x |> Seq.iter (fun n -> printfn "%s: %A" (n.GetType().Name) (print n))
    x |> Seq.map print |> should contain "Console.WriteLine(13)"

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

[<TestFixture>]
type CloneCandidateDetectionTests() =
  static member LoadInputFiles() =
    System.IO.Directory.GetFiles(@"..\..\Ralph.Core.Tests\CloneCandidateDetectionTests\TestCases", "*.cs") 
      |> Seq.map (fun file -> let tcd = new TestCaseData(file)
                              tcd.SetName(System.IO.Path.GetFileName(file))
      )
  
  [<Test>][<TestCaseSource("LoadInputFiles")>]
  member this.``Do a clone candidate test``(testCodeFile) =
    let codelines = System.IO.File.ReadAllLines(testCodeFile)

    if (Seq.head codelines).Contains("Ignore") then Assert.Ignore((Seq.head codelines).Trim('/').Trim().Substring(6))

    let ast = toCompilationUnit (String.concat "\r\n" codelines)

    let firstClass = getClasses ast |> Seq.head
    let methods = firstClass |> getMethods
    let pattern = match methods |> Seq.find (fun m -> m.Name = "pattern") |> toPattern with
                  | Some(mtch) -> mtch
                  | None       -> failwith "unable to convert to a pattern" 

    let matches =  applyPatternG pattern firstClass
    Assert.That(Seq.length matches, Is.GreaterThan(0))
    matches |> Seq.iter (printf "%A")

type public CloneCandidateTestViewModel = {Name: string; CodeLines: string []; Pattern: Pattern}
  


let public DoCloneCandidateTest testCodeFile =
  let codelines = System.IO.File.ReadAllLines(@"c:\users\jbuedel\Projects\AgentRalphPlugin\Ralph.Core.Tests\CloneCandidateDetectionTests\TestCases\" + testCodeFile + ".cs")

  if (Seq.head codelines).Contains("Ignore") then Assert.Ignore((Seq.head codelines).Trim('/').Trim().Substring(6))

  let ast = toCompilationUnit (String.concat "\r\n" codelines)

  let firstClass = getClasses ast |> Seq.head
  let methods = firstClass |> getMethods
  let pattern = match methods |> Seq.find (fun m -> m.Name = "pattern") |> toPattern with
                | Some(mtch) -> mtch
                | None       -> failwith "unable to convert to a pattern" 

  let matches =  applyPatternG pattern firstClass
  Assert.That(Seq.length matches, Is.GreaterThan(0))
  matches |> Seq.iter (printf "%A")
  {Name = testCodeFile; CodeLines = codelines; Pattern = pattern}

