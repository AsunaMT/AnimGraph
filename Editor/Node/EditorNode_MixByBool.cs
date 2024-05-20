using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimGraph.Editor
{
    public class EditorNode_MixByBool : AnimEditorNodeBase
    {
        public Node_MixByBool mixer => (Node_MixByBool)node_;

        public EditorNode_MixByBool(Node_MixByBool node, GraphViewBase grapView) : base(node, grapView)
        {
            node_ = node;
            title = "Mix by bool";
        }

        public override InspectorBase GetInspector()
        {
            var inspector = new Inspector_MixByBool();
            inspector.SetTarget(this);

            return inspector;
        }
    }
}
