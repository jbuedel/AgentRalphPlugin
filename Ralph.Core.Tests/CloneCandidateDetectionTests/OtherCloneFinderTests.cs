using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AgentRalph.CloneCandidateDetection;
using ICSharpCode.NRefactory;
using NUnit.Framework;

namespace AgentRalph.Tests.CloneCandidateDetectionTests
{
    [TestFixture]
    public class OtherCloneFinderTests
    {
        private MethodsOnASingleClassCloneFinder cloneFinder;

        [SetUp]
        public void SetUp()
        {
            cloneFinder = new MethodsOnASingleClassCloneFinder(new OscillatingExtractMethodExpansionFactory());
        }

        /// <summary>
        /// Note that there are two classes.  Their respective Foos and Bars are identical,
        /// but the classes themselves do not match.  Per the specific type of clone finder
        /// we are testing here, which only scans mehods on a single class.
        /// </summary>
        [Test]
        public void RestrictsScopeToDuplicateMembersOnASingleClass()
        {
            const string codeText =
                @"
                public class One
                {
                    void Foo()
                    {
                        Console.Write(""zippy"");
                    }
                    void Bar()
                    {
                        Console.Write(""foo"");
                    }
                }
                public class Two
                {
                    void Foo()
                    {
                        Console.Write(""zippy"");
                    }
                    void Bar()
                    {
                        Console.Write(""foo"");
                    }
                } ";
            var clones = cloneFinder.GetCloneReplacements(codeText).Clones;
            PrintClones(clones);
            Assert.AreEqual(0, clones.Count, "While the input does have duplicates, they are not however within a single class.");
        }

        [Test]
        public void SimpleCaseOfIdenticalCloneMethods()
        {
            const string codeText =
                @"
                public class One
                {
                    void Foo()
                    {
                        string str = ""zippy"";
                        Console.Write(str);
                    }
                    void Bar()
                    {
                        string str = ""zippy"";
                        Console.Write(str);
                    }
                } ";
            Assert.AreEqual(2,
                            cloneFinder.GetCloneReplacements(codeText).Clones.Count,
                            "Expected 2 matches, one for Bar matches Foo, and vice versa.");
        }

        [Test]
        public void WithExtractMethodRefactoring()
        {
            // Same as above.  Just different test data.
            const string codeText =
                @"
                using System;
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
				}";
            var replacements = cloneFinder.GetCloneReplacements(codeText);

            foreach (var clone in replacements.Clones)
            {
                Console.WriteLine("---------------------------------");
                Console.WriteLine(clone);
            }

            Assert.AreEqual(1, replacements.Clones.Count);
        }

        private const string RenameLocalVariableCodeText =
            @"
                using System;
                public class One
                {
                    void Foo()
                    {
                        string foo_str = ""zippy"";
                        Console.Write(foo_str);
                    }
                    void Bar()
                    {
                        string bar_str = ""zippy"";
                        Console.Write(bar_str);
                    }
                } ";


        [Test]
        public void WithRenameLocalVariableRefactoring()
        {
            cloneFinder.AddRefactoring(new RenameLocalVariableExpansion());

            Assert.AreEqual(2,
                            cloneFinder.GetCloneReplacements(RenameLocalVariableCodeText).Clones.Count,
                            "Expected 2 matches, one for Bar matches Foo, and vice versa.");
        }

        [Test]
        [Ignore("Since I bundled variable renaming into the match method, this test doesn't make sense.  But I'm not sure about doing the match that way, so I'm keeping this test around.")]
        [Description("Needs a rename local var refactoring to match.")]
        public void WithoutRenameLocalVariableRefactoring()
        {
            Assert.AreEqual(0,
                            cloneFinder.GetCloneReplacements(RenameLocalVariableCodeText).Clones.Count,
                            "Expected 0 matches, as the refactoring was not added.");
        }

        [Test]
        public void MultipleRefactoringsDoNotStepOnEachOther()
        {
            cloneFinder.AddRefactoring(new RenameLocalVariableExpansion());

            Assert.AreEqual(2,
                            cloneFinder.GetCloneReplacements(RenameLocalVariableCodeText).Clones.Count,
                            "Expected 2 matches, one for Bar matches Foo, and vice versa.");
        }

        [Test]
        [Description("Demonstrates the extract method processing the children of statements and blocks.")]
        public void ScansAstChildren()
        {
            const string codeText =
                @"
                using System;
                public class CloneInNestedBlock
                {
                    void Foo()
                    {
                        double w = 7;
                        double l = 8;

                        if (DateTime.Now.Day == 3)
                        {
                            Console.WriteLine(""stuff"");
                        }
                        double area = l*w;
                    }
                    void Bar()
                    {
                        Console.WriteLine(""stuff"");
                    }
                }";

            var clones = cloneFinder.GetCloneReplacements(codeText).Clones;
            PrintClones(clones);

            Assert.AreEqual(1, clones.Count, "Bar should be a clone of the body of the if statement.");

            var enumeration = codeText.Split(Convert.ToChar("\n"));
            Assert.IsTrue(enumeration[clones.First().ReplacementSectionStartLine - 1].Contains(@"Console.WriteLine(""stuff"");"),
                          "Demonstrates a bug where replacement position did not take the locations of the children into account.");
        }

