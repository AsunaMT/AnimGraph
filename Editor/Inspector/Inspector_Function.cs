using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace AnimGraph.Editor
{
    public class Inspector_Function : Inspector_Node
    {
        private EditorNode_Function Node => (EditorNode_Function)target_;
        readonly TextField nameFiled_;

        public Inspector_Function()
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
            if (!string.IsNullOrEmpty(newName))
            {
                Node.func.name_ = newName;
                Node.title = newName;
            }
        }

        public override void SetTarget(IInspectable target)
        {
            base.SetTarget(target);
            nameFiled_.SetValueWithoutNotify(Node.func.name_);
        }
    }
}
