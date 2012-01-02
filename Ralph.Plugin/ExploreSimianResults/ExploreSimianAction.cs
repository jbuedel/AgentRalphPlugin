using System;
using System.Collections.Generic;
using System.IO;
using AgentRalph.ExploreSimianResults;
using AgentRalph.Options;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;
using JetBrains.IDE.TreeBrowser;
using JetBrains.ProjectModel;
using JetBrains.Util;
using DataConstants = JetBrains.IDE.DataConstants;
using MessageBox=System.Windows.Forms.MessageBox;

// TODO: Add a description of what my refactoring candidates view does.
// TODO: Results windows has the title 'Type Hierarchy'
// TODO: Progress bar while analyzing, or run as a daemon?  What are the pros and cons?
// TODO: run it in a background thread, if possible.  Maybe it can build up the tree as it goes?  Don't let multiple instances run.
// TODO: Highlight the sections somehow.  Bold, or with a line around it.
// TODO: Simian Options should be per solution.  Simian path should be per user.
// TODO show a preview of some sort, when a block is selected.
// TODO: Make the options appear in two columns instead of one, to avoid scrolling.
// TODO option to ignore common file headers.
// TODO: show long thumbnail view with regular text in black lines and Blocks marked in red squares - assign a color to each Set so all it's Blocks can be easily identified.  Assign red to blocks that if removed would make bigger blocks.  The surrounding blocks should share a color to show they go together, even though they are not actually the same.
// todo by default analyze everything
// todo allow specific individual files to be excluded via a right click (per solution setting) 
// TODO: Allow specific individual files to be included via a right click - even if they are not in the solution. (per solution setting).
// todo add a "reset to defaults" button

namespace AgentRalph
{
    [ActionHandler("AgentRalph.ExploreSimianAction")]
    public class ExploreSimianAction : IActionHandler
    {
        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            // fetch active solution from context
            ISolution solution = context.GetData(IDE.DataConstants.SOLUTION);

            // enable this action if there is an active solution, disable otherwise
            return solution != null;
        }

        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            if (!File.Exists(SimianOptions.Instance.PathToSimian))
            {
                MessageBox.Show("You need to configure the path to the Simian executable.  Configuration is located at Resharper->Options->Agent Ralph->Simian Explorer.");
                // TODO: Launch the R# config here.
                return;
            }

            // Get solution from context in which action is executed
            ISolution solution = context.GetData(IDE.DataConstants.SOLUTION);
            if (solution == null)
                return;

            List<FileSystemPath> files_to_analyze = CollectAllFilesToAnalyze(solution);

            if(files_to_analyze.Count == 0)
            {
                SimianResultsDescriptor d2 = new SimianResultsDescriptor(solution, new List<ISet>(), "Simian Explorer");

                // Show the results in the gui.
                TreeModelBrowser.GetInstance(solution).Show("SearchResultsWindow", d2,
                                                            new TreeModelBrowserPanel(d2));
                return;
            }

            string simian_xml_file = SimianUtil.RunSimianToXml(files_to_analyze, SimianOptions.Instance);
            try
            {
                SimianResultsParser parser = new SimianResultsParser(solution);
                IEnumerable<Set> sim_sets = parser.GetSimianResults2(simian_xml_file);

                // Get the initial explorer.
                SimianResultsDescriptor d2 = new SimianResultsDescriptor(solution, ToIListOfISet(sim_sets), "Simian Explorer");

                // Show the results in the gui.
                TreeModelBrowser.GetInstance(solution).Show("SearchResultsWindow", d2,
                                                            new TreeModelBrowserPanel(d2));

                // And the examiner produces the content of the 'Refactoring Candidates' explorer.
                Examiner examiner = new Examiner(solution);
                IList<ISet> l = examiner.Examine(sim_sets);

                if(l.Count == 0)
                    return;

                d2 = new SimianResultsDescriptor(solution, l, "The Inbetweens");
                
                // Show the results in the gui.
                TreeModelBrowser.GetInstance(solution).Show("SearchResultsWindow", d2,
                                                            new TreeModelBrowserPanel(d2));


                if (parser.FilesInError.Count > 0)
                {
                    using (SimianErrorFilesForm f = new SimianErrorFilesForm())
                    {
                        f.FilesInError = parser.FilesInError;
                        f.ShowDialog();
                    }
                }
            }
            finally
            {
                if (File.Exists(simian_xml_file))
                    File.Delete(simian_xml_file);
            }
        }

        private List<FileSystemPath> CollectAllFilesToAnalyze(ISolution solution)
        {
            List<FileSystemPath> files_to_analyze = new List<FileSystemPath>();
            SimianProjectVisitor visitor = new SimianProjectVisitor(files_to_analyze);

            visitor.AddExcludeSpecs(SimianOptions.Instance.SpecsToExclude);
            visitor.AddIncludeSpecs(SimianOptions.Instance.SpecsToInclude);

            foreach (IProject project in solution.GetAllProjects())
            {
                project.Accept(visitor);
            }
            return files_to_analyze;
        }

        private IList<ISet> ToIListOfISet(IEnumerable<Set> sim_sets)
        {
            List<Set> set_list = new List<Set>(sim_sets);
            Converter<Set, ISet> converter = delegate (Set input) { return input; };
            return set_list.ConvertAll(converter);
        }

    }
}