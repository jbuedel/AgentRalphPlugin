// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 2708 $</version>
// </file>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using AgentRalph.Visitors;

namespace ICSharpCode.NRefactory.Ast
{
    [DebuggerDisplay("{this.Print()}")]
	public abstract class AbstractNode : INode
	{
		List<INode> children = new List<INode>();
		
		public INode Parent { get; set; }
		public Location StartLocation { get; set; }
		public Location EndLocation { get; set; }
		public object UserData { get; set; }

      public virtual object JsonData() {
        return null;
      }

      public virtual JNode ToJson() {
        var x = new JNode();
        x.name  =  GetType().Name;
        x.data = JsonData();
        x.children = Chilluns.Select(c => c.ToJson());
        x.id=GetHashCode();
        return x;
      }
		
        /// <summary>
        ///  This is only ever set when an AST is constructed by the VB parser.
        /// </summary>
		public List<INode> Children {
            [DebuggerStepThrough]
            get {
				return children;
			}
			set {
				Debug.Assert(value != null);
				children = value;
			}
		}

        public abstract IEnumerable<INode> Chilluns { get; }
		
		public virtual void AddChild(INode childNode)
		{
			Debug.Assert(childNode != null);
			children.Add(childNode);
		}
		
		public abstract object AcceptVisitor(IAstVisitor visitor, object data);
        public abstract bool AcceptVisitor(AstComparisonVisitor visitor, object data);
		
		public virtual object AcceptChildren(IAstVisitor visitor, object data)
		{
			foreach (INode child in children) {
				Debug.Assert(child != null);
				child.AcceptVisitor(visitor, data);
			}
			return data;
		}

        public virtual bool AcceptChildren(AstComparisonVisitor visitor, object data)
        {
            AbstractNode d = (AbstractNode) data;
            for (int i = 0; i < children.Count; i++)
            {
                INode child = Children[i];
                if (child == null)
                    return false;
                if(!child.AcceptVisitor(visitor, d.Children[i]))
                    return false;
            }
            return true;
        }

	    public static string GetCollectionString(ICollection collection)
		{
			StringBuilder output = new StringBuilder();
			output.Append('{');
			
			if (collection != null) {
				IEnumerator en = collection.GetEnumerator();
				bool isFirst = true;
				while (en.MoveNext()) {
					if (!isFirst) {
						output.Append(", ");
					} else {
						isFirst = false;
					}
					output.Append(en.Current == null ? "<null>" : en.Current.ToString());
				}
			} else {
				return "null";
			}
			
			output.Append('}');
			return output.ToString();
		}

      internal abstract bool ShallowMatch(INode right);
      public bool IsShallowMatch(INode right)
      {

        if (right != null && this.GetType() == right.GetType())
        {
          return this.ShallowMatch(right);
        }
        return false;
      }

	}
}
