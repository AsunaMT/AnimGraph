using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimGraph.Editor
{
    public class Inspector_Mixer : InspectorBase
    {
        public EditorNode_Mixer node_ => (EditorNode_Mixer)target_;
        readonly BaseField<float> weightField_;

        public Inspector_Mixer()
        {
            weightField_ = new Slider("Default Weight", 0f, 1f)
            {
                showInputField = true,
            };
            weightField_.style.flexGrow = 1;
            weightField_.style.marginLeft = 0;
            weightField_.style.marginRight = 0;
            weightField_.RegisterValueChangedCallback(OnWeightChanged);
            Add(weightField_);
        }

        private void OnWeightChanged(ChangeEvent<float> evt)
        {
            node_.mixer.weight_ = evt.newValue;
        }

        public override void SetTarget(IInspectable target)
        {
            base.SetTarget(target);
            weightField_.SetValueWithoutNotify(node_.mixer.weight_);
        }
    }
}