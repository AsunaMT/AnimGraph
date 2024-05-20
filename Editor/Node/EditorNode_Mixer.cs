using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimGraph.Editor
{
    public class EditorNode_Mixer : AnimEditorNodeBase
    {
        public Node_Mixer mixer => (Node_Mixer)node_;

        public EditorNode_Mixer(Node_Mixer node, GraphViewBase grapView) : base(node, grapView)
        {
            node_  = node;
            title = "mixer";
        }

        public override InspectorBase GetInspector()
        {
            var inspector = new Inspector_Mixer();
            inspector.SetTarget(this);

            return inspector;
        }
    }
}
