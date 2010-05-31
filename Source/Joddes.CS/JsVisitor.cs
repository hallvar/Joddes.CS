using System;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;

namespace Joddes.CS {
    public abstract class JsVisitor : IAstVisitor {

        protected Exception CreateException(INode node, string message) {
            if(String.IsNullOrEmpty(message))
                message = String.Format("Language construction {0} is not supported", node.GetType().Name);
            return new ApplicationException(string.Format("{0} {1}", message, node.StartLocation));
        }
        protected Exception CreateException(INode node) {
            return CreateException(node, null);
        }


        public virtual object VisitAddHandlerStatement(AddHandlerStatement addHandlerStatement, object data) {
            throw CreateException(addHandlerStatement);
        }

        public virtual object VisitAddressOfExpression(AddressOfExpression addressOfExpression, object data) {
            throw CreateException(addressOfExpression);
        }

        public virtual object VisitAnonymousMethodExpression(AnonymousMethodExpression anonymousMethodExpression, object data) {
            throw CreateException(anonymousMethodExpression);
        }

        public virtual object VisitArrayCreateExpression(ArrayCreateExpression arrayCreateExpression, object data) {
            throw CreateException(arrayCreateExpression);
        }

        public virtual object VisitAssignmentExpression(AssignmentExpression assignmentExpression, object data) {
            throw CreateException(assignmentExpression);
        }

        public virtual object VisitAttribute(ICSharpCode.NRefactory.Ast.Attribute attribute, object data) {
            throw CreateException(attribute);
        }

        public virtual object VisitAttributeSection(AttributeSection attributeSection, object data) {
            throw CreateException(attributeSection);
        }

        public virtual object VisitBaseReferenceExpression(BaseReferenceExpression baseReferenceExpression, object data) {
            throw CreateException(baseReferenceExpression);
        }

        public virtual object VisitBinaryOperatorExpression(BinaryOperatorExpression binaryOperatorExpression, object data) {
            throw CreateException(binaryOperatorExpression);
        }

        public virtual object VisitBlockStatement(BlockStatement blockStatement, object data) {
            throw CreateException(blockStatement);
        }

        public virtual object VisitBreakStatement(BreakStatement breakStatement, object data) {
            throw CreateException(breakStatement);
        }

        public virtual object VisitCaseLabel(CaseLabel caseLabel, object data) {
            throw CreateException(caseLabel);
        }

        public virtual object VisitCastExpression(CastExpression castExpression, object data) {
            throw CreateException(castExpression);
        }

        public virtual object VisitCatchClause(CatchClause catchClause, object data) {
            throw CreateException(catchClause);
        }

        public virtual object VisitCheckedExpression(CheckedExpression checkedExpression, object data) {
            throw CreateException(checkedExpression);
        }

        public virtual object VisitCheckedStatement(CheckedStatement checkedStatement, object data) {
            throw CreateException(checkedStatement);
        }

        public virtual object VisitClassReferenceExpression(ClassReferenceExpression classReferenceExpression, object data) {
            throw CreateException(classReferenceExpression);
        }

        public virtual object VisitCollectionInitializerExpression(CollectionInitializerExpression collectionInitializerExpression, object data) {
            throw CreateException(collectionInitializerExpression);
        }

        public virtual object VisitCompilationUnit(CompilationUnit compilationUnit, object data) {
            throw CreateException(compilationUnit);
        }

        public virtual object VisitConditionalExpression(ConditionalExpression conditionalExpression, object data) {
            throw CreateException(conditionalExpression);
        }

        public virtual object VisitConstructorDeclaration(ConstructorDeclaration constructorDeclaration, object data) {
            throw CreateException(constructorDeclaration);
        }

        public virtual object VisitConstructorInitializer(ConstructorInitializer constructorInitializer, object data) {
            throw CreateException(constructorInitializer);
        }

