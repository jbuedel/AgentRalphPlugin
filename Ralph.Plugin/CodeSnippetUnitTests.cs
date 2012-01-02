using System;
using JetBrains.Application;
using NUnit.Framework;

namespace AgentRalph.MakeEnumComparisonTypeSafe
{
    [TestFixture][Ignore("Never did get tests against snippets to work.  And didn't bother to upgrade to 4.5.")]
    public class MakeEnumComparisonTypeSafeTests
    {
        [Test]
        public void Foo()
        {
#if Resharper_4_5
            var cmd_line = new MOckCmdLine();
            TestShell t  = new TestShell(cmd_line, null, "");
            Assert.IsNotNull(SolutionManager.Instance);

            ISolution solution = SolutionManager.Instance.CurrentSolution;
            Assert.IsNotNull(solution);

            IProject project = ProjectFactory.Instance.CreateProject(new FileSystemPath("g:\temp"), ProjectKind.ANY_KIND, ProjectFileType.CSHARP, solution);
#endif

//            CSharpElementFactory.GetInstance(project).CreateFile(@"
//                enum MyEnum { First, Second }
//            ");
        }
    }

    public class MOckCmdLine : ICommandLine
    {
        public bool IsKeyDefined(string key, bool caseSensitive)
        {
            throw new NotImplementedException();
        }

        public string GetKeyValue(string key, bool caseSensitive)
        {
            throw new NotImplementedException();
        }

        public string[] GetKeyValues(string key, bool caseSensitive)
        {
            throw new NotImplementedException();
        }

        public string[] Keys
        {
            get { throw new NotImplementedException(); }
        }

        public string[] Parameters
        {
            get { throw new NotImplementedException(); }
        }
    }
}