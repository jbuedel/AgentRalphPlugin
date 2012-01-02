using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AgentRalph.Options;
using JetBrains.ProjectModel;

namespace AgentRalph.ExploreSimianResults
{
    public partial class SimianErrorFilesForm : Form
    {
        public SimianErrorFilesForm()
        {
            InitializeComponent();
        }

        public IList<IProjectFile> FilesInError
        {
            set
            {
                var v = value.GroupBy(f => f.Location.ExtensionWithDot).Select(exts => exts);

                foreach (var files in v)
                {
                    TreeNode node = this.treeView1.Nodes.Add(files.Key, files.Key);
                    foreach (IProjectFile file in files)
                    {
                        node.Nodes.Add(file.Location.Name, file.Location.Name);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // TODO: Get a full path here for the case of specific files.
            if (treeView1.SelectedNode != null)
            {
                string text = treeView1.SelectedNode.Text;
                if (text.StartsWith("."))
                    text = "*" + text;
                SimianOptions.Instance.AddSpecToExclude(text);
            }
        }
    }
}
