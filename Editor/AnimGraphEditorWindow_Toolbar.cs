using UnityEditor;

namespace AnimGraph.Editor
{
    public partial class AnimGraphEditorWindow
    {
        private void CreateToolbar()
        {
            var toolbarMgr = new AnimGraphEditorToolbarManager(rootVisualElement);
            toolbarMgr.OnWantsToPingAsset += PingGraphAsset;
            toolbarMgr.OnWantsToSaveChanges += SaveChanges;
            toolbarMgr.OnWantsToToggleBlackboard += ToggleBlackboard;
            toolbarMgr.OnWantsToToggleInspector += ToggleInspector;
        }

        private void PingGraphAsset()
        {
            EditorGUIUtility.PingObject(_graphAsset);
        }

        private void ToggleBlackboard(bool enable)
        {
            _layoutContainer.ToggleLeftPane(enable);
        }

        private void ToggleInspector(bool enable)
        {
            _layoutContainer.ToggleRightPane(enable);
        }
    }
}
