using UnityEditor;
using UnityEngine;

namespace AnimGraph.Editor
{
    public class EditorNode_SM_Entry : EditorNode_SM_NodeBase
    {
        public override string StateName { get { return "Entry"; } internal set{ } }

        internal override List<Transition> Transitions { get { return null;  } }
    }
}