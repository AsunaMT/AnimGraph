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
    public delegate void WantsToEditTransition(Transition transition);

    public delegate void WantsToDeleteTransition(EditorNode_SM_State fromNode, EditorNode_SM_State destNode);

    public sealed class TransitionDrawer : VisualElement
    {
        private readonly Length _fieldLabelWidth;

        private readonly Foldout foldout_;

        private readonly TextField transitionId_;

        private readonly FloatField exitTime_;

        private readonly FloatField transTime_;

        private readonly EnumField interruptType_;

        private readonly CurveField blendCurve_;

        private readonly Action<int> addTransitionElement_;

        private readonly Action<int> removeTransitionElement_;

        private EditorNode_SM_State _fromNode;

        private EditorNode_SM_State _destNode;

        private Transition transition_;


        public TransitionDrawer(Length fieldLabelWidth,
            Action<int> addTransitionElement,
            Action<int> removeTransitionElement,
            WantsToEditTransition onWantsToEdit,
            WantsToDeleteTransition onWantsToDelete)
        {
            _fieldLabelWidth = fieldLabelWidth;
            addTransitionElement_ = addTransitionElement;
            removeTransitionElement_ = removeTransitionElement;

            foldout_ = new Foldout
            {
                value = true,
            };
            Add(foldout_);

            // Transition Id
            transitionId_ = new TextField("Transition Id");
            transitionId_.labelElement.style.minWidth = StyleKeyword.Auto;
            transitionId_.labelElement.style.maxWidth = StyleKeyword.Auto;
            transitionId_.labelElement.style.width = _fieldLabelWidth;
            transitionId_.SetEnabled(false);
            foldout_.contentContainer.Add(transitionId_);

            // Exit time
            exitTime_ = new FloatField("Exit Time(s)");
            exitTime_.labelElement.style.minWidth = StyleKeyword.Auto;
            exitTime_.labelElement.style.maxWidth = StyleKeyword.Auto;
            exitTime_.labelElement.style.width = _fieldLabelWidth;
            exitTime_.RegisterValueChangedCallback(OnExitTimeChanged);
            foldout_.contentContainer.Add(exitTime_);

            // Fade time
            transTime_ = new FloatField("Fade Time(s)");
            transTime_.labelElement.style.minWidth = StyleKeyword.Auto;
            transTime_.labelElement.style.maxWidth = StyleKeyword.Auto;
            transTime_.labelElement.style.width = _fieldLabelWidth;
            transTime_.RegisterValueChangedCallback(OnTransTimeChanged);
            foldout_.contentContainer.Add(transTime_);

            // Transition mode
            interruptType_ = new EnumField("Interruption Type", InterruptType.None);
            interruptType_.labelElement.style.minWidth = StyleKeyword.Auto;
            interruptType_.labelElement.style.maxWidth = StyleKeyword.Auto;
            interruptType_.labelElement.style.width = _fieldLabelWidth;
            interruptType_.RegisterValueChangedCallback(OnTransitionModeChanged);
            foldout_.contentContainer.Add(interruptType_);

            // Blend curve
            blendCurve_ = new CurveField("BlendCurve");
            blendCurve_.labelElement.style.minWidth = StyleKeyword.Auto;
            blendCurve_.labelElement.style.maxWidth = StyleKeyword.Auto;
            blendCurve_.labelElement.style.width = _fieldLabelWidth;
            blendCurve_.RegisterValueChangedCallback(OnBlendCurveChanged);
            foldout_.contentContainer.Add(blendCurve_);

            // Buttons
            var buttonContainer = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    marginBottom = 20,
                },
            };
            foldout_.Add(buttonContainer);

            // Edit button
            var editButton = new Button(() => { onWantsToEdit(transition_); })
            {
                text = "Edit",
                style =
                {
                    width = 64,
                },
            };
            buttonContainer.Add(editButton);

            // Delete button
            var deleteButton = new Button(() => { onWantsToDelete(_fromNode, _destNode); })
            {
                text = "Delete",
            };
            buttonContainer.Add(deleteButton);
        }

        public void SetTransition(EditorNode_SM_State fromNode, EditorNode_SM_State destNode, Transition transition)
        {
            _fromNode = fromNode;
            _destNode = destNode;
            transition_ = transition;

            foldout_.text = $"{fromNode.StateName} -> {destNode.StateName}";

            transitionId_.SetValueWithoutNotify(transition_.id.ToString());

            exitTime_.SetValueWithoutNotify(transition_.exitTime);

            transTime_.SetValueWithoutNotify(transition_.transTime);

            interruptType_.SetValueWithoutNotify(transition_.interruptType);
            interruptType_.MarkDirtyRepaint();

            blendCurve_.SetValueWithoutNotify(transition_.blendCurve);

        }


        private void OnExitTimeChanged(ChangeEvent<float> evt)
        {
            transition_.exitTime = exitTime_.value;
        }

        private void OnTransTimeChanged(ChangeEvent<float> evt)
        {
            if (evt.newValue < 0)
            {
                transTime_.SetValueWithoutNotify(0);
            }

            transition_.transTime = transTime_.value;

        }

        private void OnTransitionModeChanged(ChangeEvent<Enum> evt)
        {
            transition_.interruptType = (InterruptType)interruptType_.value;
        }

        private void OnBlendCurveChanged(ChangeEvent<AnimationCurve> evt)
        {
            transition_.blendCurve = blendCurve_.value;
        }
    }

    public class Inspector_Transition : InspectorBase
    {
        private StateTransitionEdge Target => (StateTransitionEdge)base.target_;

        private readonly TransitionDrawer _transition0;

        private readonly TransitionDrawer _transition1;

        private readonly WantsToEditTransition onWantsToEditTransition_;

        private readonly WantsToDeleteTransition onWantsToDeleteTransition_;


        public Inspector_Transition(Action<int> addTransitionElement,
            Action<int> removeTransitionElement, WantsToEditTransition onWantsToEditTransition,
            WantsToDeleteTransition onWantsToDeleteTransition)
        {
            onWantsToEditTransition_ = onWantsToEditTransition;
            onWantsToDeleteTransition_ = onWantsToDeleteTransition;

            var scrollView = new ScrollView();
            Add(scrollView);

            _transition0 = new TransitionDrawer(FieldLabelWidth,
                addTransitionElement, removeTransitionElement, OnWantsToEditTransition, OnWantsToDeleteTransition);
            scrollView.contentContainer.Add(_transition0);

            _transition1 = new TransitionDrawer(FieldLabelWidth,
                addTransitionElement, removeTransitionElement, OnWantsToEditTransition, OnWantsToDeleteTransition);
            scrollView.contentContainer.Add(_transition1);
        }

        public override void SetTarget(IInspectable target)
        {
            base.SetTarget(target);

            if (Target == null || Target.IsEntryEdge)
            {
                // Entry node
                _transition0.visible = false;
                _transition0.SetEnabled(false);
                _transition1.visible = false;
                _transition1.SetEnabled(false);
                return;
            }

            var transition0 = Target.ConnectedNode0.OutputTransitions.FirstOrDefault(e =>
                e.IsConnection(Target.ConnectedNode0, Target.ConnectedNode1));
            if (transition0 != null)
            {
                _transition0.visible = true;
                _transition0.SetEnabled(true);
                _transition0.style.position = Position.Relative;
                var data0 = Target.ConnectedNode0.Transitions.Find(t =>
                    t.nextState.Equals(Target.ConnectedNode1.state_.id));
                _transition0.SetTransition((EditorNode_SM_State)transition0.ConnectedNode0, (EditorNode_SM_State)transition0.ConnectedNode1, data0);
            }
            else
            {
                _transition0.visible = false;
                _transition0.SetEnabled(false);
                _transition0.style.position = Position.Absolute;
            }

            var transition1 = Target.ConnectedNode1.OutputTransitions.FirstOrDefault(e =>
                e.IsConnection(Target.ConnectedNode0, Target.ConnectedNode1));
            if (transition1 != null)
            {
                _transition1.visible = true;
                _transition1.SetEnabled(true);
                _transition1.style.position = Position.Relative;
                var data1 = Target.ConnectedNode1.Transitions.Find(t =>
                    t.nextState.Equals(Target.ConnectedNode0.state_.id));
                _transition1.SetTransition((EditorNode_SM_State)transition1.ConnectedNode1, (EditorNode_SM_State)transition1.ConnectedNode0, data1);
            }
            else
            {
                _transition1.visible = false;
                _transition1.SetEnabled(false);
                _transition1.style.position = Position.Absolute;
            }
        }


        private void OnWantsToEditTransition(Transition transition)
        {
            onWantsToEditTransition_(transition);
        }

        private void OnWantsToDeleteTransition(EditorNode_SM_State fromNode, EditorNode_SM_State destNode)
        {
            onWantsToDeleteTransition_(fromNode, destNode);
             
            SetTarget(Target.IsConnection(fromNode, destNode) ? Target : null);
        }

    }
}
