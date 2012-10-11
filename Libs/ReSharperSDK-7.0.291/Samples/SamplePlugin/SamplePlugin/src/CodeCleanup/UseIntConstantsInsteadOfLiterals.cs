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
using System.ComponentModel;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.CodeCleanup;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using IExpression = JetBrains.ReSharper.Psi.Tree.IExpression;

namespace JetBrains.ReSharper.SamplePlugin.CodeCleanup
{
  [CodeCleanupModule]
  public class UseIntConstantsInsteadOfLiterals : ICodeCleanupModule
  {
    private readonly IShellLocks _shellLocks;
    private static readonly Descriptor DescriptorInstance = new Descriptor();

    public UseIntConstantsInsteadOfLiterals(IShellLocks shellLocks)
    {
      _shellLocks = shellLocks;
    }

    public void SetDefaultSetting(CodeCleanupProfile profile, Feature.Services.CodeCleanup.CodeCleanup.DefaultProfileType profileType)
    {
      switch (profileType)
      {
        case Feature.Services.CodeCleanup.CodeCleanup.DefaultProfileType.FULL:
          profile.SetSetting(DescriptorInstance, true);
          break;

        case Feature.Services.CodeCleanup.CodeCleanup.DefaultProfileType.REFORMAT:
          profile.SetSetting(DescriptorInstance, false);
          break;
        default:
          throw new ArgumentOutOfRangeException("profileType");
      }
    }

    public bool IsAvailable(IPsiSourceFile sourceFile)
    {
      return sourceFile.GetNonInjectedPsiFile<CSharpLanguage>() != null;
    }

    public void Process(IPsiSourceFile sourceFile, IRangeMarker rangeMarker, CodeCleanupProfile profile, IProgressIndicator progressIndicator)
    {
      var file = sourceFile.GetNonInjectedPsiFile<CSharpLanguage>();
      if (file == null)
        return;

      if (!profile.GetSetting(DescriptorInstance))
        return;

      CSharpElementFactory elementFactory = CSharpElementFactory.GetInstance(sourceFile.PsiModule);
      
      file.GetPsiServices().PsiManager.DoTransaction(
        () =>
          {
            using (_shellLocks.UsingWriteLock())
              file.ProcessChildren<IExpression>(
                expression =>
                  {
                    ConstantValue value = expression.ConstantValue;
                    if (value.IsInteger() && Convert.ToInt32(value.Value) == int.MaxValue)
                      ModificationUtil.ReplaceChild(expression, elementFactory.CreateExpression("int.MaxValue"));
                  }
                );
          },
        "Code cleanup");
    }

    public PsiLanguageType LanguageType
    {
      get { return CSharpLanguage.Instance; }
    }

    public ICollection<CodeCleanupOptionDescriptor> Descriptors
    {
      get { return new CodeCleanupOptionDescriptor[] { DescriptorInstance }; }
    }

    public bool IsAvailableOnSelection
    {
      get { return false; }
    }

    [DefaultValue(false)]
    [DisplayName("Use int.MaxValue literal")]
    [Category(CSharpCategory)]
    private class Descriptor : CodeCleanupBoolOptionDescriptor
    {
      public Descriptor() : base("UseIntLiterals") { }
    }
  }
}