        public virtual object VisitContinueStatement(ContinueStatement continueStatement, object data) {
            throw CreateException(continueStatement);
        }

        public virtual object VisitDeclareDeclaration(DeclareDeclaration declareDeclaration, object data) {
            throw CreateException(declareDeclaration);
        }

        public virtual object VisitDefaultValueExpression(DefaultValueExpression defaultValueExpression, object data) {
            throw CreateException(defaultValueExpression);
        }

        public virtual object VisitDelegateDeclaration(DelegateDeclaration delegateDeclaration, object data) {
            throw CreateException(delegateDeclaration);
        }

        public virtual object VisitDestructorDeclaration(DestructorDeclaration destructorDeclaration, object data) {
            throw CreateException(destructorDeclaration);
        }

        public virtual object VisitDirectionExpression(DirectionExpression directionExpression, object data) {
            throw CreateException(directionExpression);
        }

        public virtual object VisitDoLoopStatement(DoLoopStatement doLoopStatement, object data) {
            throw CreateException(doLoopStatement);
        }

        public virtual object VisitElseIfSection(ElseIfSection elseIfSection, object data) {
            throw CreateException(elseIfSection);
        }

        public virtual object VisitEmptyStatement(EmptyStatement emptyStatement, object data) {
            throw CreateException(emptyStatement);
        }

        public virtual object VisitEndStatement(EndStatement endStatement, object data) {
            throw CreateException(endStatement);
        }

        public virtual object VisitEraseStatement(EraseStatement eraseStatement, object data) {
            throw CreateException(eraseStatement);
        }

        public virtual object VisitErrorStatement(ErrorStatement errorStatement, object data) {
            throw CreateException(errorStatement);
        }

        public virtual object VisitEventAddRegion(EventAddRegion eventAddRegion, object data) {
            throw CreateException(eventAddRegion);
        }

        public virtual object VisitEventDeclaration(EventDeclaration eventDeclaration, object data) {
            throw CreateException(eventDeclaration);
        }

        public virtual object VisitEventRaiseRegion(EventRaiseRegion eventRaiseRegion, object data) {
            throw CreateException(eventRaiseRegion);
        }

        public virtual object VisitEventRemoveRegion(EventRemoveRegion eventRemoveRegion, object data) {
            throw CreateException(eventRemoveRegion);
        }

        public virtual object VisitExitStatement(ExitStatement exitStatement, object data) {
            throw CreateException(exitStatement);
        }

        public virtual object VisitExpressionRangeVariable(ExpressionRangeVariable expressionRangeVariable, object data) {
            throw CreateException(expressionRangeVariable);
        }

        public virtual object VisitExpressionStatement(ExpressionStatement expressionStatement, object data) {
            throw CreateException(expressionStatement);
        }

        public virtual object VisitExternAliasDirective(ExternAliasDirective externAliasDirective, object data) {
            throw CreateException(externAliasDirective);
        }

        public virtual object VisitFieldDeclaration(FieldDeclaration fieldDeclaration, object data) {
            throw CreateException(fieldDeclaration);
        }

        public virtual object VisitFixedStatement(FixedStatement fixedStatement, object data) {
            throw CreateException(fixedStatement); ;
        }

        public virtual object VisitForeachStatement(ForeachStatement foreachStatement, object data) {
            throw CreateException(foreachStatement);
        }

        public virtual object VisitForNextStatement(ForNextStatement forNextStatement, object data) {
            throw CreateException(forNextStatement);
        }

        public virtual object VisitForStatement(ForStatement forStatement, object data) {
            throw CreateException(forStatement);
        }

        public virtual object VisitGotoCaseStatement(GotoCaseStatement gotoCaseStatement, object data) {
            throw CreateException(gotoCaseStatement);
        }

        public virtual object VisitGotoStatement(GotoStatement gotoStatement, object data) {
            throw CreateException(gotoStatement);
        }

        public virtual object VisitIdentifierExpression(IdentifierExpression identifierExpression, object data) {
            throw CreateException(identifierExpression);
        }

