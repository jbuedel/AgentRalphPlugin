using System.Collections.Generic;
using AgentRalph.ExploreSimianResults;

namespace AgentRalph
{
    public interface ISet
    {
        int LineCount { get; }

        string FriendlyText { get; }

        List<IBlock> Blocks
        {
            get;
        }
    }
}