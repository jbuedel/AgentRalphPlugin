using System.Windows.Forms;
using AgentRalph.CloneCandidateDetection;

namespace AgentRalph.Tests
{
    public partial class ProgressForm : Form
    {
        public ProgressForm()
        {
            InitializeComponent();
        }

        private delegate void foo(object sender, MethodStartEventArgs args);
        private delegate void foo2(object sender, MethodProgressEventArgs args);

        public void OnMethodStart(object sender, MethodStartEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((foo) OnMethodStart, new[] { sender, args });
                return;
            }

            this.label1.Text = string.Format("Scanning method {0} for clones.  {1}", args.Name, args.ExpectedMethodsCount);
            this.Text = string.Format("Scanning method {0} for clones.  {1}", args.Name, args.ExpectedMethodsCount);
            this.progressBar1.Maximum = args.ExpectedMethodsCount;
            this.progressBar1.Minimum = 0;
            this.progressBar1.Value = 0;
            this.progressBar1.Step = 1;
        }

        public void OnMethodProgress(object sender, MethodProgressEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((foo2)OnMethodProgress, new[] { sender, args });
                return;
            }
            this.progressBar1.PerformStep();
        }
    }
}
