using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;

namespace AnimGraph.Editor
{
    public sealed class EditorNode_AnimOutput : EditorNodeBase
    {
        public Node_SubGraph graph_;

        public GraphPort PoseInputPort { get; }


        public EditorNode_AnimOutput(Node_SubGraph graph) : base()
        {
            graph_ = graph;
            title = "Pose Output";

            // Capabilities
            capabilities &= ~Capabilities.Deletable;
            capabilities &= ~Capabilities.Copiable;

            // Input port
            PoseInputPort = InstantiatePort(Direction.Input, typeof(Playable));
            PoseInputPort.portName = "Input";
            PoseInputPort.portColor = ColorTool.GetColor(PinType.EAnim);
            PoseInputPort.OnConnected += OnPortConnected;
            PoseInputPort.OnDisconnected += OnPortDisconnected;
            inputContainer.Add(PoseInputPort);

            SetPosition(new Rect(graph_.outPosition_, Vector2.zero));

            RefreshPorts();
            RefreshExpandedState();
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            graph_.outPosition_ = newPos.position;
        }

        protected override void OnPortConnected(Edge edge)
        {
            var graphEdge = (FlowingGraphEdge)edge;

            base.OnPortConnected(edge);
        }

        protected override void OnPortDisconnected(Edge edge)
        {

            base.OnPortDisconnected(edge);
        }
    }
}
