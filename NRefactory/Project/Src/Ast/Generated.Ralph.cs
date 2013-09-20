namespace ICSharpCode.NRefactory.Ast
{
  using System;
  using System.Collections.Generic;
  using AgentRalph.Visitors;


  public partial class AddHandlerStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (AddHandlerStatement) right;
      return false;
    }
  }

  public partial class AddressOfExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (AddressOfExpression) right;
      return false;
    }
  }

  public partial class AnonymousMethodExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (AnonymousMethodExpression) right;
      return false;
    }
  }

  public partial class ArrayCreateExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (ArrayCreateExpression) right;
      return true;
    }
  }

  public partial class AssignmentExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (AssignmentExpression) right;
      return this.Op == r.Op;
    }
  }

  public partial class Attribute
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (Attribute) right;
      return false;
    }
  }

  public abstract partial class AttributedNode
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (AttributedNode) right;
      return false;
    }
  }

  public partial class AttributeSection
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (AttributeSection) right;
      return false;
    }
  }

  public partial class BaseReferenceExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (BaseReferenceExpression) right;
      return true;
    }
  }

  public partial class BinaryOperatorExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (BinaryOperatorExpression) right;
      return this.Op == r.Op;
    }

    public override object JsonData()
    {
      return this.Op.ToString();
    }
  }

  public partial class BreakStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (BreakStatement) right;
      return true;
    }
  }

  public partial class CaseLabel
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (CaseLabel) right;
      return this.Label == r.Label;
    }
  }

  public partial class CastExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (CastExpression) right;
      return true;
    }
  }

  public partial class CatchClause
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (CatchClause) right;
      return true;
    }
  }

  public partial class CheckedExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (CheckedExpression) right;
      return false;
    }
  }

  public partial class CheckedStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (CheckedStatement) right;
      return true;
    }
  }

  public partial class ClassReferenceExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (ClassReferenceExpression) right;
      return false;
    }
  }

  public partial class CollectionInitializerExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (CollectionInitializerExpression) right;
      return true;
    }
  }

  internal sealed partial class NullCollectionInitializerExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (NullCollectionInitializerExpression) right;
      return true;
    }
  }

  public partial class ConditionalExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (ConditionalExpression) right;
      return true;
    }
  }

  public partial class ConstructorDeclaration
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (ConstructorDeclaration) right;
      return false;
    }
  }

  public partial class ConstructorInitializer : AbstractNode, INullable
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (ConstructorInitializer) right;
      return false;
    }
  }

  internal sealed partial class NullConstructorInitializer
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (NullConstructorInitializer) right;
      return false;
    }
  }

  public partial class ContinueStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (ContinueStatement) right;
      return false;
    }
  }

  public partial class DeclareDeclaration
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (DeclareDeclaration) right;
      return false;
    }
  }

  public partial class DefaultValueExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (DefaultValueExpression) right;
      return false;
    }
  }

  public partial class DelegateDeclaration
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (DelegateDeclaration) right;
      return this.Name == r.Name;
    }
  }

  public partial class DestructorDeclaration
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (DestructorDeclaration) right;
      return true;
    }
  }

  public partial class DirectionExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (DirectionExpression) right;
      return true;
    }
  }

  public partial class DoLoopStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (DoLoopStatement) right;
      return false;
    }
  }

  public partial class ElseIfSection
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (ElseIfSection) right;
      return false;
    }
  }

  public partial class EmptyStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (EmptyStatement) right;
      return false;
    }
  }

  public partial class EndStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (EndStatement) right;
      return false;
    }
  }

  public partial class EraseStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (EraseStatement) right;
      return false;
    }
  }

  public partial class ErrorStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (ErrorStatement) right;
      return false;
    }
  }

  public partial class EventAddRegion
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (EventAddRegion) right;
      return false;
    }
  }

  internal sealed partial class NullEventAddRegion
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (NullEventAddRegion) right;
      return false;
    }
  }

  public abstract partial class EventAddRemoveRegion : AttributedNode, INullable
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (EventAddRemoveRegion) right;
      return false;
    }
  }

  public partial class EventDeclaration
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (EventDeclaration) right;
      return false;
    }
  }

  public partial class EventRaiseRegion
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (EventRaiseRegion) right;
      return false;
    }
  }

  internal sealed partial class NullEventRaiseRegion
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (NullEventRaiseRegion) right;
      return false;
    }
  }

  public partial class EventRemoveRegion
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (EventRemoveRegion) right;
      return false;
    }
  }

  internal sealed partial class NullEventRemoveRegion
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (NullEventRemoveRegion) right;
      return false;
    }
  }

  public partial class ExitStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (ExitStatement) right;
      return false;
    }
  }

  public partial class ExpressionRangeVariable : AbstractNode, INullable
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (ExpressionRangeVariable) right;
      return false;
    }
  }

  internal sealed partial class NullExpressionRangeVariable
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (NullExpressionRangeVariable) right;
      return false;
    }
  }

  public partial class ExpressionStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (ExpressionStatement) right;
      return true;
    }
  }

  public partial class ExternAliasDirective
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (ExternAliasDirective) right;
      return false;
    }
  }

  public partial class FieldDeclaration
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (FieldDeclaration) right;
      return false;
    }
  }

  public partial class FixedStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (FixedStatement) right;
      return false;
    }
  }

  public partial class ForeachStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (ForeachStatement) right;
      
      return this.variableName == r.variableName;
    }
  }

  public partial class ForNextStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (ForNextStatement) right;
      return false;
    }
  }

  public partial class ForStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (ForStatement) right;
      return true;
    }
  }

  public partial class GotoCaseStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (GotoCaseStatement) right;
      return false;
    }
  }

  public partial class GotoStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (GotoStatement) right;
      return this.Label == r.Label;
    }
  }

  public partial class IdentifierExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (IdentifierExpression) right;
      return this.Identifier == r.Identifier;
    }
  }

  public partial class IfElseStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (IfElseStatement) right;
      return true;
    }
  }

  public partial class IndexerDeclaration
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (IndexerDeclaration) right;
      return false;
    }
  }

  public partial class IndexerExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (IndexerExpression) right;
      return true;
    }
  }

  public partial class InterfaceImplementation
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (InterfaceImplementation) right;
      return false;
    }
  }

  public partial class InvocationExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (InvocationExpression) right;
      return true;
    }
  }

  public partial class LabelStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (LabelStatement) right;
      return this.Label == r.Label;
    }
  }

  public partial class LambdaExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (LambdaExpression) right;
      return false;
    }
  }

  public partial class LockStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (LockStatement) right;
      return true;
    }
  }

  public abstract partial class MemberNode
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (MemberNode) right;
      return false;
    }
  }

  public partial class MemberReferenceExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (MemberReferenceExpression) right;
      return this.MemberName == r.MemberName;
    }
  }

  public partial class MethodDeclaration
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (MethodDeclaration) right;
      for (int i = 0; i < this.HandlesClause.Count; i++)
      {
        if (this.HandlesClause[i] != r.HandlesClause[i]) return false;
      }
      return this.Name == r.Name && this.Modifier == r.Modifier;
    }
  }

  public partial class NamedArgumentExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (NamedArgumentExpression) right;
      return false;
    }
  }

  public partial class NamespaceDeclaration
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (NamespaceDeclaration) right;
      return false;
    }
  }

  public partial class ObjectCreateExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (ObjectCreateExpression) right;
      return true;
    }
  }

  public partial class OnErrorStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (OnErrorStatement) right;
      return false;
    }
  }

  public partial class OperatorDeclaration
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (OperatorDeclaration) right;
      return false;
    }
  }

  public partial class OptionDeclaration
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (OptionDeclaration) right;
      return false;
    }
  }

  public partial class ParameterDeclarationExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (ParameterDeclarationExpression) right;
      return false;
    }
  }

  public abstract partial class ParametrizedNode
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (ParametrizedNode) right;
      return false;
    }
  }

  public partial class ParenthesizedExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (ParenthesizedExpression) right;
      return false;
    }
  }

  public partial class PointerReferenceExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (PointerReferenceExpression) right;
      return false;
    }
  }

  public partial class PropertyDeclaration
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (PropertyDeclaration) right;
      return false;
    }
  }

  public partial class PropertyGetRegion
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (PropertyGetRegion) right;
      return false;
    }
  }

  internal sealed partial class NullPropertyGetRegion
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (NullPropertyGetRegion) right;
      return false;
    }
  }

  public abstract partial class PropertyGetSetRegion : AttributedNode, INullable
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (PropertyGetSetRegion) right;
      return false;
    }
  }

  public partial class PropertySetRegion
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (PropertySetRegion) right;
      return false;
    }
  }

  internal sealed partial class NullPropertySetRegion
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (NullPropertySetRegion) right;
      return false;
    }
  }

  public partial class QueryExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (QueryExpression) right;
      return true;
    }
  }

  internal sealed partial class NullQueryExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (NullQueryExpression) right;
      return true;
    }
  }

  public partial class QueryExpressionAggregateClause
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (QueryExpressionAggregateClause) right;
      return false;
    }
  }

  public abstract partial class QueryExpressionClause : AbstractNode, INullable
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (QueryExpressionClause) right;
      return false;
    }
  }

  internal sealed partial class NullQueryExpressionClause
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (NullQueryExpressionClause) right;
      return true;
    }
  }

  public partial class QueryExpressionDistinctClause
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (QueryExpressionDistinctClause) right;
      return true;
    }
  }

  public partial class QueryExpressionFromClause
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (QueryExpressionFromClause) right;
      return true;
    }
  }

  internal sealed partial class NullQueryExpressionFromClause
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (NullQueryExpressionFromClause) right;
      return true;
    }
  }

  public abstract partial class QueryExpressionFromOrJoinClause
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (QueryExpressionFromOrJoinClause) right;
      return false;
    }
  }

  public partial class QueryExpressionGroupClause
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (QueryExpressionGroupClause) right;
      return false;
    }
  }

  public partial class QueryExpressionGroupJoinVBClause
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (QueryExpressionGroupJoinVBClause) right;
      return true;
    }
  }

  public partial class QueryExpressionGroupVBClause
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (QueryExpressionGroupVBClause) right;
      return true;
    }
  }

  public partial class QueryExpressionJoinClause
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (QueryExpressionJoinClause) right;
      return true;
    }
  }

  public partial class QueryExpressionJoinConditionVB
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (QueryExpressionJoinConditionVB) right;
      return true;
    }
  }

  public partial class QueryExpressionJoinVBClause
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (QueryExpressionJoinVBClause) right;
      return false;
    }
  }

  internal sealed partial class NullQueryExpressionJoinVBClause
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (NullQueryExpressionJoinVBClause) right;
      return true;
    }
  }

  public partial class QueryExpressionLetClause
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (QueryExpressionLetClause) right;
      return true;
    }
  }

  public partial class QueryExpressionLetVBClause
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (QueryExpressionLetVBClause) right;
      return true;
    }
  }

  public partial class QueryExpressionOrderClause
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (QueryExpressionOrderClause) right;
      return true;
    }
  }

  public partial class QueryExpressionOrdering
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (QueryExpressionOrdering) right;
      return true;
    }
  }

  public partial class QueryExpressionPartitionVBClause
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (QueryExpressionPartitionVBClause) right;
      return true;
    }
  }

  public partial class QueryExpressionSelectClause
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (QueryExpressionSelectClause) right;
      return true;
    }
  }

  public partial class QueryExpressionSelectVBClause
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (QueryExpressionSelectVBClause) right;
      return true;
    }
  }

  public partial class QueryExpressionWhereClause
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (QueryExpressionWhereClause) right;
      return true;
    }
  }

  public partial class RaiseEventStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (RaiseEventStatement) right;
      return false;
    }
  }

  public partial class ReDimStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (ReDimStatement) right;
      return false;
    }
  }

  public partial class RemoveHandlerStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (RemoveHandlerStatement) right;
      return false;
    }
  }

  public partial class ResumeStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (ResumeStatement) right;
      return false;
    }
  }

  public partial class ReturnStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (ReturnStatement) right;
      return true;
    }
  }

  public partial class SizeOfExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (SizeOfExpression) right;
      return true;
    }
  }

  public partial class StackAllocExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (StackAllocExpression) right;
      return false;
    }
  }

  public partial class StopStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (StopStatement) right;
      return false;
    }
  }

  public partial class SwitchSection
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (SwitchSection) right;
      return false;
    }
  }

  public partial class SwitchStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (SwitchStatement) right;
      return true;
    }
  }

  public partial class TemplateDefinition
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (TemplateDefinition) right;
      return false;
    }
  }

  public partial class ThisReferenceExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (ThisReferenceExpression) right;
      return true;
    }
  }

  public partial class ThrowStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (ThrowStatement) right;
      return true;
    }
  }

  public partial class TryCatchStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (TryCatchStatement) right;
      return true;
    }
  }

  public partial class TypeDeclaration
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (TypeDeclaration) right;
      return false;
    }
  }

  public partial class TypeOfExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (TypeOfExpression) right;
      return true;
    }
  }

  public partial class TypeOfIsExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (TypeOfIsExpression) right;
      return false;
    }
  }

  public partial class TypeReferenceExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (TypeReferenceExpression) right;
      return true;
    }
  }

  public partial class UnaryOperatorExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (UnaryOperatorExpression) right;
      return this.Op == r.Op;
    }
  }

  public partial class UncheckedExpression
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (UncheckedExpression) right;
      return false;
    }
  }

  public partial class UncheckedStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (UncheckedStatement) right;
      return false;
    }
  }

  public partial class UnsafeStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (UnsafeStatement) right;
      return false;
    }
  }

  public partial class Using
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (Using) right;
      return false;
    }
  }

  public partial class UsingDeclaration
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (UsingDeclaration) right;
      return false;
    }
  }

  public partial class UsingStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (UsingStatement) right;

      return true;
    }
  }

  public partial class VariableDeclaration
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (VariableDeclaration) right;
      return this.Name == r.Name;
    }
  }

  public partial class WithStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (WithStatement) right;
      return false;
    }
  }

  public partial class YieldStatement
  {
    internal override bool ShallowMatch(INode right)
    {
      var r = (YieldStatement) right;
      return true;
    }
  }
}