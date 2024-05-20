using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimGraph.Editor
{
    public class EditorNode_AnimClip : AnimEditorNodeBase
    {
        public Node_AnimClip clipNode_ => (Node_AnimClip)node_;

        public EditorNode_AnimClip(Node_AnimClip node, GraphViewBase grapView) : base(node, grapView)
        {
            node_ = node;
            title = "AnimClip";
        }

        public override InspectorBase GetInspector()
        {
            var inspector = new Inspector_AnimClip();
            inspector.SetTarget(this);

            return inspector;
        }
    }
}
