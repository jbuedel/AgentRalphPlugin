using System.Collections.Generic;
using AgentRalph.ExploreSimianResults;
using JetBrains.Application;
using JetBrains.CommonControls;
using JetBrains.DocumentModel;
using JetBrains.IDE.TreeBrowser;
using JetBrains.ProjectModel;

using JetBrains.ReSharper.Features.Common.TreePsiBrowser;
using JetBrains.TreeModels;
using JetBrains.UI.PopupWindowManager;
using JetBrains.UI.TreeView;

namespace AgentRalph
{
    public class SimianResultsDescriptor : TreeModelBrowserDescriptor
    {
        private readonly TreeSectionModel myModel = new TreeSectionModel();
        private readonly TreeModelBrowserPresenter myPresenter = new TreeModelBrowserPresenter();
        private readonly string title;

        public SimianResultsDescriptor(ISolution solution, IList<ISet> sim_sets, string title) : base(solution)
        {
            this.title = title;
            // Create the list of roots, one for each identified set of matching lines.
            List<TreeSection> set_sections = new List<TreeSection>();

            if(sim_sets.Count == 0)
            {
                set_sections.Add(new TreeSection(new TreeSimpleModel(), "No duplication found!"));
            }
            else 
            {
                List<ISet> l = new List<ISet>(sim_sets);
                l.Sort(delegate (ISet s1, ISet s2)
                           {
                               if (s2.LineCount > s1.LineCount)
                                   return 1;
                               else if (s2.LineCount == s1.LineCount)
                                   return 0;
                               else return -1;
                           });

                foreach (ISet set in l)
                {
                    // Each set is composed of actual file sections containing an instance of the match.  Create
                    // a node for each of those.
                    TreeSimpleModel tsm = new TreeSimpleModel();

                    foreach (IBlock block in set.Blocks)
                    {
                        tsm.Insert(null, block);
                    }

                    set_sections.Add(new TreeSection(tsm, set.FriendlyText));
                }
            }

            myModel.Sections = set_sections;
            RequestUpdate(UpdateKind.Structure, true);
        }


        public override TreeModel Model
        {
            get { return myModel; }
        }

        public override StructuredPresenter<TreeModelNode, IPresentableItem> Presenter
        {
            get { return myPresenter; }
        }

        public override bool Navigate(TreeModelNode node, PopupWindowContextSource popupWindowContextSource, bool transferFocus)
        {
            // Let the base take a crack at navigating.
            if (base.Navigate(node, popupWindowContextSource, transferFocus))
            {
                return true;
            }
            else
            {
                if (node != null && node.DataValue is IBlock)
                {
                    IBlock block = (IBlock) node.DataValue;
                    if(block.Range != null)
                    {
                        using (ReadLockCookie.Create())
                        {
                            JetBrains.ReSharper.Feature.Services.Navigation.NavigationManager.Navigate(
                                (DocumentRange) block.Range, this.Solution, transferFocus);
                        }
                    }
                }
            }
            return false;
        }
    }
}
