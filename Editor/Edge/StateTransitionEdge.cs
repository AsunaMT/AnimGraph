using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimGraph.Editor
{
    public sealed class StateTransitionEdge : GraphElement, IInspectable, IEdgePointProvider
    {
        public StateTransitionEdgeDirections EdgeDirections => EdgeControl.EdgeDirections;

        public bool IsEntryEdge { get; internal set; }

        internal Node_StateMachine stateMachine_ { get; }

        public int ConnectionCount
        {
            get
            {
                var count = 0;
                if (ConnectedNode0 != null) count++;
                if (ConnectedNode1 != null) count++;
                return count;
            }
        }

        public EditorNode_SM_NodeBase ConnectedNode0 { get; internal set; }

        public EditorNode_SM_NodeBase ConnectedNode1 { get; internal set; }

        public StateTransitionEdgeControl EdgeControl { get; }

        public WantsToEditTransition EditTransition;

        private Vector2 _dragPoint;


        public StateTransitionEdge(Node_StateMachine graphAsset, EditorNode_SM_NodeBase node0, EditorNode_SM_NodeBase node1)
        {
            stateMachine_ = graphAsset;
            ConnectedNode0 = node0;
            ConnectedNode1 = node1;
            _dragPoint = ConnectedNode0.worldBound.center;

            pickingMode = PickingMode.Position;
            style.flexGrow = 0;
            style.flexShrink = 0;
            style.position = Position.Absolute;
            style.width = 2; // For debugging
            style.height = 2; // For debugging
            AddToClassList("edge");

            var edgeStyleSheet = Resources.Load<StyleSheet>("StateTransitionEdge");
            styleSheets.Add(edgeStyleSheet);

            EdgeControl = new StateTransitionEdgeControl(this);
            Add(EdgeControl);

            if (ConnectionCount == 2)
            {
                ResetLayer();
            }

            capabilities |= Capabilities.Selectable | Capabilities.Deletable;

            // TODO FIXME: Can't receive the following callbacks, why?
            RegisterCallback<MouseEnterEvent>(OnMouseEnter);
            RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
            this.AddManipulator(new ContextualMenuManipulator(BuildContextualMenu));
        }

        public override bool ContainsPoint(Vector2 localPoint)
        {
            return EdgeControl.ContainsPoint(this.ChangeCoordinatesTo(EdgeControl, localPoint));
        }

        public override bool Overlaps(Rect rectangle)
        {
            return EdgeControl.Overlaps(this.ChangeCoordinatesTo(EdgeControl, rectangle));
        }

        public override void OnSelected()
        {
            EdgeControl.Highlight = true;
        }

        public override void OnUnselected()
        {
            EdgeControl.Highlight = false;
        }

        public void AddDirection(StateTransitionEdgeDirections direction)
        {
            EdgeControl.AddDirection(direction);
        }

        public void RemoveDirection(StateTransitionEdgeDirections direction)
        {
            EdgeControl.RemoveDirection(direction);
        }

        public void Drag(Vector2 mousePosition)
        {
            _dragPoint = mousePosition;
        }

        public void SetConnection(int index, EditorNode_SM_NodeBase node)
        {
            switch (index)
            {
                case 0:
                    ConnectedNode0 = node;
                    break;

                case 1:
                    ConnectedNode1 = node;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (ConnectionCount == 2)
            {
                ResetLayer();
            }
        }

        public bool IsConnection(EditorNode_SM_NodeBase a, EditorNode_SM_NodeBase b)
        {
            return (ConnectedNode0 == a && ConnectedNode1 == b) || (ConnectedNode0 == b && ConnectedNode1 == a);
        }

        public bool TryGetConnectedNode(EditorNode_SM_NodeBase node, out EditorNode_SM_NodeBase connectedNode)
        {
            if (node == ConnectedNode0)
            {
                connectedNode = ConnectedNode1;
            }
            else if (node == ConnectedNode1)
            {
                connectedNode = ConnectedNode0;
            }
            else
            {
                connectedNode = null;
            }

            return connectedNode != null;
        }


        Vector2 IEdgePointProvider.GetEdgePoint0()
        {
            var worldPoint = ConnectedNode0?.worldBound.center ?? default;
            var edgeControlSpacePoint = EdgeControl.WorldToLocal(worldPoint);
            return edgeControlSpacePoint;
        }

        Vector2 IEdgePointProvider.GetEdgePoint1()
        {
            var worldPoint = ConnectedNode1?.worldBound.center ?? _dragPoint;
            var edgeControlSpacePoint = EdgeControl.WorldToLocal(worldPoint);
            return edgeControlSpacePoint;
        }


        private void OnMouseEnter(MouseEnterEvent evt)
        {
            if (!enabledInHierarchy || selected) return;

            EdgeControl.Highlight = true;
        }

        private void OnMouseLeave(MouseLeaveEvent evt)
        {
            if (!enabledInHierarchy || selected) return;

            EdgeControl.Highlight = false;
        }

        private void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            Debug.LogError("TODO: Edge BuildContextualMenu");
        }


        #region Inspector

        public InspectorBase GetInspector()
        {
            var inspector = new Inspector_Transition(
                AddConditionElement, RemoveConditionElement, EditTransition, DeleteTransition);
            inspector.SetTarget(this);

            return inspector;
        }


        // TODO: Can be deleted
        private void AddConditionElement(int index)
        {
        }

        // TODO: Can be deleted
        private void RemoveConditionElement(int index)
        {
        }

/*        private void IndicateTransition(EditorNode_SM_NodeBase fromNode, EditorNode_SM_NodeBase destNode)
        {
            if (fromNode == ConnectedNode0 && destNode == ConnectedNode1)
            {
                // 0 -> 1
                EdgeControl.Indicate(StateTransitionEdgeDirections.Dir_0_1);
                return;
            }

            if (fromNode == ConnectedNode1 && destNode == ConnectedNode0)
            {
                // 1 -> 0
                EdgeControl.Indicate(StateTransitionEdgeDirections.Dir_1_0);
                return;
            }

            throw new ArgumentException("Node not connected.");
        }*/

        private void DeleteTransition(EditorNode_SM_NodeBase fromNode, EditorNode_SM_NodeBase destNode)
        {
            var edge = fromNode.RemoveTransition((EditorNode_SM_State)destNode);
            if (!edge.IsConnection(fromNode, destNode))
            {
                var graph = GetFirstAncestorOfType<StateMachineGraphView>();
                graph.RemoveElement(edge);
            }
        }

        #endregion
    }
}
