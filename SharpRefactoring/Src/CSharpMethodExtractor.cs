// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Siegfried Pammer" email="sie_pam@gmx.at"/>
//     <version>$Revision: 3287 $</version>
// </file>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ICSharpCode.Core;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.PrettyPrinter;
using ICSharpCode.NRefactory.Visitors;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Dom.NRefactoryResolver;
//using ICSharpCode.SharpDevelop.Project;
//using ICSharpCode.TextEditor.Document;
using SharpRefactoring.Visitors;
using Dom = ICSharpCode.SharpDevelop.Dom;

namespace SharpRefactoring
{
	public class CSharpMethodExtractor : MethodExtractorBase
	{
		static readonly StringComparer CSharpNameComparer = StringComparer.Ordinal;
		
		public CSharpMethodExtractor()
			: base()
		{
		}
		
		protected override string GenerateCode(INode unit, bool installSpecials)
		{
			CSharpOutputVisitor visitor = new CSharpOutputVisitor();
			
			if (installSpecials) {
				SpecialNodesInserter.Install(this.specialsList, visitor);
			}
			
			unit.AcceptVisitor(visitor, null);
			return visitor.Text;
		}

        public AbstractNode GetCall(MethodDeclaration parent, MethodDeclaration method, VariableDeclaration returnVariable)
	    {
	        return CreateCaller(parent, method, returnVariable);
	    }


        // public override bool Extract(MethodDeclaration md, Window window, List<INode> children)
	    public bool Extract(ParametrizedNode parentNode, Window window, List<INode> children)
		{
            this.currentSelection = new MySelection(children.GetRange(window.Top, window.Size));

//            this.start = new Location(this.currentSelection.StartPosition.Column + 1, this.currentSelection.StartPosition.Line + 1);
//            this.end = new Location(this.currentSelection.EndPosition.Column + 1, this.currentSelection.EndPosition.Line + 1);
            this.start = this.currentSelection.StartPosition;
            this.end = this.currentSelection.EndPosition;



			this.parentNode = parentNode;
			
			MethodDeclaration newMethod = new MethodDeclaration();

			// Initialise new method
			newMethod.Body = GetBlock(currentSelection.Nodes);
			newMethod.Body.StartLocation = new Location(0,0);
			
			
			List<VariableDeclaration> possibleReturnValues = new List<VariableDeclaration>();
			List<VariableDeclaration> otherReturnValues = new List<VariableDeclaration>();

			if (!CheckForJumpInstructions(newMethod, this.currentSelection))
				return false;
			newMethod.Modifier = parentNode.Modifier;
			newMethod.Modifier &= ~(Modifiers.Internal | Modifiers.Protected | Modifiers.Private | Modifiers.Public | Modifiers.Override);
			LookupTableVisitor ltv = new LookupTableVisitor(SupportedLanguage.CSharp);
			parentNode.AcceptVisitor(ltv, null);
			var variablesList = (from list in ltv.Variables.Values
			                     from item in list
                                 select new Variable(item)).Where(v => !(v.StartPos > end || v.EndPos < start) && HasReferencesInSelection(currentSelection, v)).Union(FromParameters(newMethod)).Select(va => ResolveVariable(va));
			foreach (var variable in variablesList) {
				
				bool hasOccurrencesAfter = HasOccurrencesAfter(CSharpNameComparer, this.parentNode, end, variable.Name, variable.StartPos, variable.EndPos);
				bool isInitialized = (variable.Initializer != null) ? !variable.Initializer.IsNull : false;
				bool hasAssignment = HasAssignment(newMethod, variable);
				if (IsInSel(variable.StartPos, this.currentSelection) && hasOccurrencesAfter) {
					possibleReturnValues.Add(new VariableDeclaration(variable.Name, variable.Initializer, variable.Type));
					otherReturnValues.Add(new VariableDeclaration(variable.Name, variable.Initializer, variable.Type));
				}
				if (!(IsInSel(variable.StartPos, this.currentSelection) || IsInSel(variable.EndPos, this.currentSelection))) {
					ParameterDeclarationExpression newParam = null;
					if ((hasOccurrencesAfter && isInitialized) || variable.WasRefParam)
						newParam = new ParameterDeclarationExpression(variable.Type, variable.Name, ParameterModifiers.Ref);
					else {
						if ((hasOccurrencesAfter && hasAssignment) || variable.WasOutParam)
							newParam = new ParameterDeclarationExpression(variable.Type, variable.Name, ParameterModifiers.Out);
						else {
							if (!hasOccurrencesAfter)
								newParam = new ParameterDeclarationExpression(variable.Type, variable.Name, ParameterModifiers.None);
							else {
								if (!hasOccurrencesAfter && !isInitialized)
									newMethod.Body.Children.Insert(0, new LocalVariableDeclaration(new VariableDeclaration(variable.Name, variable.Initializer, variable.Type)));
								else
									newParam = new ParameterDeclarationExpression(variable.Type, variable.Name, ParameterModifiers.In);
							}
						}
					}
					if (newParam != null)
						newMethod.Parameters.Add(newParam);
				}
			}

			List<VariableDeclaration> paramsAsVarDecls = new List<VariableDeclaration>();
			this.beforeCallDeclarations = new List<LocalVariableDeclaration>();

			for (int i = 0; i < otherReturnValues.Count - 1; i++) {
				VariableDeclaration varDecl = otherReturnValues[i];
				paramsAsVarDecls.Add(varDecl);
				ParameterDeclarationExpression p = new ParameterDeclarationExpression(varDecl.TypeReference, varDecl.Name);
				p.ParamModifier = ParameterModifiers.Out;
				if (!newMethod.Parameters.Contains(p)) {
					newMethod.Parameters.Add(p);
				} else {
					this.beforeCallDeclarations.Add(new LocalVariableDeclaration(varDecl));
				}
			}

			CreateReturnStatement(newMethod, possibleReturnValues);

			newMethod.Name = "NewMethod";

			this.extractedMethod = newMethod;

			return true;
		}

