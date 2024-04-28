using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Playables;

namespace AnimGraph.Editor
{

    public partial class AnimGraphEditorWindow : EditorWindow
    {
        #region Global

        private static AnimGraphEditorWindow _focusedWindow;

        [OnOpenAsset]
        public static bool OnOpenAnimationGraphAsset(int instanceId, int line)
        {
            var asset = EditorUtility.InstanceIDToObject(instanceId);
            if (asset is AnimGraph animGraphAsset)
            {
                var success = true;
                var editor = Resources.FindObjectsOfTypeAll<AnimGraphEditorWindow>()
                    .FirstOrDefault(window => window._graphAsset == animGraphAsset);
                // var editor = windows.Find();
                if (!editor)
                {
                    editor = CreateInstance<AnimGraphEditorWindow>();
                    try
                    {
                        editor.OpenGraphAsset(animGraphAsset);
                    }
                    catch
                    {
                        success = false;
                    }
                }

                if (success)
                {
                    editor.Show();
                    editor.Focus();
                }
                else
                {
                    editor.Close();
                }

                return true;
            }

            return false;
        }

        public static bool ShowNotificationOnFocusedWindow(string message, MessageType messageType, float duration)
        {
            if (!_focusedWindow)
            {
                return false;
            }

            GUIContent content;
            switch (messageType)
            {
                case MessageType.None:
                case MessageType.Info:
                    content = EditorGUIUtility.IconContent("console.infoicon");
                    break;

                case MessageType.Warning:
                    content = EditorGUIUtility.IconContent("console.warnicon");
                    break;

                case MessageType.Error:
                    content = EditorGUIUtility.IconContent("console.erroricon");
                    break;

                default:
                    return false;
            }

            content.text = message;
            _focusedWindow.ShowNotification(content);

            return true;
        }

        #endregion



        private TripleSplitterRowView _layoutContainer;

        private AnimGraph _graphAsset;

        private AnimGraph _graphAssetSnapshot;


        public override void SaveChanges()
        {
            base.SaveChanges();

            EditorUtility.SetDirty(_graphAsset);
            AssetDatabase.SaveAssetIfDirty(_graphAsset);

            DestroyImmediate(_graphAssetSnapshot);
            _graphAssetSnapshot = Instantiate(_graphAsset);
        }

        public override void DiscardChanges()
        {
            base.DiscardChanges();

            _graphAssetSnapshot.name = _graphAsset.name;
            EditorUtility.CopySerializedManagedFieldsOnly(_graphAssetSnapshot, _graphAsset);
            DestroyImmediate(_graphAssetSnapshot);
        }

        private void OnEnable()
        {
            // Toolbar
            CreateToolbar();

            // Layout container
            _layoutContainer = new TripleSplitterRowView(new Vector2(200, 400), new Vector2(200, 400));
            rootVisualElement.Add(_layoutContainer);

            // Fill view
           // CreateBlackboardPanel();
            CreateGraphViewPanel();
           // CreateInspectorPanel();

            // Try restore editor(after code compiling)
            //TryRestoreEditor();
        }

        private void Update()
        {
/*            // Update sub components
            _blackboardManager.Update(_changedDataCategories);
            _graphViewManager.Update(_changedDataCategories);
            _inspectorManager.Update(_changedDataCategories);

            // Update inspect target
            if ((_changedDataCategories & DataCategories.GraphContent) != 0)
            {
                SetInspectTarget(_graphViewManager.GetSelectedGraphElements());
            }

            _changedDataCategories = DataCategories.None;*/
        }

        private void OnGUI()
        {
            // Shortcuts
            var evt = Event.current;
            if (evt.control && evt.keyCode == KeyCode.S)
            {
                SaveChanges();
            }
        }

        private void OpenGraphAsset(AnimGraph graphAsset)
        {
            _graphViewManager.CloseGraphViews(null);

            _graphAsset = graphAsset;
            _graphAssetSnapshot = Instantiate(_graphAsset);

            // Window
            titleContent.text = _graphAsset.name;

/*            // Blackboard
            _blackboardManager.Initialize(_graphAsset);*/

            // GraphView
           _graphViewManager.OpenGraphView(_graphAsset.mainGraph_);
        }

    }
}
