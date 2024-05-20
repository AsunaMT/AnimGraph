using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;

namespace AnimGraph.Editor
{
    public sealed class EditorNode_FuncOutput : EditorNodeBase
    {
        public Node_Function func_;

        public GraphPort PoseInputPort { get; }


        public EditorNode_FuncOutput(Node_Function func) : base()
        {
            func_ = func;
            title = "Return";

            // Capabilities
            capabilities &= ~Capabilities.Deletable;
            capabilities &= ~Capabilities.Copiable;

            // Input port
            PoseInputPort = InstantiatePort(Direction.Input, func.GetOutType());
            PoseInputPort.portName = "Input";
            PoseInputPort.portColor = ColorTool.GetColor(func.valType_);
            PoseInputPort.OnConnected += OnPortConnected;
            PoseInputPort.OnDisconnected += OnPortDisconnected;
            inputContainer.Add(PoseInputPort);

            SetPosition(new Rect(func_.outPosition_, Vector2.zero));

            RefreshPorts();
            RefreshExpandedState();
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            func_.outPosition_ = newPos.position;
        }

        protected override void OnPortConnected(Edge edge)
        {
            var node = ((EditorNodeBase)edge.output.node).node_;
            if (node != null)
            {
                func_.rootId_ = node.id_;
            }
        }

        protected override void OnPortDisconnected(Edge edge)
        {
            func_.rootId_ = -1;
        }
    }
}
