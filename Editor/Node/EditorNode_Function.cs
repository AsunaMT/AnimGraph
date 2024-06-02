using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimGraph.Editor
{
    public class EditorNode_Function : EditorNodeBase
    {
        public Node_Function func => (Node_Function)node_;

        public EditorNode_Function(Node_Function func, GraphViewBase grapView) : base(func, grapView, func.GetOutType())
        {
            node_ = func;
            if (string.IsNullOrEmpty(func.name_))
            {
                title = string.Format("Function({0})", func.id_);
            }
            else
            {
                title = func.name_;
            }
        }

        public override InspectorBase GetInspector()
        {
            var inspector = new Inspector_Function();
            inspector.SetTarget(this);
            return inspector;
        }
    }
}
