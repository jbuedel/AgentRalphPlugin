using AgentRalph.ExploreSimianResults;

internal class BlockPair
{
    public BlockPair(Block top, Block bottom)
    {
        Top = top;
        Bottom = bottom;
    }

    public Block Top { get; private set; }

    public Block Bottom { get; private set; }
}