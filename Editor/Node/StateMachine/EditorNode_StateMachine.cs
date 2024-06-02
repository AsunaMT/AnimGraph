using UnityEditor;
using UnityEngine;

namespace AnimGraph.Editor
{
    public class EditorNode_StateMachine : AnimEditorNodeBase
    {
        public Node_StateMachine machine_ => (Node_StateMachine)node_;
        public EditorNode_StateMachine(Node_StateMachine node, GraphViewBase grapView) : base(node, grapView)
        {
            node_ = node;
            if (string.IsNullOrEmpty(node.Name))
            {
                title = string.Format("State Machine({0})", machine_.id_);
            }
            else
            {
                title = node.Name;
            }

        }

        public override InspectorBase GetInspector()
        {
            var inspector = new Inspector_StateMachine();
            inspector.SetTarget(this);
            return inspector;
        }
    }
}