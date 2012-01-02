/*
 * Created by SharpDevelop.
 * User: HP
 * Date: 28.04.2008
 * Time: 22:18
 */

using System;
using System.Collections.Generic;

using ICSharpCode.Core;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;
using Dom = ICSharpCode.SharpDevelop.Dom;

namespace SharpRefactoring.Visitors
{
	public class FindReferenceVisitor : AbstractAstVisitor
	{
		List<IdentifierExpression> identifiers;
		string name;
		Location rangeStart, rangeEnd;
	    StringComparer comparer;
	    private readonly bool ignoreLocation;

	    public List<IdentifierExpression> Identifiers {
			get { return identifiers; }
		}
		
		public FindReferenceVisitor(StringComparer nameComparer, string name, Location rangeStart, Location rangeEnd)
		{
			this.identifiers = new List<IdentifierExpression>();
			this.name = name;
			this.rangeEnd = rangeEnd;
			this.rangeStart = rangeStart;
			this.comparer = nameComparer;
		}

		public FindReferenceVisitor(StringComparer comparer, string name)
		{
            identifiers = new List<IdentifierExpression>();
		    this.comparer = comparer;
		    this.name = name;
		    rangeEnd = Location.Empty;
		    rangeStart = Location.Empty;
		    ignoreLocation = true;
		}

	    public override object VisitIdentifierExpression(ICSharpCode.NRefactory.Ast.IdentifierExpression identifierExpression, object data)
		{
			if (Compare(identifierExpression)) {
				identifiers.Add(identifierExpression);
			}
			return base.VisitIdentifierExpression(identifierExpression, data);
		}
		
		bool Compare(IdentifierExpression ie) {
			return this.comparer.Equals(ie.Identifier, this.name) &&
			        Inside(ie.StartLocation, ie.EndLocation);
		}
		
		bool IsIn(TypeReference type, List<TypeReference> list)
		{
			foreach (TypeReference tr in list) {
				if (tr.Type == type.Type)
					return true;
			}
			
			return false;
		}
		
		bool Inside(Location start, Location end)
		{
            if(ignoreLocation)
                return true;

            // When I simply removed the location check, it started creating extracted methods with parameters that it 
            // shouldn't have.  When it's like this, it extracts methods without any parameters.
			return start >= this.rangeStart && end <= this.rangeEnd;
		}
	}
}
