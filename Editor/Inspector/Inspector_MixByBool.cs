using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimGraph.Editor
{
    public class Inspector_MixByBool : InspectorBase
    {
        public EditorNode_MixByBool node_ => (EditorNode_MixByBool)target_;
        readonly BaseField<bool> controlField_;

        public Inspector_MixByBool()
        {
            controlField_ = new Toggle("Default Control");
            controlField_.style.flexGrow = 1;
            controlField_.style.marginLeft = 0;
            controlField_.style.marginRight = 0;
            controlField_.RegisterValueChangedCallback(OnControlChanged);
            Add(controlField_);
        }

        private void OnControlChanged(ChangeEvent<bool> evt)
        {
            node_.mixer.control_ = evt.newValue;
        }

        public override void SetTarget(IInspectable target)
        {
            base.SetTarget(target);
            controlField_.SetValueWithoutNotify(node_.mixer.control_);
        }
    }
}