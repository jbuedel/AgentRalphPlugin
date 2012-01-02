using System;
using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;

namespace AgentRalph.MakeEnumComparisonTypeSafe
{
    /// <summary>
    /// This class is automatically loaded by ReSharper daemon 
    /// because it's marked with the attribute.
    /// </summary>
    [DaemonStage]
    public class MakeEnumComparisonTypeSafeDaemonStage : IDaemonStage
    {
        public IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
        {
            if (process == null)
                throw new ArgumentNullException("process");

            return new MakeEnumComparisonTypeSafeDaemonStageProcess(process);
        }

        public ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settingsStore)
        {
            // We want to add markers to the right-side stripe as well as contribute to document errors
            return ErrorStripeRequest.STRIPE_AND_ERRORS;
        }
    }
}