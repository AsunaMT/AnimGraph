using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimGraph.Editor
{
    public class EditorNode_GetVar : EditorNodeBase
    {
        public EditorNode_GetVar(Node_GetVar node, GraphViewBase grapView) : base(node, grapView, node.GetOutType())
        {
            node_ = node;
            title = "Get " + node.variable_.name;
        }
    }
}
