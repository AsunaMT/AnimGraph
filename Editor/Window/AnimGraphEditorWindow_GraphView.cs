using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Playables;

namespace AnimGraph.Editor
{
    public partial class AnimGraphEditorWindow
    {
        private AnimGraphEditorGraphViewManager graphViewManager;

        private void CreateGraphViewPanel()
        {
            graphViewManager = new AnimGraphEditorGraphViewManager(_layoutContainer.MiddlePane);
            graphViewManager.OnGraphViewSelectionChanged += SetInspectTarget;
            graphViewManager.window = this;
        }

    }
}
