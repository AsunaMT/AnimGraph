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
    public class Inspector_SubGraph : Inspector_Node
    {
        private EditorNode_SubGraph Node => (EditorNode_SubGraph)target_;
        readonly TextField nameFiled_;

        public Inspector_SubGraph()
        {
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
            if (Node == null)
            {
                return;
            }
            string newName = evt.newValue;
            if(!string.IsNullOrEmpty(newName))
            {
                Node.graph_.name_ = newName;
                Node.title = newName;
            }
        }

        public override void SetTarget(IInspectable target)
        {
            base.SetTarget(target);
            nameFiled_.SetValueWithoutNotify(Node.graph_.name_);
        }
    }
}