	    private bool HasReferencesInSelection(ISelection selection, Variable variable)
	    {
	        FindReferenceVisitor frv = new FindReferenceVisitor(CSharpNameComparer, variable.Name, selection.StartPosition, selection.EndPosition);
	        var statement = new BlockStatement();
	        statement.Children = selection.Nodes;
	        statement.AcceptVisitor(frv, null);
	        return frv.Identifiers.Count > 0;

	    }

	    bool HasReferencesInSelection(MethodDeclaration newMethod, Variable variable)
		{
			FindReferenceVisitor frv = new FindReferenceVisitor(CSharpNameComparer, variable.Name,
			                                                    newMethod.Body.StartLocation, newMethod.Body.EndLocation);
			
			newMethod.AcceptVisitor(frv, null);
			return frv.Identifiers.Count > 0;
		}

		Variable ResolveVariable(Variable variable)
		{
/*			Dom.ParseInformation info = ParserService.GetParseInformation(GetCurrentFileName());
			Dom.ExpressionResult res = new Dom.ExpressionResult(variable.Name,
			                                                    Dom.DomRegion.FromLocation(variable.StartPos, variable.EndPos),
			                                                    Dom.ExpressionContext.Default, null);
			Dom.ResolveResult result = this.GetResolver().Resolve(res, info, this.textEditor.Document.TextContent);
*/
//			if (variable.Type.Type == "var") 
//				variable.Type = Dom.Refactoring.CodeGenerator.ConvertType(result.ResolvedType, new Dom.ClassFinder(result.CallingMember));

//			variable.IsReferenceType = result.ResolvedType.IsReferenceType == true;
			
			return variable;
		}
		
		IEnumerable<Variable> FromParameters(MethodDeclaration newMethod)
		{
			foreach (ParameterDeclarationExpression pde in parentNode.Parameters) {
				FindReferenceVisitor frv = new FindReferenceVisitor(CSharpNameComparer, pde.ParameterName/*, newMethod.Body.StartLocation, newMethod.Body.EndLocation*/);
				
				newMethod.AcceptVisitor(frv, null);
				if (frv.Identifiers.Count > 0) {
					pde.ParamModifier &= ~(ParameterModifiers.Params);
					
					if (parentNode is MethodDeclaration) {
						yield return new Variable((parentNode as MethodDeclaration).Body, pde);
					} else {
						throw new NotSupportedException("not supported!");
					}
				}
			}
		}
		
//		public override IOutputAstVisitor GetOutputVisitor()
//		{
//			return new CSharpOutputVisitor();
//		}

//		public override Dom.IResolver GetResolver()
//		{
//			return new NRefactoryResolver(Dom.LanguageProperties.CSharp);
//		}

        public bool Extract(MethodDeclaration md, Window window)
        {
            return Extract(md, window, md.Body.Children);
        }
	}
}
