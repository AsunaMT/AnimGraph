using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimGraph.Editor
{
    public class EditorNode_SubGraph : AnimEditorNodeBase
    {
        public Node_SubGraph graph_ => (Node_SubGraph)node_;

        public EditorNode_SubGraph(Node_SubGraph node, GraphViewBase grapView) : base(node, grapView)
        {
            node_ = node;
            if (string.IsNullOrEmpty(node.name_))
            {
                title = string.Format("Sub AnimGraph({0})", graph_.id_);
            }
            else
            {
                title = node.name_;
            }
            
        }

        public override InspectorBase GetInspector()
        {
            var inspector = new Inspector_SubGraph();
            inspector.SetTarget(this);
            return inspector;
        }
    }
}
