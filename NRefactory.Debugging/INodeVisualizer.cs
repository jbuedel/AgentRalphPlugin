using System.Diagnostics;
using System.Windows.Forms;
using AgentRalph;
using ICSharpCode.NRefactory.Ast;
using Microsoft.VisualStudio.DebuggerVisualizers;
using NRefactory.Debugging;
using NRefactoryDemo;

[assembly: DebuggerVisualizer(typeof (INodeVisualizer), typeof (VisualizerObjectSource), Target = typeof (INode), Description = "NRefactory Ast Visualizer")]

namespace NRefactory.Debugging
{
    public class INodeVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            var owner = objectProvider.GetObject();
            var node = owner as AbstractNode;
            if (node != null)
            {
                AstView v = new AstView {Unit = node, Dock = DockStyle.Fill};
                windowService.ShowDialog(v);
            }
        }

        // Thanks to the magic of TestDriven.Net, I need not make a full on unit test or test exe.
// ReSharper disable UnusedMember.Global
        public static void TestShowVisualizer()
// ReSharper restore UnusedMember.Global
        {
            const string codeText = @"public class Foo {void Target() { Console.WriteLine(7); } }";
            var target = AstMatchHelper.ParseToCompilationUnit(codeText);

            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(target, typeof (INodeVisualizer));
            visualizerHost.ShowVisualizer();
        }
    }
}