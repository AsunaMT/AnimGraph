using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

namespace AnimGraph.Editor
{
    public abstract class EditorNode_SM_NodeBase : EditorNodeBase
    {
        public abstract string StateName { get; internal set; }

        internal abstract List<Transition> Transitions { get; }

        protected const float CONNECTION_HANDLER_WIDTH = 12;

        public State state_;
        protected StateMachineGraphView smGraphView => (StateMachineGraphView)graphView_;
        protected Node_StateMachine machine_ => (Node_StateMachine)graphView_?.GraphAsset;
        protected EditorNode_SM_NodeBase(State state, StateMachineGraphView graphView) : base()
        {
            state_ = state;
            graphView_ = graphView;
            // Styles
            mainContainer.style.backgroundColor = new Color(45 / 255f, 45 / 255f, 45 / 255f, 1.0f);
            mainContainer.style.justifyContent = Justify.Center;
            mainContainer.style.borderTopWidth = CONNECTION_HANDLER_WIDTH;
            mainContainer.style.borderBottomWidth = CONNECTION_HANDLER_WIDTH;
            mainContainer.style.borderLeftWidth = CONNECTION_HANDLER_WIDTH;
            mainContainer.style.borderRightWidth = CONNECTION_HANDLER_WIDTH;
            var titleLabel = titleContainer.Q<Label>(name: "title-label");
            titleLabel.style.maxWidth = 150;
            titleLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            titleLabel.style.whiteSpace = WhiteSpace.Normal;

            // Callbacks
            mainContainer.AddManipulator(new StateTransitionEdgeConnector(graphView.stateMachine_));
        }

/*        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
        }*/


        #region Transitions // TODO OPTIMIZABLE

        internal List<StateTransitionEdge> OutputTransitions { get; } = new List<StateTransitionEdge>();


        private readonly List<StateTransitionEdge> _inputTransitions = new List<StateTransitionEdge>();


        public virtual StateTransitionEdge AddTransition(EditorNode_SM_State destNode, out bool dataDirty)
        {
            var edge = ViewOnlyConnect(destNode);

            // Add transition data
            var transitionData = Transitions.FirstOrDefault(t => t.nextState.Equals(destNode.state_.id));
            if (transitionData == null)
            {
                state_.AddTransition(destNode.state_);
                dataDirty = true;
            }
            else
            {
                dataDirty = false;
            }

            return edge;
        }

        public StateTransitionEdge RemoveTransition(EditorNode_SM_State destNode)
        {
            var removedEdge = ViewOnlyDisconnect(destNode);
            if (removedEdge != null)
            {
                // Remove transition data
                state_.RemoveTransition(destNode.state_.id);
            }

            return removedEdge;
        }

        public bool RemoveTransition(StateTransitionEdge transitionEdge)
        {
            if (OutputTransitions.Remove(transitionEdge))
            {
                if (transitionEdge.TryGetConnectedNode(this, out var destNode))
                {
                    destNode._inputTransitions.Remove(transitionEdge);
                }

                // Remove transition data

                state_.RemoveTransition(destNode.state_.id);

                return true;
            }

            return false;
        }

        public StateTransitionEdge ViewOnlyConnect(EditorNode_SM_State destNode)
        {
            var edge = OutputTransitions.FirstOrDefault(e => e.IsConnection(this, destNode));

            // Transition already exists
            if (edge != null)
            {
                return edge;
            }

            // Reversed transition already exists
            edge = destNode.OutputTransitions.FirstOrDefault(e => e.IsConnection(this, destNode));
            if (edge != null)
            {
                edge.AddDirection(StateTransitionEdgeDirections.Bidirectional);
                OutputTransitions.Add(edge);
                destNode._inputTransitions.Add(edge);
                return edge;
            }

            // New transition
            edge = new StateTransitionEdge(smGraphView.stateMachine_, this, destNode);
            edge.EditTransition = smGraphView.EditTransition;
            edge.AddDirection(StateTransitionEdgeDirections.Dir_0_1);
            edge.IsEntryEdge = false;

            OutputTransitions.Add(edge);
            destNode._inputTransitions.Add(edge);

            return edge;
        }

        public StateTransitionEdge ViewOnlyDisconnect(EditorNode_SM_State destNode)
        {
            StateTransitionEdge removedEdge = null;
            for (int i = 0; i < OutputTransitions.Count; i++)
            {
                var edge = OutputTransitions[i];
                if (edge.IsConnection(this, destNode))
                {
                    OutputTransitions.RemoveAt(i);
                    removedEdge = edge;
                    break;
                }
            }

            if (removedEdge != null)
            {
                destNode._inputTransitions.Remove(removedEdge);

                if (destNode.OutputTransitions.Contains(removedEdge))
                {
                    if (removedEdge.ConnectedNode1 == destNode)
                    {
                        removedEdge.RemoveDirection(StateTransitionEdgeDirections.Dir_0_1);
                    }
                    else
                    {
                        removedEdge.RemoveDirection(StateTransitionEdgeDirections.Dir_1_0);
                    }
                }
                else
                {
                    removedEdge.SetConnection(0, null);
                    removedEdge.SetConnection(1, null);
                    removedEdge.RemoveDirection(StateTransitionEdgeDirections.Bidirectional);
                }
            }

            return removedEdge;
        }

        public List<StateTransitionEdge> ViewOnlyDisconnectAll()
        {
            var edgesToRemove = new List<StateTransitionEdge>(OutputTransitions.Count + _inputTransitions.Count);
            for (int i = 0; i < OutputTransitions.Count; i++)
            {
                var edge = OutputTransitions[i];
                edge.RemoveDirection(StateTransitionEdgeDirections.Bidirectional);
                edgesToRemove.Add(edge);
                if (edge.TryGetConnectedNode(this, out var destNode))
                {
                    destNode._inputTransitions.Remove(edge);
                }
            }

            OutputTransitions.Clear();

            for (int i = 0; i < _inputTransitions.Count; i++)
            {
                var edge = _inputTransitions[i];
                edge.RemoveDirection(StateTransitionEdgeDirections.Bidirectional);
                if (!edgesToRemove.Contains(edge))
                {
                    edgesToRemove.Add(edge);
                }

                if (edge.TryGetConnectedNode(this, out var fromNode))
                {
                    fromNode.OutputTransitions.Remove(edge);
                }
            }

            _inputTransitions.Clear();

            return edgesToRemove;
        }

        #endregion
    }
}