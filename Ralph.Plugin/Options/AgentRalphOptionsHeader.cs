using System.Windows.Forms;
using JetBrains.UI.CrossFramework;
using JetBrains.UI.Options;

namespace AgentRalph.Options
{
    [OptionsPage(ID, "Agent Ralph", null)]
    public class AgentRalphOptionsHeader : UserControl, IOptionsPage
    {
        public const string ID = "AgentRalphOptionsHeaderId";

        public bool OnOk()
        {
            return true;
        }

        public bool ValidatePage()
        {
            return true;
        }

        public EitherControl Control
        {
            get { return this; }
        }

        public string Id
        {
            get { return ID; }
        }
    }
}