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
        private AnimGraphEditorGraphViewManager _graphViewManager;

        private void CreateGraphViewPanel()
        {
            _graphViewManager = new AnimGraphEditorGraphViewManager(_layoutContainer.MiddlePane);
            _graphViewManager.window = this;
        }

    }
}
