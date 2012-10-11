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

using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.SamplePlugin.DaemonStage
{
  [DaemonStage(StagesBefore = new[] {typeof (LanguageSpecificDaemonStage)})]
  internal class UseInt16MaxValueLiteralDaemonStage : IDaemonStage
  {
    public ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settingsStore)
    {
      return ErrorStripeRequest.STRIPE_AND_ERRORS;
    }

    public IEnumerable<IDaemonStageProcess> CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
    {
      IFile psiFile = process.SourceFile.GetNonInjectedPsiFile<CSharpLanguage>();
      if (psiFile == null)
        return Enumerable.Empty<IDaemonStageProcess>();

      return new [] { new UseInt16MaxValueLiteralDaemonProcess(process) };
    }
  }
}