        public virtual object VisitIfElseStatement(IfElseStatement ifElseStatement, object data) {
            throw CreateException(ifElseStatement);
        }

        public virtual object VisitIndexerDeclaration(IndexerDeclaration indexerDeclaration, object data) {
            throw CreateException(indexerDeclaration);
        }

        public virtual object VisitIndexerExpression(IndexerExpression indexerExpression, object data) {
            throw CreateException(indexerExpression);
        }

        public virtual object VisitInnerClassTypeReference(InnerClassTypeReference innerClassTypeReference, object data) {
            throw CreateException(innerClassTypeReference);
        }

        public virtual object VisitInterfaceImplementation(InterfaceImplementation interfaceImplementation, object data) {
            throw CreateException(interfaceImplementation);
        }

        public virtual object VisitInvocationExpression(InvocationExpression invocationExpression, object data) {
            throw CreateException(invocationExpression);
        }

        public virtual object VisitLabelStatement(LabelStatement labelStatement, object data) {
            throw CreateException(labelStatement);
        }

        public virtual object VisitLambdaExpression(LambdaExpression lambdaExpression, object data) {
            throw CreateException(lambdaExpression);
        }

        public virtual object VisitLocalVariableDeclaration(LocalVariableDeclaration localVariableDeclaration, object data) {
            throw CreateException(localVariableDeclaration);
        }

        public virtual object VisitLockStatement(LockStatement lockStatement, object data) {
            throw CreateException(lockStatement);
        }

        public virtual object VisitMemberReferenceExpression(MemberReferenceExpression memberReferenceExpression, object data) {
            throw CreateException(memberReferenceExpression);
        }

        public virtual object VisitMethodDeclaration(MethodDeclaration methodDeclaration, object data) {
            throw CreateException(methodDeclaration);
        }

        public virtual object VisitNamedArgumentExpression(NamedArgumentExpression namedArgumentExpression, object data) {
            throw CreateException(namedArgumentExpression);
        }

        public virtual object VisitNamespaceDeclaration(NamespaceDeclaration namespaceDeclaration, object data) {
            throw CreateException(namespaceDeclaration);
        }

        public virtual object VisitObjectCreateExpression(ObjectCreateExpression objectCreateExpression, object data) {
            throw CreateException(objectCreateExpression);
        }

        public virtual object VisitOnErrorStatement(OnErrorStatement onErrorStatement, object data) {
            throw CreateException(onErrorStatement);
        }

        public virtual object VisitOperatorDeclaration(OperatorDeclaration operatorDeclaration, object data) {
            throw CreateException(operatorDeclaration);
        }

        public virtual object VisitOptionDeclaration(OptionDeclaration optionDeclaration, object data) {
            throw CreateException(optionDeclaration);
        }

        public virtual object VisitParameterDeclarationExpression(ParameterDeclarationExpression parameterDeclarationExpression, object data) {
            throw CreateException(parameterDeclarationExpression);
        }

        public virtual object VisitParenthesizedExpression(ParenthesizedExpression parenthesizedExpression, object data) {
            throw CreateException(parenthesizedExpression);
        }

        public virtual object VisitPointerReferenceExpression(PointerReferenceExpression pointerReferenceExpression, object data) {
            throw CreateException(pointerReferenceExpression);
        }

        public virtual object VisitPrimitiveExpression(PrimitiveExpression primitiveExpression, object data) {
            throw CreateException(primitiveExpression);
        }

        public virtual object VisitPropertyDeclaration (PropertyDeclaration propertyDeclaration, object data)
        {
        	//throw CreateException(propertyDeclaration);
        	return null;
		}

        public virtual object VisitPropertyGetRegion(PropertyGetRegion propertyGetRegion, object data) {
            throw CreateException(propertyGetRegion);
        }

