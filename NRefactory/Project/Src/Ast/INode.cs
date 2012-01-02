// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 2708 $</version>
// </file>

using System;
using System.Collections.Generic;
using AgentRalph.Visitors;

namespace ICSharpCode.NRefactory.Ast
{
	public interface INode
	{
		INode Parent { 
			get;
			set;
		}
        /// <summary>
        /// This is only ever set when an AST is constructed by the VB parser, far as I can tell.
        /// </summary>
		List<INode> Children {
			get;
		}
		
		Location StartLocation {
			get;
			set;
		}
		
		Location EndLocation {
			get;
			set;
		}
		
		object UserData {
			get;
			set;
		}
		
		/// <summary>
		/// Visits all children
		/// </summary>
		/// <param name="visitor">The visitor to accept</param>
		/// <param name="data">Additional data for the visitor</param>
		/// <returns>The paremeter <paramref name="data"/></returns>
		object AcceptChildren(IAstVisitor visitor, object data);
		
		/// <summary>
		/// Accept the visitor
		/// </summary>
		/// <param name="visitor">The visitor to accept</param>
		/// <param name="data">Additional data for the visitor</param>
		/// <returns>The value the visitor returns after the visit</returns>
		object AcceptVisitor(IAstVisitor visitor, object data);
		
        bool AcceptVisitor(AstComparisonVisitor visitor, object data);
	}
}
