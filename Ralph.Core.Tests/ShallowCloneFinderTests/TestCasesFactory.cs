using System.Collections.Generic;
using System.IO;
using AgentRalph.CloneCandidateDetection;
using AgentRalph.Tests.CloneCandidateDetectionTests;
using NUnit.Framework;

namespace AgentRalph.Tests.ShallowCloneFinderTests
{
    [TestFixture]
    public class TestCasesFactory : CloneCandidateDetectionTestBase
    {
        public static IEnumerable<TestCaseData> MyCases
        {
            get
            {
                var files = Directory.GetFiles(@"..\..\Ralph.Core.Tests\ShallowCloneFinderTests\TestCases", "*.cs");

                var factory = new ShallowExpansionFactory();
                var clonefinder = new MethodsOnASingleClassCloneFinder(factory);

                clonefinder.AddRefactoring(new LiteralToParameterExpansion());

                return ConvertCodeFileToTestCaseData(files, clonefinder);
            }
        }
    }
}