using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AnimGraph.Editor
{
    public class EditorNode_SM_State : EditorNode_SM_NodeBase
    {
        internal override List<Transition> Transitions => state_.exitTransitions;

        public override string StateName
        {
            get => state_.name;
            internal set
            {
                state_.name = value;
                title = value;
            }
        }


        public EditorNode_SM_State(State state, StateMachineGraphView graphView) : base(state, graphView)
        {
            node_ = state.entity;
            title = state.name;
            SetPosition(new Rect(state.position, Vector2.zero));

            RefreshPorts();
            RefreshExpandedState();
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            state_.position = newPos.position;
        }

        public override InspectorBase GetInspector()
        {
            var inspector = new Inspector_State(machine_);
            inspector.SetTarget(this);

            return inspector;
        }
    }
}