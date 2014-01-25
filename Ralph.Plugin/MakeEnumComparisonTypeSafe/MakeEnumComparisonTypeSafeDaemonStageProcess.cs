using System;
using JetBrains.Application.Progress;
using JetBrains.Decompiler.Ast;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Impl;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;


public class BadEnumProblemAnalyzer : ElementProblemAnalyzer<ITreeNode>
{
  private class bADeNUMHighligh : IHighlighting
  {
    public bADeNUMHighligh(string toolTip, string errorStripeToolTip)
    {
      ToolTip = toolTip;
      ErrorStripeToolTip = errorStripeToolTip;
      NavigationOffsetPatch = 0;
    }

    public bool IsValid()
    {
      return true;
    }

    public string ToolTip { get; private set; }
    public string ErrorStripeToolTip { get; private set; }
    public int NavigationOffsetPatch { get; private set; }
  }
  protected override void Run(ITreeNode element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
  {
    var h = new bADeNUMHighligh("Josh", "Josh strine tooltip");
    var r = element.GetHighlightingRange();
    consumer.AddHighlighting(h, r);
  }
}
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
