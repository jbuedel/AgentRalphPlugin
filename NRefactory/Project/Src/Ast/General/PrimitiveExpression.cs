// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="none" email=""/>
//     <version>$Revision: 3263 $</version>
// </file>

using System;
using System.Collections.Generic;
using AgentRalph.Visitors;

namespace ICSharpCode.NRefactory.Ast
{
	public class PrimitiveExpression : Expression
	{
		string stringValue;

	    public Type ValueType
	    {
	        get { return Value == null ? null : Value.GetType(); }
	    }

	    public Parser.LiteralFormat LiteralFormat { get; set; }
		public object Value { get; set; }
		
		public string StringValue {
			get {
				return stringValue;
			}
			set {
				stringValue = value == null ? String.Empty : value;
			}
		}
		
		public PrimitiveExpression(object val, string stringValue)
		{
			this.Value       = val;
			this.StringValue = stringValue;
		}
		
		public override object AcceptVisitor(IAstVisitor visitor, object data)
		{
			return visitor.VisitPrimitiveExpression(this, data);
		}

	    public override bool AcceptVisitor(AstComparisonVisitor visitor, object data)
	    {
	        return visitor.VisitPrimitiveExpression(this, data);
	    }

	    public override string ToString()
	    {
	        return String.Format("[PrimitiveExpression: Value={1}, ValueType={2}, StringValue={0}]",
			                     stringValue,
			                     Value,
                                 ValueType == null ? "null" : ValueType.FullName
			                    );
	    }
        public override IEnumerable<INode> Chilluns { get { return Children; } }
        internal override bool ShallowMatch(INode right)
        {
          var r = (PrimitiveExpression) right;
          if (this.LiteralFormat == r.LiteralFormat)
          {
            if (this.Value == null && r.Value == null)
            {
              // both null counts as a match
            }
            else if (this.Value == null || r.Value == null || !this.Value.Equals(r.Value))
            {
              // Fail if one or the other is null, or the values don't match.
              return false;
            }
          }
          return true;

        }

	}
}
