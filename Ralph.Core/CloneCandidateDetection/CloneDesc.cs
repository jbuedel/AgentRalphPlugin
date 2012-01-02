using System.Collections.Generic;
using ICSharpCode.NRefactory.Ast;
using SharpRefactoring;

namespace AgentRalph.CloneCandidateDetection
{
    /// <summary>
    /// Not a permutation result itself.  This is not the resulting tree.
    /// </summary>
    public class CloneDesc
    {
        public readonly MethodDeclaration PermutatedMethod;

        /// <summary>
        /// The subtrees of whatever method that were included to produce the <see cref="PermutatedMethod"/>.
        /// </summary>
        public List<INode> Children
        {
            get
            {
                if (Previous != null)
                    return Previous.Children;
                return m_children;
            }
        }

        private readonly CloneDesc Previous;

        public CloneDesc(MethodDeclaration extractedMethod, Window w, List<INode> children)
        {
            PermutatedMethod = extractedMethod;
            m_children = children;
            m_window = w;
        }

        public CloneDesc(MethodDeclaration extractedMethod, CloneDesc pd, QuickFixInfo fixInfo)
        {
            PermutatedMethod = extractedMethod;
            Previous = pd;
            replacementInvocationInfo = fixInfo;
        }

        public Window Window
        {
            get
            {
                if (Previous != null)
                    return Previous.Window;
                return m_window;
            }
        }

        private readonly Window m_window;
        private readonly List<INode> m_children;

        private QuickFixInfo replacementInvocationInfo;
        public QuickFixInfo ReplacementInvocationInfo
        {
            get
            {
                replacementInvocationInfo.Invocation = ReplacementInvocation;
                return replacementInvocationInfo;
            }
            set { replacementInvocationInfo = value; }
        }

        public AbstractNode ReplacementInvocation { get; set; }
    }
}