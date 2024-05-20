using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;

namespace AnimGraph.Editor
{
    public class Inspector_AnimClip : Inspector_Node
    {
        private EditorNode_AnimClip Node => (EditorNode_AnimClip)target_;

        private readonly ObjectField clipField_;


        public Inspector_AnimClip()
        {
            // Clip
            clipField_ = new ObjectField("Clip")
            {
                objectType = typeof(AnimationClip),
            };
            clipField_.labelElement.style.minWidth = StyleKeyword.Auto;
            clipField_.labelElement.style.maxWidth = StyleKeyword.Auto;
            clipField_.labelElement.style.width = FieldLabelWidth;
            clipField_.RegisterValueChangedCallback(OnClipChanged);
            Add(clipField_);
        }

        public override void SetTarget(IInspectable target)
        {
            base.SetTarget(target);
            // Clip
            clipField_.SetValueWithoutNotify(Node.clipNode_.clipAsset_);
        }


        private void OnClipChanged(ChangeEvent<Object> evt)
        {
            Node.clipNode_.clipAsset_ = (AnimationClip)evt.newValue;
        }

    }
}
