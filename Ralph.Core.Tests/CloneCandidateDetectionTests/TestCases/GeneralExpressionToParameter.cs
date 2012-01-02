// Ignore - This test will need some sort of expression->parameter refactoring.
using System;

namespace AgentRalph.Tests.CloneCandidateDetectionTests.TestCases
{
    internal class GeneralExpressionToParameter
    {
        private void Foo(Node acNode)
        {
            /* BEGIN */
            acNode.Name = "Node name";
            acNode.ControlType = typeof (Control);
            acNode.OnContextHelpEvent += OnCommand;

            acNode.ClosedImageIndex = (int) Number.One;
            /* END */
        }

        private void Bar(Node pdaNode, string name, Type type, int icons)
        {
            pdaNode.Name = name;
            pdaNode.ControlType = type;
            pdaNode.OnContextHelpEvent += OnCommand;

            // Add enterprise scheduled reports node.
            pdaNode.ClosedImageIndex = icons;
        }

        private void OnCommand()
        {
        }
    }

    internal class Control
    {
    }

    internal enum Number
    {
        One, Two, Three
    }

    internal class Node
    {
        public string Name;
        public Type ControlType;
        public int ClosedImageIndex;
        public event Action OnContextHelpEvent;
    }
}