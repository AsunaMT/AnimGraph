using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace AnimGraph.Editor
{
    public class SubGraphView : GraphViewBase
    {
        public override EditorNodeBase RootNode => PoseOutputNode;

        public EditorNode_AnimOutput PoseOutputNode { get; }

        public SubGraphView(Node_SubGraph graphAsset, VisualElement container)
            : base(graphAsset, container)
        {
            // Root node
            PoseOutputNode = new EditorNode_AnimOutput(graphAsset);
            AddElement(PoseOutputNode);

            // Nodes
/*            var nodeTable = new Dictionary<string, MixerGraphEditorNode>(GraphLayer.Nodes.Count + 1);
            foreach (var nodeData in GraphLayer.Nodes)
            {
                var node = PlayableEditorNodeFactory.CreateNode(GraphAsset, nodeData, false);
                node.OnDoubleClicked += OnDoubleClickNode;
                AddElement(node);
                nodeTable.Add(node.Guid, node);
            }

            // Edges
            if (!string.IsNullOrEmpty(PoseOutputNode.RootPlayableNodeGuid))
            {
                var rootPlayableNode = nodeTable[PoseOutputNode.RootPlayableNodeGuid];
                var edge = PoseOutputNode.PoseInputPort.ConnectTo<FlowingGraphEdge>(rootPlayableNode.OutputPort);
                AddElement(edge);
            }*/


        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            foreach (var port in ports)
            {
                if (port.node != startPort.node &&
                    port.direction != startPort.direction &&
                    port.portType == startPort.portType)
                {
                    compatiblePorts.Add(port);
                }
            }

            return compatiblePorts;
        }
    }
}
