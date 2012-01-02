using System;
using ICSharpCode.NRefactory.Ast;
using Attribute = ICSharpCode.NRefactory.Ast.Attribute;

namespace AgentRalph.Visitors
{
    // ReSharper disable MemberCanBeMadeStatic.Local

    partial class AstComparisonVisitor
    {
        public bool Match = true;

        private bool SetFailure()
        {
            return Match = false;
        }

        private bool IsMatch(Using left, Using right)
        {
            return left.Name == right.Name;
        }

        private bool IsMatch(UsingDeclaration left, UsingDeclaration right)
        {
            return true;
        }

        private bool IsMatch(LambdaExpression left, LambdaExpression right)
        {
            return true;
        }

        private bool IsMatch(QueryExpressionFromClause left, QueryExpressionFromClause right)
        {
            return true;
        }

        private bool IsMatch(QueryExpressionSelectClause left, QueryExpressionSelectClause right)
        {
            return true;
        }

        private bool IsMatch(ConditionalExpression left, ConditionalExpression right)
        {
            return true;
        }

        private bool IsMatch(AnonymousMethodExpression ame, AnonymousMethodExpression r_ame)
        {
            return true;
        }

        private bool IsMatch(UsingStatement statement, UsingStatement data)
        {
            return true;
        }

        private bool IsMatch(UnaryOperatorExpression uoe, UnaryOperatorExpression r_uoe)
        {
            return uoe.Op == r_uoe.Op;
        }

        private bool IsMatch(BinaryOperatorExpression l, BinaryOperatorExpression r)
        {
            return l.Op == r.Op;
        }

        private bool IsMatch(PrimitiveExpression l, PrimitiveExpression r)
        {
            if (l.Value == null && r.Value == null)
            {
                // both null counts as a match
            }
            else if (l.Value == null || r.Value == null || !l.Value.Equals(r.Value))
            {
                // Fail if one or the other is null, or the values don't match.
                return false;
            }
            return true;
        }

        private bool IsMatch(VariableDeclaration vd, VariableDeclaration r_vd)
        {
            return vd.Name == r_vd.Name;
        }

        private bool IsMatch(IdentifierExpression uoe, IdentifierExpression r_uoe)
        {
            return uoe.Identifier == r_uoe.Identifier;
        }

        private bool IsMatch(AssignmentExpression uoe, AssignmentExpression r_uoe)
        {
            return uoe.Op == r_uoe.Op;
        }

        private bool IsMatch(InvocationExpression uoe, InvocationExpression r_uoe)
        {
            return uoe.Arguments.Count == r_uoe.Arguments.Count;
        }

        private bool IsMatch(QueryExpression l, QueryExpression r)
        {
            return l.MiddleClauses.Count == r.MiddleClauses.Count;
        }


        private bool IsMatch(MethodDeclaration l, MethodDeclaration r)
        {
            return l.TypeReference.Type == r.TypeReference.Type;
        }

        private bool IsMatch(MemberReferenceExpression l, MemberReferenceExpression r)
        {
            return l.MemberName == r.MemberName;
        }

        private bool IsMatch(BlockStatement l, BlockStatement r)
        {
            return l.Children.Count == r.Children.Count; // ignore empty method bodies (like abstract methods)
        }

        private bool IsMatch(CatchClause left, CatchClause right)
        {
            return left.VariableName == right.VariableName;
        }

        private bool IsMatch(TryCatchStatement left, TryCatchStatement right)
        {
            return true;
        }

        private bool IsMatch(CastExpression left, CastExpression right)
        {
            return left.CastType == right.CastType;
        }

        private bool IsMatch(ArrayCreateExpression left, ArrayCreateExpression right)
        {
            return true;
        }

        private bool IsMatch(LocalVariableDeclaration left, LocalVariableDeclaration right)
        {
            return true;
        }

        private bool IsMatch(ThrowStatement left, ThrowStatement right)
        {
            return true;
        }

        private bool IsMatch(CollectionInitializerExpression left, CollectionInitializerExpression right)
        {
            return true;
        }

        private bool IsMatch(ObjectCreateExpression left, ObjectCreateExpression right)
        {
            return true;
        }

        private bool IsMatch(IfElseStatement left, IfElseStatement right)
        {
            return true;
        }

        private bool IsMatch(TypeReference l, TypeReference r)
        {
            return l.Type == r.Type;
        }

        private bool IsMatch(ParameterDeclarationExpression left, ParameterDeclarationExpression right)
        {
            return left.ParameterName == right.ParameterName;
        }

        private bool IsMatch(ReturnStatement left, ReturnStatement right)
        {
            return true;
        }

        private bool IsMatch(IndexerExpression left, IndexerExpression right)
        {
            return true;
        }

        private bool IsMatch(ExpressionStatement left, ExpressionStatement right)
        {
            return true;
        }

        private bool IsMatch(ParenthesizedExpression left, ParenthesizedExpression right)
        {
            return true;
        }

