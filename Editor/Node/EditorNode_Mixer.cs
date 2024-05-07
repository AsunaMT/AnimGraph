using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimGraph.Editor
{
    public class EditorNode_Mixer : EditorNodeBase
    {
        Node_Mixer mixer => (Node_Mixer)node_;

        public EditorNode_Mixer(Node_Mixer node, GraphViewBase grapView) : base(node, grapView)
        {
            node_  = node;
            title = "mixer";
        }
    }
}
