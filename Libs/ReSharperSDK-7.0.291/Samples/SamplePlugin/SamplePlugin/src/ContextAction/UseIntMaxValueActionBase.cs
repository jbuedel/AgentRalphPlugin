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
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.DocumentManagers;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace JetBrains.ReSharper.SamplePlugin.ContextAction
{
  public abstract class UseIntMaxValueActionBase : ContextActionBase
  {
    protected readonly IContextActionDataProvider Provider;
    private const string UsedKeyWord = "int.MaxValue";
    protected const string Name = "Use System.Int32.MaxValue";
    protected const string Description = "Use System.Int32.MaxValue instead of 2147483647 constant.";
    private IExpression _selectedExpression;

    protected UseIntMaxValueActionBase(IContextActionDataProvider provider)
    {
      Provider = provider;      
    }

    public override string Text
    {
      get { return string.Format("Use {0}", UsedKeyWord); }
    }

    public override bool IsAvailable(IUserDataHolder cache)
    {
      using (ReadLockCookie.Create())
      {
        _selectedExpression = GetSelectedExpression();

        if (_selectedExpression != null)
          return !IsConstantExpression(_selectedExpression) && IsIntMaxValue(_selectedExpression);

        return false;
      }
    }

    protected abstract bool IsConstantExpression([CanBeNull] IExpression expression);

    private static bool IsIntMaxValue([CanBeNull] IExpression literal)
    {
      return GetValue(literal) == int.MaxValue;
    }

    private static int? GetValue([CanBeNull] IExpression literal)
    {
      if (literal != null && literal.IsValid())
      {
        var value = literal.ConstantValue;

        if (value.IsInteger())
          return Convert.ToInt32(value.Value);
      }

      return null;
    }

    private IExpression GetSelectedExpression()
    {
      return Provider.GetSelectedElement<IExpression>(true, true);
    }

    protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
    {
      IExpression expression = CreateExpression();
      if (expression != null)
      {
        using (WriteLockCookie.Create())
          expression = ReplaceElement(_selectedExpression, expression);
        if (expression != null)
        {
          IFile file = expression.GetContainingFile();
          ISolution solution1 = expression.GetPsiServices().Solution;
          IRangeMarker marker = expression.GetDocumentRange().CreateRangeMarker(solution1.GetComponent<DocumentManager>());
          OptimizeRefs(marker, file);
        }
      }

      return null;
    }

    private static IExpression ReplaceElement([NotNull] ITreeNode oldElement, [NotNull] ITreeNode newElement)
    {
      if (oldElement == null) throw new ArgumentNullException("oldElement");
      if (newElement == null) throw new ArgumentNullException("newElement");

      return ModificationUtil.ReplaceChild(oldElement, newElement) as IExpression;
    }

    protected abstract void OptimizeRefs(IRangeMarker marker, IFile file);
    protected abstract IExpression CreateExpression();
  }
}