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

using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.SamplePlugin.DaemonStage
{
  internal class UseInt16MaxValueLiteralDaemonProcess : IDaemonStageProcess
  {
    private readonly IDaemonProcess _daemonProcess;

    public UseInt16MaxValueLiteralDaemonProcess(IDaemonProcess daemonProcess)
    {
      _daemonProcess = daemonProcess;
    }

    #region IDaemonStageProcess Members

    public IDaemonProcess DaemonProcess
    {
      get { return _daemonProcess; }
    }

    public void Execute(Action<DaemonStageResult> commiter)
    {
      if (!_daemonProcess.FullRehighlightingRequired)
        return;

      var highlightings = new List<HighlightingInfo>();

      IFile file = _daemonProcess.SourceFile.GetNonInjectedPsiFile<CSharpLanguage>();
      if (file == null)
        return;

      file.ProcessChildren<IExpression>(
        expression =>
          {
            ConstantValue value = expression.ConstantValue;
            if (value.IsInteger() && Convert.ToInt32(value.Value) == Int16.MaxValue)
            {
              highlightings.Add(new HighlightingInfo(expression.GetDocumentRange(),
                                                     new UseOfInt16MaxValueLiteralHighlighting(expression)));
            }
          }
        );

      commiter(new DaemonStageResult(highlightings));
    }

    #endregion
  }
}