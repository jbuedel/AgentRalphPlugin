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

using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;
using JetBrains.DataFlow;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Refactorings.Function2Property;
using JetBrains.ReSharper.Refactorings.Rename;
using JetBrains.TextControl;
using DataConstants = JetBrains.ReSharper.Psi.Services.DataConstants;

namespace JetBrains.ReSharper.SamplePlugin.CallRename
{
  [SolutionComponent]
  public class CallRenameUtil
  {
    private readonly ActionManager _actionManager;
    private readonly ISolution _solution;
    private readonly RenameRefactoringService _renameRefactoringService;

    public CallRenameUtil(ActionManager actionManager, ISolution solution, RenameRefactoringService renameRefactoringService)
    {
      _actionManager = actionManager;
      _solution = solution;
      _renameRefactoringService = renameRefactoringService;
    }

    public void CallRename(IDeclaredElement declaredElement, ITextControl textControl, string newName)
    {
      Lifetimes.Using(
        lifetime => _renameRefactoringService.ExcuteRename(
          _actionManager.DataContexts.CreateWithDataRules(
            lifetime,
            DataRules
              .AddRule("ManualChangeNameFix", DataConstants.DECLARED_ELEMENTS, new [] {declaredElement})
              .AddRule("ManualChangeNameFix", TextControl.DataContext.DataConstants.TEXT_CONTROL, textControl)
              .AddRule("ManualChangeNameFix", ProjectModel.DataContext.DataConstants.SOLUTION, _solution)
              .AddRule("ManualChangeNameFix",
                RenameWorkflow.RenameDataProvider, new SimpleRenameDataProvider(newName)))));
    }
  }
}