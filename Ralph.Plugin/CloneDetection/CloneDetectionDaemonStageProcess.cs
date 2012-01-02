using System;
using System.Collections.Generic;
using System.Linq;
using AgentRalph.CloneCandidateDetection;
using ICSharpCode.NRefactory;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Impl;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;
using JetBrains.Util.dataStructures.TypedIntrinsics;

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
                var file = manager.GetPsiFile(myDaemonProcess.SourceFile, CSharpLanguage.Instance) as ICSharpFile;
                if (file == null)
                    return;

                // GetText gives the unsaved file contents, unlike file.ProjectFile.GetReadStream().
                string codeText = file.GetText();

                MethodsOnASingleClassCloneFinder cloneFinder = new MethodsOnASingleClassCloneFinder(new ShallowExpansionFactory());

                cloneFinder.AddRefactoring(new LiteralToParameterExpansion());

                ScanResult scan_result = cloneFinder.GetCloneReplacements(codeText);
                if (scan_result != null)
                {
                    var document = myDaemonProcess.SourceFile.Document;

                    var Highlightings = new List<HighlightingInfo>();

                    foreach (var info in scan_result.Clones)
                    {
                        // We basically highlight the first line of the clone.
                        var start = document.GetLineStartOffset(((Int32<DocLine>) (info.HighlightStartLocationLine - 1))) + info.HighlightStartLocationColumn;
                        var end = start + info.HighlightLength;// Hmm, HighlightLength seems sort of arbitrary.
                        var warningHighlightRange = new DocumentRange(document, new TextRange(start, end));

                        // And this defines the chunk that gets replaced.
                        var replacedCodeRange = new DocumentRange(document,
                                                                  new TextRange(
                                                                      document.GetLineStartOffset(
                                                                          (Int32<DocLine>)
                                                                          (info.ReplacementSectionStartLine-1)),
                                                                      document.GetLineStartOffset(
                                                                          (Int32<DocLine>)
                                                                          info.ReplacementSectionEndLine)));



                        var highlight = new HighlightingInfo(warningHighlightRange, 
                                                             new CloneDetectionHighlighting(info, replacedCodeRange));
                        Highlightings.Add(highlight);
                    }

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

        public IDaemonProcess DaemonProcess
        {
            get { return myDaemonProcess; }
        }
    }
}