using System;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Impl;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AgentRalph.MakeEnumComparisonTypeSafe
{
    public class MakeEnumComparisonTypeSafeDaemonStageProcess: IDaemonStageProcess
    {
        private readonly IDaemonProcess myDaemonProcess;

        public MakeEnumComparisonTypeSafeDaemonStageProcess(IDaemonProcess myDaemonProcess)
        {
            this.myDaemonProcess = myDaemonProcess;
        }

        public void Execute(Action<DaemonStageResult> commiter)
        {
            PsiManager manager = PsiManager.GetInstance(myDaemonProcess.Solution);
            ICSharpFile file = manager.GetPsiFile(myDaemonProcess.SourceFile, CSharpLanguage.Instance) as ICSharpFile;
            if (file == null)
                return;

            // Running visitor against the PSI
            var elementProcessor = new MakeEnumComparisonTypeSafeFinderElementProcessor(myDaemonProcess);
            file.ProcessDescendants(elementProcessor);

            // Checking if the daemon is interrupted by user activity
            if (myDaemonProcess.InterruptFlag)
                throw new ProcessCancelledException();

            // Fill in the result
            DaemonStageResult result = new DaemonStageResult(elementProcessor.Highlightings);
            commiter(result);
        }

        public IDaemonProcess DaemonProcess
        {
            get { return DaemonProcess; }
        }
    }
}
