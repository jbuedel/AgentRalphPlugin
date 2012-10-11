using System;
using System.Collections.Generic;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;

namespace AgentRalph.CloneDetection
{
	/// <summary>
	/// This class is automatically loaded by ReSharper daemon 
	/// because it's marked with the attribute.
	/// </summary>
	[DaemonStage]
    public class CloneDetectionDaemonStage : IDaemonStage // TODO: Change this to a CSharpDaemonStageBase, per http://codevanced.net/post/How-to-write-a-ReSharper-plugin.aspx
	{
		/// <summary>
		/// This method provides a <see cref="IDaemonStageProcess"/> instance which is assigned to highlighting a single document.
		/// </summary>
		public IEnumerable<IDaemonStageProcess> CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind kind)
		{
			if (process == null)
				throw new ArgumentNullException("process");

			return new[] {new CloneDetectionDaemonStageProcess(process)};
		}
	    public ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settingsStore)
	    {
            // We want to add markers to the right-side stripe as well as contribute to document errors
            return ErrorStripeRequest.STRIPE_AND_ERRORS;
        }
	}
}