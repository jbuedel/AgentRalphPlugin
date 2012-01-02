using System.Collections.Generic;
using AgentRalph.ExploreSimianResults;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;

namespace AgentRalph
{
    public class Set : ISet
    {
        private readonly List<IBlock> blocks = new List<IBlock>();
        private readonly int lineCount;

        public Set(int lineCount)
        {
            this.lineCount = lineCount;
        }

        public Set()
        {
            
        }

        public int LineCount
        {
            get { return lineCount; }
        }

        public string FriendlyText
        {
            get { return string.Format("{0} lines repeated {1} times", LineCount, blocks.Count); }
        }

        public List<IBlock> Blocks
        {
            get { return blocks; }
        }

        public void AddBlock(string sourceFile, int startLineNumber, int endLineNumber, IProjectFile projectFile, DocumentRange? range)
        {
            blocks.Add(new Block(startLineNumber, endLineNumber, projectFile, this, range));
        }

        public void AddBlock(string sourceFile, int startLineNumber)
        {
            AddBlock(sourceFile, startLineNumber, startLineNumber + lineCount - 1, null, new DocumentRange());
        }

        public static ISet ToIste(Set input)
        {
            return input;
        }

        public static bool operator==(Set s, Set s1)
        {
            return s.Equals(s1);
        }

        public static bool operator !=(Set s, Set s1)
        {
            return !(s == s1);
        }

        private bool Equals(Set s1)
        {
            IBlock block1 = Blocks[0];
            IBlock block2 = s1.blocks[0];

            return block1.SharesSourceFile(block2) && block1.StartLineNumber == block2.StartLineNumber && block1.EndLineNumber == block2.EndLineNumber;

        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Set)) return false;
            return Equals((Set) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((blocks != null ? blocks.GetHashCode() : 0)*397) ^ lineCount;
            }
        }
    }
}