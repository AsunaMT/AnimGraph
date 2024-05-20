using AnimGraph.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimGraph.Editor
{
    public class EditorNode_Operator : EditorNodeBase
    {
        public OperatorNodeBase operatorNode => (OperatorNodeBase)node_;

        public EditorNode_Operator(OperatorNodeBase node, GraphViewBase grapView) : base(node, grapView, node.GetOutType())
        {
            node_ = node;
            title = node.operatorName;
        }
    }
}
