using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

namespace AnimGraph.Editor
{
    public partial class AnimGraphEditorWindow
    {
        private AnimGraphEditorInspectorManager inspectorManager;


        private void CreateInspectorPanel()
        {
            inspectorManager = new AnimGraphEditorInspectorManager(_layoutContainer.RightPane);
        }

        private void SetInspectTarget(IReadOnlyList<ISelectable> selection)
        {
            inspectorManager.SetInspectTarget(selection);
        }
    }
}
