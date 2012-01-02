using System.Collections.Generic;
using AgentRalph;
using AgentRalph.ExploreSimianResults;
using JetBrains.DocumentModel;
using NUnit.Framework;

// TODO Save statistics for each run, and show changes between them as a graph or somethig.
[TestFixture]
public class ConsecutiveBlockFinderTests
{
    [Test]
    public void IdentifyBlocksThatIfRemovedWouldCreateLargerSets()
    {
        List<Set> s = new List<Set>();

        // Create a situation where a single file has two redundant
        // sets, each set having two blocks.  Like so:
        /*  1 
         *  2 public void Foo()
         *  3 {
         *  4     ////////////////////////////////
         *  5     ////// Redundant set Top ///////
         *  6     ////////////////////////////////
         *  7     --> Foo extract method candidate <--
         *  8     ////////////////////////////////
         *  9     ////// Redundant set Bottom ////
         * 10     ////////////////////////////////
         * 11 }
         * 12 public void Bar()
         * 13 { 
         * 14     ////////////////////////////////
         * 15     ////// Redundant set Top ///////
         * 16     ////////////////////////////////
         * 17     --> Bar extract method candidate <--
         * 18     ////////////////////////////////
         * 19     ////// Redundant set Bottom ////
         * 20     ////////////////////////////////
         * 21 }
         * 22 
         * 
         * If the two --><-- sections were removed, the two sets would 
         * become one big set.
         */
        s.Add(new Set(3));
        s[0].AddBlock("SampleRedundantFile.cs", 4);
        s[0].AddBlock("SampleRedundantFile.cs", 14);

        s.Add(new Set(3));
        s[1].AddBlock("SampleRedundantFile.cs", 8);
        s[1].AddBlock("SampleRedundantFile.cs", 18);

        Examiner e = new Examiner(null);

        List<ISet> candidates = e.Examine(s);
        Assert.AreEqual(1, candidates.Count);
        Assert.AreEqual(7, candidates[0].Blocks[0].StartLineNumber);
        Assert.AreEqual(7, candidates[0].Blocks[0].EndLineNumber);
        Assert.AreEqual(17, candidates[0].Blocks[1].StartLineNumber);
        Assert.AreEqual(17, candidates[0].Blocks[1].EndLineNumber);
    }

    [Test]
    public void BlocksInDifferentFilesCantBeConsecutive()
    {
        List<Set> s = new List<Set>();

        // Same block defs as above, except the sets are in different files,
        // and therefore will never be consecutive.
        s.Add(new Set(3));
        s[0].AddBlock("SampleRedundantFile.cs", 4);
        s[0].AddBlock("SampleRedundantFile.cs", 14);

        s.Add(new Set(3));
        s[1].AddBlock("DifferentFile.cs", 8);
        s[1].AddBlock("DifferentFile.cs", 18);

        Examiner e = new Examiner(null);

        List<ISet> candidates = e.Examine(s);
        Assert.IsEmpty(candidates);
    }

    [Test]
    public void BlocksInSetNotInFileOrder()
    {
        List<Set> s = new List<Set>();

        // Exact same as the other test, except the blocks are not in file order.        
        s.Add(new Set(3));
        s[0].AddBlock("SampleRedundantFile.cs", 14);
        s[0].AddBlock("SampleRedundantFile.cs", 4);

        s.Add(new Set(3));
        s[1].AddBlock("SampleRedundantFile.cs", 18);
        s[1].AddBlock("SampleRedundantFile.cs", 8);

        Examiner e = new Examiner(null);

        List<ISet> candidates = e.Examine(s);
        Assert.AreEqual(17, candidates[0].Blocks[0].StartLineNumber);
        Assert.AreEqual(17, candidates[0].Blocks[0].EndLineNumber);
        Assert.AreEqual(7, candidates[0].Blocks[1].StartLineNumber);
        Assert.AreEqual(7, candidates[0].Blocks[1].EndLineNumber);
    }
    [Test]
    public void test()
    {
        Block block = new Block(15, 17,
                                null, null, new DocumentRange());
        Assert.AreEqual(166, block.TextRange.StartOffset);
    }

}