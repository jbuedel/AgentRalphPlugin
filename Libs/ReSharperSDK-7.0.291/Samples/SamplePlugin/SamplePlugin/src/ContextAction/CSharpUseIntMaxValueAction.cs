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

using JetBrains.Application.Progress;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Psi.CSharp.CodeStyle;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.SamplePlugin.ContextAction
{
  [ContextAction(Name = Name, Group = "C#", Description = Description, Priority = -20)]
  public class CSharpUseIntMaxValueAction : UseIntMaxValueActionBase
  {
    public CSharpUseIntMaxValueAction(ICSharpContextActionDataProvider provider)
      : base(provider)
    {
    }

    protected override bool IsConstantExpression(IExpression expression)
    {
      if (expression != null)
      {
        return expression.GetContainingNode<IAttribute>(true) != null
               || expression.GetContainingNode<ISwitchLabelStatement>(true) != null
               || expression.GetContainingNode<IGotoCaseStatement>(true) != null
               || expression.GetContainingNode<ILocalConstantDeclaration>(true) != null
               || expression.GetContainingNode<IConstantDeclaration>(true) != null;
      }

      return false;
    }

    protected override void OptimizeRefs(IRangeMarker marker, IFile file)
    {
      file.OptimizeImportsAndRefs(marker, false, true, NullProgressIndicator.Instance);
    }

    protected override IExpression CreateExpression()
    {
      return ((ICSharpContextActionDataProvider)Provider).ElementFactory.CreateExpression("int.MaxValue");
    }
  }
}