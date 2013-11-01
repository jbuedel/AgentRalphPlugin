using System;
using System.Collections.Generic;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AgentRalph.CloneDetection
{
	/// <summary>
	/// This class is automatically loaded by ReSharper daemon 
	/// because it's marked with the attribute.
	/// </summary>
	[DaemonStage]
    public class CloneDetectionDaemonStage : CSharpDaemonStageBase 
	{
        /// <summary>
        /// This method provides a <see cref="IDaemonStageProcess"/> instance which is assigned to highlighting a single document.
        /// </summary>
	    protected override IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind, ICSharpFile file)
	    {
			if (process == null)
				throw new ArgumentNullException("process");

			return new CloneDetectionDaemonStageProcess(process);
		}
	}
}