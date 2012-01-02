using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.Util;

namespace AgentRalph.ExploreSimianResults
{
    public interface IBlock
    {
        int StartLineNumber { get; }

        int EndLineNumber { get; }

        string Text
        {
            get;
        }

        /// <summary>
        /// Used by the navigation to open the containing file.
        /// </summary>
        IProjectFile ProjectFile { get; }

        /// <summary>
        /// Used by the navigation to scroll to the correct place within the containing file.
        /// </summary>
        TextRange TextRange { get; }

        DocumentRange? Range { get; }

        /// <summary>
        /// You must override ToString() to be the contents of the tree view nodes for this block.
        /// </summary>
        /// <returns></returns>
        string ToString();

        bool SharesSourceFile(IBlock block);
    }
}