        private bool IsMatch(TypeReferenceExpression left, TypeReferenceExpression right)
        {
            return true;
        }

        private bool IsMatch(ThisReferenceExpression left, ThisReferenceExpression right)
        {
            return true;
        }

        private bool IsMatch(AddHandlerStatement left, AddHandlerStatement right)
        {
            return false;
        }

        private bool IsMatch(AddressOfExpression right, AddressOfExpression left)
        {
            return false;
        }

        private bool IsMatch(Attribute left, Attribute right)
        {
            return left.Name == right.Name;
        }

        private bool IsMatch(AttributeSection left, AttributeSection right)
        {
            return left.AttributeTarget == right.AttributeTarget;
        }

        private bool IsMatch(BaseReferenceExpression left, BaseReferenceExpression right)
        {
            return true;
        }

        private bool IsMatch(BreakStatement left, BreakStatement right)
        {
            return true;
        }

        private bool IsMatch(CaseLabel left, CaseLabel right)
        {
            return left.BinaryOperatorType == right.BinaryOperatorType;
        }

        private bool IsMatch(CheckedExpression left, CheckedExpression right)
        {
            return false;
        }

        private bool IsMatch(CheckedStatement left, CheckedStatement right)
        {
            return true;
        }

        private bool IsMatch(ClassReferenceExpression left, ClassReferenceExpression right)
        {
            return false;
        }

        private bool IsMatch(CompilationUnit left, CompilationUnit right)
        {
            return true;
        }

        private bool IsMatch(ConstructorDeclaration left, ConstructorDeclaration right)
        {
            return left.Name == right.Name;
        }

        private bool IsMatch(ConstructorInitializer left, ConstructorInitializer right)
        {
            return true;
        }

        private bool IsMatch(ContinueStatement left, ContinueStatement right)
        {
            return true;
        }

        private bool IsMatch(DefaultValueExpression left, DefaultValueExpression right)
        {
            return true;
        }

        private bool IsMatch(DelegateDeclaration left, DelegateDeclaration right)
        {
            return left.Name == right.Name;
        }

        private bool IsMatch(DestructorDeclaration left, DestructorDeclaration right)
        {
            return left.Name == right.Name;
        }

        private bool IsMatch(YieldStatement left, YieldStatement data)
        {
            return true;
        }

        private bool IsMatch(WithStatement left, WithStatement data)
        {
            return false;
        }

        private bool IsMatch(UnsafeStatement left, UnsafeStatement data)
        {
            return false;
        }

        private bool IsMatch(DeclareDeclaration left, DeclareDeclaration data)
        {
            return false;
        }

        private bool IsMatch(DirectionExpression left, DirectionExpression right)
        {
            return left.FieldDirection == right.FieldDirection;
        }

        private bool IsMatch(DoLoopStatement left, DoLoopStatement data)
        {
            return true;
        }

        private bool IsMatch(ElseIfSection left, ElseIfSection data)
        {
            return true;
        }

        private bool IsMatch(EmptyStatement left, EmptyStatement data)
        {
            return false;
        }

        private bool IsMatch(EndStatement left, EndStatement data)
        {
            return false;
        }

        private bool IsMatch(EraseStatement left, EraseStatement data)
        {
            return false;
        }

        private bool IsMatch(ErrorStatement left, ErrorStatement data)
        {
            return false;
        }

        private bool IsMatch(EventAddRegion left, EventAddRegion data)
        {
            return true;
        }

        private bool IsMatch(EventDeclaration left, EventDeclaration data)
        {
            return true;
        }

        private bool IsMatch(ForNextStatement left, ForNextStatement data)
        {
            return false;
        }

        private bool IsMatch(ForStatement left, ForStatement data)
        {
            return true;
        }

        private bool IsMatch(GotoCaseStatement left, GotoCaseStatement data)
        {
            return true;
        }

        private bool IsMatch(GotoStatement left, GotoStatement right)
        {
            return left.Label == right.Label;
        }

        private bool IsMatch(IndexerDeclaration left, IndexerDeclaration data)
        {
            return true;
        }

        private bool IsMatch(LabelStatement left, LabelStatement right)
        {
            return left.Label == right.Label;
        }

        private bool IsMatch(LockStatement left, LockStatement data)
        {
            return true;
        }

        private bool IsMatch(NamedArgumentExpression left, NamedArgumentExpression data)
        {
            return left.Name == data.Name;
        }

        private bool IsMatch(NamespaceDeclaration left, NamespaceDeclaration data)
        {
            return left.Name == data.Name;
        }

        private bool IsMatch(OnErrorStatement left, OnErrorStatement data)
        {
            return false;
        }

        private bool IsMatch(OptionDeclaration left, OptionDeclaration data)
        {
            return false;
        }

        private bool IsMatch(PointerReferenceExpression left, PointerReferenceExpression data)
        {
            return false;
        }

        private bool IsMatch(PropertyDeclaration left, PropertyDeclaration data)
        {
            return left.Name == data.Name;
        }

