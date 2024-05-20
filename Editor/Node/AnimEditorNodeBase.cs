using AnimGraph.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Playables;

namespace AnimGraph.Editor
{
    public class AnimEditorNodeBase : EditorNodeBase 
    {
        public AnimEditorNodeBase(GraphNodeBase node, GraphViewBase grapView) : base(node, grapView, typeof(Playable)) { }
    }
}
