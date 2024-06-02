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

        private static AnimGraphEditorWindow focusedWindow_;

        [OnOpenAsset]
        public static bool OnOpenAnimationGraphAsset(int instanceId, int line)
        {
            var asset = EditorUtility.InstanceIDToObject(instanceId);
            if (asset is AnimGraphAsset animGraphAsset)
            {
                var success = true;
                AnimGraphEditorWindow editor = GetWindow<AnimGraphEditorWindow>(); //CreateInstance<AnimGraphEditorWindow>();
                try
                {
                    editor.OpenGraphAsset(animGraphAsset);
                }
                catch
                {
                    success = false;
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
            if (!focusedWindow_)
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
            focusedWindow_.ShowNotification(content);

            return true;
        }

        #endregion



        private TripleSplitterRowView _layoutContainer;

        public AnimGraphAsset _graphAsset;

        private AnimGraphAsset _graphAssetSnapshot;


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
            CreateVarListPanel();
            CreateGraphViewPanel();
            CreateInspectorPanel();

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

        private void OpenGraphAsset(AnimGraphAsset graphAsset)
        {
            graphViewManager.CloseGraphViews(null);

            _graphAsset = graphAsset;
            _graphAssetSnapshot = Instantiate(_graphAsset);

            // Window
            titleContent.text = _graphAsset.name;

            varListManager.Initialize(_graphAsset);
            _graphAsset.EditorInit();
            graphViewManager.OpenGraphView(_graphAsset.mainGraph_);
        }
    }
}