        private bool IsMatch(PropertyGetRegion left, PropertyGetRegion data)
        {
            return true;
        }

        private bool IsMatch(PropertySetRegion left, PropertySetRegion data)
        {
            return true;
        }

        private bool IsMatch(QueryExpressionDistinctClause left, QueryExpressionDistinctClause data)
        {
            return false;
        }

        private bool IsMatch(QueryExpressionGroupClause left, QueryExpressionGroupClause data)
        {
            return true;
        }

        private bool IsMatch(QueryExpressionGroupJoinVBClause left, QueryExpressionGroupJoinVBClause data)
        {
            return false;
        }

        private bool IsMatch(QueryExpressionGroupVBClause left, QueryExpressionGroupVBClause data)
        {
            return false;
        }

        private bool IsMatch(QueryExpressionJoinClause left, QueryExpressionJoinClause data)
        {
            return left.IntoIdentifier == data.IntoIdentifier;
        }

        private bool IsMatch(QueryExpressionJoinConditionVB left, QueryExpressionJoinConditionVB data)
        {
            return false;
        }

        private bool IsMatch(QueryExpressionJoinVBClause left, QueryExpressionJoinVBClause data)
        {
            return false;
        }

        private bool IsMatch(QueryExpressionLetClause left, QueryExpressionLetClause right)
        {
            return left.Identifier == right.Identifier;
        }

        private bool IsMatch(QueryExpressionLetVBClause left, QueryExpressionLetVBClause data)
        {
            return false;
        }

        private bool IsMatch(QueryExpressionOrderClause left, QueryExpressionOrderClause right)
        {
            return true;
        }

        private bool IsMatch(ReDimStatement left, ReDimStatement data)
        {
            return false;
        }

        private bool IsMatch(RemoveHandlerStatement left, RemoveHandlerStatement data)
        {
            return false;
        }

        private bool IsMatch(ResumeStatement left, ResumeStatement data)
        {
            return false;
        }

        private bool IsMatch(SizeOfExpression left, SizeOfExpression data)
        {
            return true;
        }

        private bool IsMatch(StackAllocExpression left, StackAllocExpression data)
        {
            return false;
        }

        private bool IsMatch(StopStatement left, StopStatement data)
        {
            return false;
        }

        private bool IsMatch(SwitchStatement left, SwitchStatement right)
        {
            return true;
        }

        private bool IsMatch(TemplateDefinition td, TemplateDefinition data)
        {
            return td.Name == data.Name;
        }

        private bool IsMatch(TypeDeclaration left, TypeDeclaration data)
        {
            return left.Name == data.Name && left.Type == data.Type;
        }

        private bool IsMatch(TypeOfExpression left, TypeOfExpression data)
        {
            return true;
        }

        private bool IsMatch(TypeOfIsExpression left, TypeOfIsExpression data)
        {
            return false;
        }

        private bool IsMatch(UncheckedExpression left, UncheckedExpression data)
        {
            return false;
        }

        private bool IsMatch(UncheckedStatement left, UncheckedStatement data)
        {
            return false;
        }

        private bool IsMatch(EventRaiseRegion left, EventRaiseRegion data)
        {
            return true;
        }


        private bool IsMatch(EventRemoveRegion left, EventRemoveRegion data)
        {
            return true;
        }

        private bool IsMatch(ExitStatement left, ExitStatement data)
        {
            return false;
        }

        private bool IsMatch(ExpressionRangeVariable left, ExpressionRangeVariable data)
        {
            return false;
        }

        private bool IsMatch(ExternAliasDirective left, ExternAliasDirective data)
        {
            return false;
        }

        private bool IsMatch(FieldDeclaration left, FieldDeclaration data)
        {
            return true;
        }

        private bool IsMatch(FixedStatement left, FixedStatement right)
        {
            return false;
        }

        private bool IsMatch(ForeachStatement left, ForeachStatement right)
        {
            return left.VariableName == right.VariableName;
        }

        private bool IsMatch(InterfaceImplementation left, InterfaceImplementation right)
        {
            return false;
        }

        private bool IsMatch(QueryExpressionAggregateClause left, QueryExpressionAggregateClause right)
        {
            return false;
        }

        private bool IsMatch(QueryExpressionOrdering left, QueryExpressionOrdering right)
        {
            return left.Direction == right.Direction;
        }

        private bool IsMatch(QueryExpressionPartitionVBClause left, QueryExpressionPartitionVBClause right)
        {
            return false;
        }

        private bool IsMatch(QueryExpressionSelectVBClause left, QueryExpressionSelectVBClause right)
        {
            return false;
        }

        private bool IsMatch(QueryExpressionWhereClause left, QueryExpressionWhereClause right)
        {
            return true;
        }

        private bool IsMatch(RaiseEventStatement left, RaiseEventStatement right)
        {
            return false;
        }
    }
    // ReSharper restore MemberCanBeMadeStatic.Local
}