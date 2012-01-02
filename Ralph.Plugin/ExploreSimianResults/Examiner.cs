using System.Collections.Generic;
using AgentRalph.ExploreSimianResults;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;

namespace AgentRalph
{
    public class Examiner
    {
        public Examiner(ISolution solution)
        {
            Solution = solution;
        }

        // REFACTOR:  Possibly change this algorithm to use a sorted list of blocks.  Then the closest block is always the next one (unless they aren't in the same file).
        private ISolution Solution { get; set; }

        public List<ISet> Examine(IEnumerable<Set> sets)
        {
            // For every block, find the next closest following (can't come before) block.
            List<BlockPair> pairs = new List<BlockPair>();
            foreach (Set set in sets)
            {
                foreach (Block block in set.Blocks)
                {
                    Block closest = FindClosestBlock(sets, block);
                    if (closest != null)
                        pairs.Add(new BlockPair(block, closest));
                }
            }

            // Do any closest block pairs have their top set and bottom set the same? 
            // That is, the diff between the two is their middles.
            foreach (BlockPair pair in pairs)
            {
                foreach (BlockPair pair1 in pairs)
                {
                    if (pair != pair1)
                    {
                        if (pair.Top.Set == pair1.Top.Set && pair.Bottom.Set == pair1.Bottom.Set)
                        {
                            Set set = new Set();

                            IProjectFile project_file = pair.Top.ProjectFile;// they are both in the same file

                            set.Blocks.Add(MakeBlock(pair, set, project_file));
                            set.Blocks.Add(MakeBlock(pair1, set, project_file));

                            return new List<ISet> ( new ISet[] {set} );
                        }
                    }
                }
            }
            return new List<ISet>();
        }

        private Block MakeBlock(BlockPair pair, Set set, IProjectFile project_file)
        {
            int startLineNumber = pair.Top.EndLineNumber + 1;
            int endLineNumber = pair.Bottom.StartLineNumber - 1;

            DocumentRange range = SimianResultsParser.GetDocumentRange(project_file, startLineNumber, endLineNumber, Solution);

            return new Block(startLineNumber, endLineNumber, project_file, set, range);
        }

        private Block FindClosestBlock(IEnumerable<Set> remaining_sets, Block top_block)
        {
            int min_distance = int.MaxValue;
            Block closest_block = null;

            foreach (Set set in remaining_sets)
            {
                foreach (Block block in set.Blocks)
                {
                    // To be consecutive, they must be in the same file.
                    if (top_block.SharesSourceFile(block))
                    {
                        // And top block must appear first in the file.
                        if (top_block.EndLineNumber < block.StartLineNumber)
                        {
                            int distance = top_block.DistanceTo(block);
                            if (distance < min_distance && block != top_block)
                            {
                                min_distance = distance;
                                closest_block = block;
                            }
                        }
                    }
                }
            }
            return closest_block;
        }
    }
}