        public virtual object VisitPropertySetRegion(PropertySetRegion propertySetRegion, object data) {
            throw CreateException(propertySetRegion);
        }

        public virtual object VisitQueryExpression(QueryExpression queryExpression, object data) {
            throw CreateException(queryExpression);
        }

        public virtual object VisitQueryExpressionAggregateClause(QueryExpressionAggregateClause queryExpressionAggregateClause, object data) {
            throw CreateException(queryExpressionAggregateClause);
        }

        public virtual object VisitQueryExpressionDistinctClause(QueryExpressionDistinctClause queryExpressionDistinctClause, object data) {
            throw CreateException(queryExpressionDistinctClause);
        }

        public virtual object VisitQueryExpressionFromClause(QueryExpressionFromClause queryExpressionFromClause, object data) {
            throw CreateException(queryExpressionFromClause);
        }

        public virtual object VisitQueryExpressionGroupClause(QueryExpressionGroupClause queryExpressionGroupClause, object data) {
            throw CreateException(queryExpressionGroupClause);
        }

        public virtual object VisitQueryExpressionGroupJoinVBClause(QueryExpressionGroupJoinVBClause queryExpressionGroupJoinVBClause, object data) {
            throw CreateException(queryExpressionGroupJoinVBClause);
        }

        public virtual object VisitQueryExpressionGroupVBClause(QueryExpressionGroupVBClause queryExpressionGroupVBClause, object data) {
            throw CreateException(queryExpressionGroupVBClause);
        }

        public virtual object VisitQueryExpressionJoinClause(QueryExpressionJoinClause queryExpressionJoinClause, object data) {
            throw CreateException(queryExpressionJoinClause);
        }

        public virtual object VisitQueryExpressionJoinConditionVB(QueryExpressionJoinConditionVB queryExpressionJoinConditionVB, object data) {
            throw CreateException(queryExpressionJoinConditionVB);
        }

        public virtual object VisitQueryExpressionJoinVBClause(QueryExpressionJoinVBClause queryExpressionJoinVBClause, object data) {
            throw CreateException(queryExpressionJoinVBClause);
        }

        public virtual object VisitQueryExpressionLetClause(QueryExpressionLetClause queryExpressionLetClause, object data) {
            throw CreateException(queryExpressionLetClause);
        }

        public virtual object VisitQueryExpressionLetVBClause(QueryExpressionLetVBClause queryExpressionLetVBClause, object data) {
            throw CreateException(queryExpressionLetVBClause);
        }

        public virtual object VisitQueryExpressionOrderClause(QueryExpressionOrderClause queryExpressionOrderClause, object data) {
            throw CreateException(queryExpressionOrderClause);
        }

        public virtual object VisitQueryExpressionOrdering(QueryExpressionOrdering queryExpressionOrdering, object data) {
            throw CreateException(queryExpressionOrdering);
        }

        public virtual object VisitQueryExpressionPartitionVBClause(QueryExpressionPartitionVBClause queryExpressionPartitionVBClause, object data) {
            throw CreateException(queryExpressionPartitionVBClause);
        }

        public virtual object VisitQueryExpressionSelectClause(QueryExpressionSelectClause queryExpressionSelectClause, object data) {
            throw CreateException(queryExpressionSelectClause);
        }

        public virtual object VisitQueryExpressionSelectVBClause(QueryExpressionSelectVBClause queryExpressionSelectVBClause, object data) {
            throw CreateException(queryExpressionSelectVBClause);
        }

        public virtual object VisitQueryExpressionWhereClause(QueryExpressionWhereClause queryExpressionWhereClause, object data) {
            throw CreateException(queryExpressionWhereClause);
        }

        public virtual object VisitRaiseEventStatement(RaiseEventStatement raiseEventStatement, object data) {
            throw CreateException(raiseEventStatement);
        }

        public virtual object VisitReDimStatement(ReDimStatement reDimStatement, object data) {
            throw CreateException(reDimStatement);
        }

