using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;

namespace SharpRefactoring
{
    public interface ISelection
    {
        bool ContainsPosition(int column, int line);
        Location EndPosition { get; }
        Location StartPosition { get; }
        List<INode> Nodes { get; }
    }
    public class MySelection : ISelection
    {
        private readonly List<INode> Children;

        public MySelection(List<INode> children)
        {
            if(children == null || children.Count == 0)
                throw new ArgumentException();
            Children = children;
        }

        public bool ContainsPosition(int column, int line)
        {
            var location = new Location(column, line);
            return StartPosition <= location && location < EndPosition;
        }

        public Location EndPosition
        {
            get { return Children.Last().EndLocation; }
        }

        public Location StartPosition
        {
            get { return Children.First().StartLocation; }
        }

        public List<INode> Nodes
        {
            get { return Children; }
        }
    }
}