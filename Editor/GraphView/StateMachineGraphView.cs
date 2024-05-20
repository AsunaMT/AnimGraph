using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Xml;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimGraph.Editor
{
    public class StateMachineGraphView : GraphViewBase
    {
        public override EditorNodeBase RootNode => StateMachineEntryNode;

        public EditorNode_SM_Entry StateMachineEntryNode { get; }

        public Node_StateMachine stateMachine_ => (Node_StateMachine)GraphAsset;

        public Dictionary<State, EditorNode_SM_State> runtime2editorState_ = new Dictionary<State, EditorNode_SM_State>();

        public StateMachineGraphView(Node_StateMachine graphAsset, VisualElement container)
            : base(graphAsset, container)
        {
            stateMachine_.InitTable();
            // Root node
            StateMachineEntryNode = new EditorNode_SM_Entry(this);
            AddElement(StateMachineEntryNode);

            foreach(var state in graphAsset.sm_.states)
            {
                var editorState = new EditorNode_SM_State(state, this);
                editorState.OnDoubleClicked += OnDoubleClickNode;
                AddElement(editorState);
                runtime2editorState_.Add(state, editorState);
            }

            if (graphAsset.sm_.entry != -1)
            {
                var entryState = runtime2editorState_[graphAsset.entryState];
                var edge = StateMachineEntryNode.ViewOnlyConnect(entryState);
                edge.IsEntryEdge = true;
                AddElement(edge);
            }

            foreach (var state in graphAsset.sm_.states)
            {
                foreach (var transition in state.exitTransitions)
                {
                    var destNode = runtime2editorState_[graphAsset.GetState(transition.nextState)];
                    var edge = runtime2editorState_[state].ViewOnlyConnect(destNode);
                    edge.IsEntryEdge = false;
                    AddElement(edge);
                }
            }
        }

        public List<EditorNode_SM_State> GetCompatibleNodes(EditorNode_SM_NodeBase fromNode)
        {
            var nodeList = new List<EditorNode_SM_State>();
            foreach (var node in nodes)
            {
                if (node == fromNode || node is EditorNode_SM_Entry)
                {
                    continue;
                }

                nodeList.Add((EditorNode_SM_State)node);
            }

            return nodeList;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);

            if (selection.Count == 0)
            {
                // Build menu items
                var localMousePos = contentViewContainer.WorldToLocal(evt.mousePosition);
                    evt.menu.AppendAction("Create State",
                        _ => TryCreateState(localMousePos));
                evt.menu.AppendSeparator();
            }
            
            void TryCreateState(Vector2 localMousePosition)
            {
                State state = stateMachine_.CreateState();
                state.position = localMousePosition;
                state.SetName(string.Format("New State({0})", state.id));
                var editorState = new EditorNode_SM_State(state, this);
                editorState.OnDoubleClicked += OnDoubleClickNode;
                AddElement(editorState);
            }
        }


        public void AddStateTransitionEdge(StateTransitionEdge edge, bool forceRaiseGraphViewChangedEvent)
        {
            if (!Contains(edge))
            {
                AddElement(edge);
                //RaiseContentChangedEvent(DataCategories.TransitionData);
            }
            else if (forceRaiseGraphViewChangedEvent)
            {
                //RaiseContentChangedEvent(DataCategories.TransitionData);
            }
        }




        private void OnDoubleClickNode(EditorNode_SM_NodeBase graphNode)
        {
            if (graphNode is EditorNode_SM_Entry) return;

/*            var graphGuid = ((EditorNode_SM_State)graphNode).MixerGraphGuid;
            RaiseWantsToOpenGraphEvent(graphGuid);*/
        }

        public override void AddConnection(EditorNodeBase source, EditorNodeBase target, int index)
        {
            throw new NotImplementedException();
        }

        public override void RemoveConnection(int targetId, int index)
        {
            throw new NotImplementedException();
        }
    }
}
