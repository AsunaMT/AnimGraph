using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace AnimGraph.Editor
{
    public class EditorNode_SM_Entry : EditorNode_SM_NodeBase
    {
        public override string StateName { get { return "Entry"; } internal set{ } }

        internal override List<Transition> Transitions { get { return null;  } }


        public EditorNode_SM_Entry(StateMachineGraphView graphView)
            : base(null, graphView)
        {
            title = "State Machine Entry";
            titleContainer.style.backgroundColor = Color.green;

            // Capabilities
            capabilities &= ~Capabilities.Deletable;
            capabilities &= ~Capabilities.Copiable;

            SetPosition(new Rect(machine_.sm_.entryPosition, Vector2.zero));

            RefreshPorts();
            RefreshExpandedState();
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            machine_.sm_.entryPosition = newPos.position;
        }

/*        public override IInspector<GraphEditorNode> GetInspector()
        {
            var inspector = new StateMachineEntryNodeInspector();
            inspector.SetTarget(this);

            return inspector;
        }*/

        public override StateTransitionEdge AddTransition(EditorNode_SM_State destNode, out bool dataDirty)
        {
            // Only allow one transition
            if (OutputTransitions.Count > 0)
            {
                if (OutputTransitions[0].IsConnection(this, destNode))
                {
                    dataDirty = false;
                    return OutputTransitions[0];
                }

                var edgesToRemove = ViewOnlyDisconnectAll();
                graphView_.DeleteElements(edgesToRemove);
            }

            var edge = ViewOnlyConnect(destNode);
            edge.IsEntryEdge = true;

            // Transition data
            machine_.sm_.entry = destNode.state_.id;
            dataDirty = true;

            return edge;
        }

    }
}