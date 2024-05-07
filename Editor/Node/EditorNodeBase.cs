using GBG.AnimationGraph.Node;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

namespace AnimGraph.Editor
{
    public class GraphEditorNodeClickManipulator : MouseManipulator
    {
        private readonly Action<MouseDownEvent> _onClicked;


        public GraphEditorNodeClickManipulator(Action<MouseDownEvent> onClicked)
        {
            _onClicked = onClicked;

            activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        }


        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        }


        private void OnMouseDown(MouseDownEvent e)
        {
            _onClicked(e);
        }
    }

    public abstract partial class EditorNodeBase : Node
    {
        /// <summary>
        /// By default this visual element will serve as a divider under the title container.
        /// </summary>
        public VisualElement BannerContainer { get; }

        public VisualElement IconContainer { get; }

        public GraphNodeBase node_;

        public List<GraphPort> input_;

        public GraphPort outPut_;

        protected GraphViewBase graphView_;


        // virtual node constructor
        protected EditorNodeBase()
        {

            // Hide collapse button
            base.titleButtonContainer.Clear();
            var titleLabel = titleContainer.Q<Label>(name: "title-label");
            titleLabel.style.marginRight = 6;

            // Use title button container as icon container
            IconContainer = base.titleButtonContainer;

            // Banner container
            BannerContainer = mainContainer.Q("divider");

            // Callbacks
            this.AddManipulator(new GraphEditorNodeClickManipulator(OnClicked));
        }

        protected EditorNodeBase(GraphNodeBase node, GraphViewBase grapView)
        {
            graphView_ = grapView;
            base.titleButtonContainer.Clear();
            var titleLabel = titleContainer.Q<Label>(name: "title-label");
            titleLabel.style.marginRight = 6;

            IconContainer = base.titleButtonContainer;

            BannerContainer = mainContainer.Q("divider");

            this.AddManipulator(new GraphEditorNodeClickManipulator(OnClicked));

            if (node == null) return;
            node_ = node;
            string name = node.GetType().Name;
            title = name.Substring(name.IndexOf('_') + 1);
            //init port
            for (int i = 0; i < node_.input_.Count; i++)
            {
                var inputPort = InstantiatePort(Direction.Input, typeof(Playable));
                inputPort.portName = node_.input_[i].name;
                inputPort.portColor = ColorTool.GetColor(node_.input_[i].pinTye);
                inputPort.OnConnected += OnPortConnected;
                inputPort.OnDisconnected += OnPortDisconnected;
                inputContainer.Add(inputPort);
            }
            outPut_ = InstantiatePort(Direction.Output, typeof(Playable));
            PinType outType;
            if (node.isAnim_)
            {
                outType = PinType.EAnim;
            }
            else
            {
                outType = ((DataNodeBase)node).valType_;
            }
            outPut_.portColor = ColorTool.GetColor(outType);
            outPut_.portName = "Output";
            outputContainer.Add(outPut_);
            SetPosition(new Rect(node.position_, Vector2.zero));

            RefreshPorts();
            RefreshExpandedState();
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            if(node_ != null)
            {
                node_.position_ = newPos.position;
            }
        }


        public override Port InstantiatePort(Orientation orientation,
            Direction direction, Port.Capacity capacity, Type type)
        {
            var port = GraphPort.Create<FlowingGraphEdge>(direction, type);
/*            port.OnConnected += OnPortConnected;
            port.OnDisconnected += OnPortDisconnected;
*/
            return port;
        }

        protected GraphPort InstantiatePort(Direction direction, Type type)
        {
            return (GraphPort)InstantiatePort(Orientation.Horizontal, direction, Port.Capacity.Single, type);
        }

        protected virtual void OnPortConnected(Edge edge)
        {
            var node = (EditorNodeBase)edge.output.node;
            int index = inputContainer.IndexOf(edge.input);
            if (node != null)
            {
                graphView_.AddConnection(node, this, index);
            }
        }

        protected virtual void OnPortDisconnected(Edge edge)
        {
            int index = inputContainer.IndexOf(edge.input);
            graphView_.RemoveConnection(node_.id_, index);
        }
        /*
                #region Inspector

                public virtual IInspector<GraphEditorNode> GetInspector()
                {
                    var defaultInspector = new GraphElementInspector<GraphEditorNode>();
                    defaultInspector.SetTarget(this);

                    return defaultInspector;
                }

                #endregion*/


        #region Events

        public event Action OnNodeChanged;

        /// <summary>
        /// Don't raise events which is contained in GraphView.graphViewChanged callback.
        /// </summary>
        protected void RaiseNodeChangedEvent()
        {
            OnNodeChanged?.Invoke();
        }


        public event Action<EditorNodeBase> OnDoubleClicked;

        private void OnClicked(MouseDownEvent evt)
        {
            if (evt.clickCount == 2)
            {
                OnDoubleClicked?.Invoke(this);
            }
        }

        #endregion
    }
}
