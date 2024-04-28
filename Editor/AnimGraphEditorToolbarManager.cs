using System;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AnimGraph.Editor
{
    public class AnimGraphEditorToolbarManager
    {
        public event Action OnWantsToPingAsset;

        public event Action OnWantsToSaveChanges;

        public event Action<bool> OnWantsToToggleBlackboard;

        public event Action<bool> OnWantsToToggleInspector;

        // TODO: Show live debug target Animator

        public AnimGraphEditorToolbarManager(VisualElement parent)
        {
            var toolbar = new Toolbar();
            parent.Add(toolbar);

            // Mode
            var modeLabel = new Label("ss")
            {
                style =
                {
                    marginLeft = 3,
                    marginRight = 3,
                }
            };
            modeLabel.SetEnabled(false);
            toolbar.Add(modeLabel);

            // Ping asset button
            var pingAssetButton = new ToolbarButton(PingAsset)
            {
                text = "Ping Asset"
            };
            toolbar.Add(pingAssetButton);

            // Save asset button
            var saveAssetButton = new ToolbarButton(SaveChanges)
            {
                text = "Save Changes"
            };
            toolbar.Add(saveAssetButton);


            // Blackboard toggle
            var blackboardToggle = new ToolbarToggle
            {
                text = "Blackboard",
                value = true,
            };
            blackboardToggle.RegisterValueChangedCallback(evt => ToggleBlackboard(evt.newValue));
            toolbar.Add(blackboardToggle);

            // Inspector toggle
            var inspectorToggle = new ToolbarToggle
            {
                text = "Inspector",
                value = true,
            };
            inspectorToggle.RegisterValueChangedCallback(evt => ToggleInspector(evt.newValue));
            toolbar.Add(inspectorToggle);
        }

        private void SaveChanges()
        {
            OnWantsToSaveChanges?.Invoke();
        }

        private void ToggleBlackboard(bool enable)
        {
            OnWantsToToggleBlackboard?.Invoke(enable);
        }

        private void ToggleInspector(bool enable)
        {
            OnWantsToToggleInspector?.Invoke(enable);
        }

        private void PingAsset()
        {
            OnWantsToPingAsset?.Invoke();
        }

    }
}
