//------------------------------------------------------------------------------
// <auto-generated>
//     Generated by IntelliJ parserGen
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#pragma warning disable 0168, 0219, 0108, 0414
// ReSharper disable RedundantNameQualifier
using System.Collections;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
namespace JetBrains.ReSharper.PsiPlugin.Tree {
  public static partial class RuleDeclarationNavigator {
    [JetBrains.Annotations.Pure]
    [JetBrains.Annotations.CanBeNull]
    [JetBrains.Annotations.ContractAnnotation("null <= null")]
    public static JetBrains.ReSharper.PsiPlugin.Tree.IRuleDeclaration GetByBody (JetBrains.ReSharper.PsiPlugin.Tree.IRuleBody param) {
      if (param == null) return null;
      TreeElement current = (TreeElement)param;
      if (current.parent is JetBrains.ReSharper.PsiPlugin.Tree.Impl.RuleDeclaration) {
        if (current.parent.GetChildRole (current) != JetBrains.ReSharper.PsiPlugin.Tree.Impl.RuleDeclaration.PSI_BODY) return null;
        current = current.parent;
      } else return null;
      return (JetBrains.ReSharper.PsiPlugin.Tree.IRuleDeclaration) current;
    }
    [JetBrains.Annotations.Pure]
    [JetBrains.Annotations.CanBeNull]
    [JetBrains.Annotations.ContractAnnotation("null <= null")]
    public static JetBrains.ReSharper.PsiPlugin.Tree.IRuleDeclaration GetByExtras (JetBrains.ReSharper.PsiPlugin.Tree.IExtrasDefinition param) {
      if (param == null) return null;
      TreeElement current = (TreeElement)param;
      if (current.parent is JetBrains.ReSharper.PsiPlugin.Tree.Impl.RuleDeclaration) {
        if (current.parent.GetChildRole (current) != JetBrains.ReSharper.PsiPlugin.Tree.Impl.RuleDeclaration.PSI_EXTRAS) return null;
        current = current.parent;
      } else return null;
      return (JetBrains.ReSharper.PsiPlugin.Tree.IRuleDeclaration) current;
    }
    [JetBrains.Annotations.Pure]
    [JetBrains.Annotations.CanBeNull]
    [JetBrains.Annotations.ContractAnnotation("null <= null")]
    public static JetBrains.ReSharper.PsiPlugin.Tree.IRuleDeclaration GetByParameters (JetBrains.ReSharper.PsiPlugin.Tree.IRuleBracketTypedParameters param) {
      if (param == null) return null;
      TreeElement current = (TreeElement)param;
      if (current.parent is JetBrains.ReSharper.PsiPlugin.Tree.Impl.RuleDeclaration) {
        if (current.parent.GetChildRole (current) != JetBrains.ReSharper.PsiPlugin.Tree.Impl.RuleDeclaration.PSI_PARAMETERS) return null;
        current = current.parent;
      } else return null;
      return (JetBrains.ReSharper.PsiPlugin.Tree.IRuleDeclaration) current;
    }
    [JetBrains.Annotations.Pure]
    [JetBrains.Annotations.CanBeNull]
    [JetBrains.Annotations.ContractAnnotation("null <= null")]
    public static JetBrains.ReSharper.PsiPlugin.Tree.IRuleDeclaration GetByRuleName (JetBrains.ReSharper.PsiPlugin.Tree.IRuleDeclaredName param) {
      if (param == null) return null;
      TreeElement current = (TreeElement)param;
      if (current.parent is JetBrains.ReSharper.PsiPlugin.Tree.Impl.RuleDeclaration) {
        if (current.parent.GetChildRole (current) != JetBrains.ReSharper.PsiPlugin.Tree.Impl.RuleDeclaration.PSI_RULE_NAME) return null;
        current = current.parent;
      } else return null;
      return (JetBrains.ReSharper.PsiPlugin.Tree.IRuleDeclaration) current;
    }
  }
}