        public virtual object VisitRemoveHandlerStatement(RemoveHandlerStatement removeHandlerStatement, object data) {
            throw CreateException(removeHandlerStatement);
        }

        public virtual object VisitResumeStatement(ResumeStatement resumeStatement, object data) {
            throw CreateException(resumeStatement);
        }

        public virtual object VisitReturnStatement(ReturnStatement returnStatement, object data) {
            throw CreateException(returnStatement);
        }

        public virtual object VisitSizeOfExpression(SizeOfExpression sizeOfExpression, object data) {
            throw CreateException(sizeOfExpression);
        }

        public virtual object VisitStackAllocExpression(StackAllocExpression stackAllocExpression, object data) {
            throw CreateException(stackAllocExpression);
        }

        public virtual object VisitStopStatement(StopStatement stopStatement, object data) {
            throw CreateException(stopStatement);
        }

        public virtual object VisitSwitchSection(SwitchSection switchSection, object data) {
            throw CreateException(switchSection);
        }

        public virtual object VisitSwitchStatement(SwitchStatement switchStatement, object data) {
            throw CreateException(switchStatement);
        }

        public virtual object VisitTemplateDefinition(TemplateDefinition templateDefinition, object data) {
            throw CreateException(templateDefinition);
        }

        public virtual object VisitThisReferenceExpression(ThisReferenceExpression thisReferenceExpression, object data) {
            throw CreateException(thisReferenceExpression);
        }

        public virtual object VisitThrowStatement(ThrowStatement throwStatement, object data) {
            throw CreateException(throwStatement);
        }

        public virtual object VisitTryCatchStatement(TryCatchStatement tryCatchStatement, object data) {
            throw CreateException(tryCatchStatement);
        }

        public virtual object VisitTypeDeclaration(TypeDeclaration typeDeclaration, object data) {
            throw CreateException(typeDeclaration);
        }

        public virtual object VisitTypeOfExpression(TypeOfExpression typeOfExpression, object data) {
            throw CreateException(typeOfExpression);
        }

        public virtual object VisitTypeOfIsExpression(TypeOfIsExpression typeOfIsExpression, object data) {
            throw CreateException(typeOfIsExpression);
        }

        public virtual object VisitTypeReference(TypeReference typeReference, object data) {
            throw CreateException(typeReference);
        }

        public virtual object VisitTypeReferenceExpression(TypeReferenceExpression typeReferenceExpression, object data) {
            throw CreateException(typeReferenceExpression);
        }

        public virtual object VisitUnaryOperatorExpression(UnaryOperatorExpression unaryOperatorExpression, object data) {
            throw CreateException(unaryOperatorExpression);
        }

        public virtual object VisitUncheckedExpression(UncheckedExpression uncheckedExpression, object data) {
            throw CreateException(uncheckedExpression);
        }

        public virtual object VisitUncheckedStatement(UncheckedStatement uncheckedStatement, object data) {
            throw CreateException(uncheckedStatement);
        }

        public virtual object VisitUnsafeStatement(UnsafeStatement unsafeStatement, object data) {
            throw CreateException(unsafeStatement);
        }

        public virtual object VisitUsing(Using @using, object data) {
            throw CreateException(@using);
        }

        public virtual object VisitUsingDeclaration(UsingDeclaration usingDeclaration, object data) {
            throw CreateException(usingDeclaration);
        }

        public virtual object VisitUsingStatement(UsingStatement usingStatement, object data) {
            throw CreateException(usingStatement);
        }

        public virtual object VisitVariableDeclaration(VariableDeclaration variableDeclaration, object data) {
            throw CreateException(variableDeclaration);
        }

        public virtual object VisitWithStatement(WithStatement withStatement, object data) {
            throw CreateException(withStatement);
        }

        public virtual object VisitYieldStatement(YieldStatement yieldStatement, object data) {
            throw CreateException(yieldStatement);
        }
    }
}
