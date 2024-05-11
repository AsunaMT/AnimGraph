using UnityEditor;
using UnityEngine;

namespace AnimGraph.Editor
{
    public class EditorNode_StateMachine : EditorNodeBase
    {
        public EditorNode_StateMachine(Node_StateMachine node, GraphViewBase grapView) : base(node, grapView)
        {
            title = "State Machine";
        }
    }
}