using UnityEditor;
using UnityEngine;

namespace AnimGraph.Editor
{
    public class Inspector_Mixer : InspectorBase
    {
        public EditorNode_Mixer node_ => (EditorNode_Mixer)target_;

        public override void SetTarget(IInspectable target)
        {
            target_ = target;
        }
    }
}