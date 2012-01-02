using NUnit.Framework;

namespace AgentRalph.Tests
{
    [TestFixture]
    public class IndexableMethodFinderVisitorTests
    {
        [Test]
        public void FindsMethodsAndAllowsStringIndexesTest()
        {
            var cu = AstMatchHelper.ParseToCompilationUnit(@"
				class FooBar
				{
					void Foo()
					{
						Console.WriteLine(""Hello world"");
						Console.WriteLine(""2nd line"");
						Console.WriteLine(""3rd line"");
						Console.WriteLine(""fourth"");
					}
					void Template() 
					{
						Console.WriteLine(""3rd line"");
					}
				}");

            IndexableMethodFinderVisitor mfv = new IndexableMethodFinderVisitor();
            cu.AcceptVisitor(mfv, null);

            Assert.IsNotNull(mfv.Methods["Template"]);
            Assert.IsNotNull(mfv.Methods["Foo"]);
        }

        [Test]
        public void FindsClasses()
        {
            var cu = AstMatchHelper.ParseToCompilationUnit(@"
				class FooBar
				{
					void Foo() { }
					void Bar() { }
				}
				class BarFoo
				{
					void Foo() { }
					void Bar() { }
				}");

            IndexableClassFinderVisitor mfv = new IndexableClassFinderVisitor();
            cu.AcceptVisitor(mfv, null);

            Assert.IsNotNull(mfv.Classes["FooBar"]);
            Assert.IsNotNull(mfv.Classes["BarFoo"]);
        }
    }
}