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

using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.DocumentManagers;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.CodeStyle;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;

namespace JetBrains.ReSharper.SamplePlugin.QuickFix
{
  public class UseOfInt16MaxValueLiteralBulbItem : IBulbAction
  {
    private readonly IExpression _literalExpression;

    public UseOfInt16MaxValueLiteralBulbItem(IExpression literalExpression)
    {
      _literalExpression = literalExpression;
    }

    public string Text
    {
      get { return string.Format("Replace '{0}' with 'Int16.MaxValue'", _literalExpression.GetText()); }
    }

    public void Execute(ISolution solution, ITextControl textControl)
    {
      if (!_literalExpression.IsValid())
        return;

      IFile containingFile = _literalExpression.GetContainingFile();

      CSharpElementFactory elementFactory = CSharpElementFactory.GetInstance(_literalExpression.GetPsiModule());

      IExpression newExpression = null;
      _literalExpression.GetPsiServices().PsiManager.DoTransaction(
        () =>
          {
            using (solution.GetComponent<IShellLocks>().UsingWriteLock())
              newExpression = ModificationUtil.ReplaceChild(
                _literalExpression, elementFactory.CreateExpression("System.Int16.MaxValue"));
          }, GetType().Name);

      if (newExpression != null)
      {
        IRangeMarker marker = newExpression.GetDocumentRange().CreateRangeMarker(solution.GetComponent<DocumentManager>());
        containingFile.OptimizeImportsAndRefs(
          marker, false, true, NullProgressIndicator.Instance);
      }
    }
  }
}