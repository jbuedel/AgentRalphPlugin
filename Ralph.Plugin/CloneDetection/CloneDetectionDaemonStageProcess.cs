using System;
using System.Linq;
using AgentRalph.CloneCandidateDetection;
using ICSharpCode.NRefactory;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AgentRalph.CloneDetection
{
    internal class CloneDetectionDaemonStageProcess : IDaemonStageProcess
    {
        private readonly IDaemonProcess myDaemonProcess;

        public CloneDetectionDaemonStageProcess(IDaemonProcess process)
        {
            myDaemonProcess = process;
        }

        public void Execute(Action<DaemonStageResult> commiter)
        {
            try
            {
                PsiManager manager = PsiManager.GetInstance(myDaemonProcess.Solution);
                ICSharpFile file = manager.GetPsiFile(myDaemonProcess.ProjectFile, CSharpLanguageService.CSHARP) as ICSharpFile;
                if (file == null)
                    return;

                // GetText gives the unsaved file contents, unlike file.ProjectFile.GetReadStream().
                string codeText = file.GetText();

                MethodsOnASingleClassCloneFinder cloneFinder = new MethodsOnASingleClassCloneFinder(new ShallowExpansionFactory());

                cloneFinder.AddRefactoring(new LiteralToParameterExpansion());

                ScanResult scan_result = cloneFinder.GetCloneReplacements(codeText);
                if (scan_result != null)
                {
                    var Highlightings = (from clone in scan_result.Clones
                                         let docRange =
                                             SimianResultsParser.GetDocumentRange(myDaemonProcess.ProjectFile,
                                                                                  new Location(clone.HighlightStartLocationColumn, clone.HighlightStartLocationLine),
                                                                                  clone.HighlightLength, myDaemonProcess.Solution)

                                         let bodyrange =
                                             SimianResultsParser.GetDocumentRange(myDaemonProcess.ProjectFile,
                                                                                  clone.ReplacementSectionStartLine,
                                                                                  clone.ReplacementSectionEndLine,
                                                                                  myDaemonProcess.Solution)
                                         select
                                             new HighlightingInfo(docRange,
                                                                  new CloneDetectionHighlighting(clone,
                                                                                                 bodyrange))).ToArray();

                    // Creating container to put highlightings into.
                    DaemonStageResult ret = new DaemonStageResult(Highlightings);

                    commiter(ret);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e);
                throw;
            }
        }
    }
}