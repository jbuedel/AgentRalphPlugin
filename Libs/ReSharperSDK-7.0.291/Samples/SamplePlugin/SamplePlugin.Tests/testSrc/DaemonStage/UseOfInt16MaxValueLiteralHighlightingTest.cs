/*
 * Copyright 2007-2011 JetBrains s.r.o.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp;
using JetBrains.ReSharper.SamplePlugin.DaemonStage;
using NUnit.Framework;

namespace JetBrains.ReSharper.SamplePlugin.Tests.DaemonStage
{
  [TestFixture]
  public class UseOfInt16MaxValueLiteralHighlightingTest : CSharpHighlightingTestBase
  {
    protected override bool HighlightingPredicate(IHighlighting highlighting, IContextBoundSettingsStore settingsstore)
    {
      return highlighting is UseOfInt16MaxValueLiteralHighlighting;
    }

    protected override string RelativeTestDataPath
    {
      get { return @"Daemon\UseOfInt16MaxValueLiteral"; }
    }

    [Test]
    [TestCase("Case1.cs")]
    public void Test(string testName)
    {
      DoTestFiles(testName);
    }
  }
}