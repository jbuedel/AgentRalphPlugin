using System;
using System.Windows.Forms;
using JetBrains.UI.Options;

namespace AgentRalph.Options
{
    [OptionsPage(ID, "Simian Explorer", "AgentRalph.Options.SimianOptionPage.png", ParentId = AgentRalphOptionsHeader.ID)]
    public partial class SimianOptionsPage : UserControl, IOptionsPage
    {
        public const string ID = "SimianPageId";

        public SimianOptionsPage(IOptionsDialog optionsUI)
        {
            InitializeComponent();

            // Map the options storage object members into the GUI elements.
            SimianOptions opt = SimianOptions.Instance;

            foreach (SimianBooleanOption pair in opt.CommandLineOptions)
            {
                CheckBox box = new CheckBox {Text = pair.FriendlyText, Dock = DockStyle.Top};

                box.DataBindings.Add("Checked", pair, "Value");

                panelOptionsContainer.Controls.Add(box);
            }


            IncludesTextBox.Lines = opt.SpecsToInclude;
            ExcludesTextBox.Lines = opt.SpecsToExclude;
            SimianPathTextBox.Text = opt.PathToSimian;
        }

        #region IOptionsPage Members

        public bool OnOk()
        {
            SimianOptions opt = SimianOptions.Instance;
            // No need to map the check boxes back - they were bound and therefore updated dynamically.
            opt.SpecsToInclude = IncludesTextBox.Lines;
            opt.SpecsToExclude = ExcludesTextBox.Lines;
            opt.PathToSimian = SimianPathTextBox.Text;
            return true;
        }

        public bool ValidatePage()
        {
            return true;
        }

        public Control Control
        {
            get { return this; }
        }

        public string Id
        {
            get { return ID; }
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            using (FileDialog d = new OpenFileDialog())
            {
                if (d.ShowDialog() == DialogResult.OK && d.CheckFileExists)
                {
                    SimianPathTextBox.Text = d.FileName;
                }
            }
        }
    }
}