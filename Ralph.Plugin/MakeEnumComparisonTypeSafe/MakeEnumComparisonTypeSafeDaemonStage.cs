using System;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;

namespace AgentRalph.MakeEnumComparisonTypeSafe
{
    /// <summary>
    /// This class is automatically loaded by ReSharper daemon 
    /// because it's marked with the attribute.
    /// </summary>
    [DaemonStage]
    public class MakeEnumComparisonTypeSafeDaemonStage : IDaemonStage
    {
        public IDaemonStageProcess CreateProcess(IDaemonProcess process, DaemonProcessKind processKind)
        {
            if (process == null)
                throw new ArgumentNullException("process");

            return new MakeEnumComparisonTypeSafeDaemonStageProcess(process);
        }

        public ErrorStripeRequest NeedsErrorStripe(IProjectFile projectFile)
        {
            // We want to add markers to the right-side stripe as well as contribute to document errors
            return ErrorStripeRequest.STRIPE_AND_ERRORS;
        }
    }
}