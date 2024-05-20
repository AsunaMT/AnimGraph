using AnimGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimGraph.Editor
{
    internal class EditorNode_SetVar : EditorNodeBase
    {
        public EditorNode_SetVar(Node_SetVar node, GraphViewBase grapView) : base(node, grapView, node.GetOutType())
        {
            node_ = node;
            title = "Set " + node.variable_.name;
        }
    }
}
