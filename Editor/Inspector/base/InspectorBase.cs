using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace AnimGraph.Editor
{
    public abstract class InspectorBase : VisualElement
    {
        protected Length FieldLabelWidth { get; set; } = Length.Percent(35);

        protected IInspectable target_;

        protected TextField NodeType { get; }

        public virtual void SetTarget(IInspectable target)
        {
            target_ = target;
            NodeType.labelElement.style.width = FieldLabelWidth;
            NodeType.SetValueWithoutNotify(target_?.GetType().Name);
        }

        public InspectorBase()
        {
            NodeType = new TextField("Type");
            NodeType.labelElement.style.minWidth = StyleKeyword.Auto;
            NodeType.labelElement.style.maxWidth = StyleKeyword.Auto;
            NodeType.labelElement.style.width = FieldLabelWidth;
            NodeType.SetEnabled(false);
            Add(NodeType);
        }
    }
}
