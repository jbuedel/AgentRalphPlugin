using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;

namespace AgentRalph
{
  [ActionHandler("AgentRalph.About")]
  public class AboutAction : IActionHandler
  {
    public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
    {
      // return true or false to enable/disable this action
      return true;
    }

    public void Execute(IDataContext context, DelegateExecute nextExecute)
    {
      MessageBox.Show(
        "AgentRalphPlugin\nJosh Buedel\n\nA code clone tool",
        "About AgentRalphPlugin",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information);
    }
  }
}