        private static void PrintClones(IList<QuickFixInfo> clones)
        {
            foreach (var clone in clones)
            {
                Debug.WriteLine(clone + "\n------------------------------");
            }
        }

        [Test]
        [Ignore("Needs implementation.")]
        [Description("First crack at wiring up Interceptor support.")]
        public void CollapsationInterception()
        {
            const string codeText = @"using System;

                public class One
                {
                    private void Foo()
                    {
                        int i1 = 7;
                        int i2 = 9;

                        Assert.AreEqual(i1, i2);
                    }
                    
                    [Template(Interceptor=""Baz"")]
                    private void Bar(int i1, int i2)
                    {
                        Assert.AreEqual(i1, i2);
                    }

                    private void Baz(int i1, int i2)
                    {
                        Assert.That(i1, Is.EqualTo(i2));
                    }
                }

                internal class Template : Attribute
                {
                    public string Interceptor;
                }

                internal static class Is
                {
                    public static object EqualTo(int s2)
                    {
                        throw new NotImplementedException();
                    }
                }

                internal static class Assert
                {
                    public static void AreEqual(int s1, int s2)
                    {
                        throw new NotImplementedException();
                    }

                    public static void That(int s1, object to)
                    {
                        throw new NotImplementedException();
                    }
            }";

            // The TemplateAttribute on Bar is causing this clone match to fail.
            ScanResult scanResult = cloneFinder.GetCloneReplacements(codeText);
            Assert.AreEqual(1, scanResult.Clones.Count, "Foo contains a clone of Bar.");

            QuickFixInfo clone = scanResult.Clones[0];
            Assert.IsTrue(clone.TextForACallToJanga.Contains("Baz"),
                          "The interceptor attribute should have caused a call to Baz, instead of Bar");
        }

        [Test]
        [Description("For example, AssemblyInfo.cs. Doesn't assert anything.  Simply not throwing an exception is all I am looking for here.")]
        public void CsFilesWithoutClassesAreOk()
        {
            const string codeText = @"using System;  namespace zippy {}";
            cloneFinder.GetCloneReplacements(codeText);
        }

        [Test]
        public void OverloadsDoNotCauseException()
        {
            const string codeText = @"
                public class One
                {
                    void Foo()
                    {
                        Console.Write(1);
                    }
                    void Foo(object o)
                    {
                        Console.Write(o);
                    }
                }";
            cloneFinder.GetCloneReplacements(codeText);
        }

        [Test]
        public void AbstractMembersDoNotCauseException()
        {
            const string codeText = @"	abstract class Test030
	{
		public abstract void Foo();
		public abstract void Bar();
	}";
            ScanResult scanResult = cloneFinder.GetCloneReplacements(codeText);
            TestLog.EmbedPlainText("codeText", codeText);
            Assert.AreEqual(0, scanResult.Clones.Count, "Abstract methods should not be treated as clones.");
        }

        [Test][Description("Simply exercises the event by finding the largest extracted method, and printing it.")]
        public void ExerciseOnExtractedCandidateEvent()
        {
            const string codeText = @"    
                                public class CloneInDoWhileBlock
                                {
                                    void Foo()
                                    {
                                        do
                                        {
                                            int i = 7 + 8;
                                            int j = 9 +10;
                                            Console.WriteLine(i + j);

                                            /* BEGIN */
                                            Console.WriteLine(7);
                                            /* END */
                                        }
                                        while (DateTime.Now < DateTime.Today);
                                    }

                                    private void Bar()
                                    {
                                        Console.WriteLine(7);
                                        Console.WriteLine(7);
                                        Console.WriteLine(7);
                                    }
                                }";

            MethodsOnASingleClassCloneFinder finder = new MethodsOnASingleClassCloneFinder(new OscillatingExtractMethodExpansionFactory());

            CloneDesc largest = null;

            finder.OnExtractedCandidate += ((sender, args) =>
            {
                TestLog.EmbedPlainText("Each: ", args.Candidate.PermutatedMethod.Print());

                if (largest == null || largest.PermutatedMethod.CountNodes() < args.Candidate.PermutatedMethod.CountNodes())
                    largest = args.Candidate;
            });

            finder.GetCloneReplacements(codeText);

            TestLog.EmbedPlainText("The largest:", largest.PermutatedMethod.Print());
        }

    }
}