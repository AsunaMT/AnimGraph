using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimGraph.Editor
{
    public class Inspector_State : Inspector_Node
    {
        Node_StateMachine stateMachine;
        private EditorNode_SM_State State => (EditorNode_SM_State)target_;
        readonly TextField nameFiled_;

        public Inspector_State(Node_StateMachine machine_)
        {
            stateMachine = machine_;
            // {
            nameFiled_ = new TextField("name");
            nameFiled_.labelElement.style.minWidth = StyleKeyword.Auto;
            nameFiled_.labelElement.style.maxWidth = StyleKeyword.Auto;
            nameFiled_.labelElement.style.width = FieldLabelWidth;
            nameFiled_.RegisterValueChangedCallback(OnNameChange);
            Add(nameFiled_);
        }

        private void OnNameChange(ChangeEvent<string> evt)
        {
            if (State == null)
            {
                return;
            }
            string newName = evt.newValue;
            if (!string.IsNullOrEmpty(newName))
            {
                foreach(var state in stateMachine.sm_.states)
                {
                    foreach(var transition in state.exitTransitions)
                    {
                        if (transition.nextState == State.state_.id)
                        {
                            string old = transition.entity.name_[..(transition.entity.name_.IndexOf('>') + 1)];
                            transition.entity.name_ = old + newName;
                        } 
                        else if (transition.previousState == State.state_.id)
                        {
                            string old = transition.entity.name_[transition.entity.name_.IndexOf('-')..];
                            transition.entity.name_ = newName + old;
                        }
                    }
                }
                State.state_.SetName(newName);
                State.title = newName;
            }
        }

        public override void SetTarget(IInspectable target)
        {
            base.SetTarget(target);
            nameFiled_.SetValueWithoutNotify(State.state_.name);
        }
    }
}
