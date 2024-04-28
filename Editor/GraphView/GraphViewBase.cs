using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimGraph.Editor 
{
    public abstract class GraphViewBase : GraphView
    {
        private const string GRID_BACKGROUND_STYLE_PATH = "GridBackground";
        public abstract EditorNodeBase RootNode { get; }

        protected IGraphNode GraphAsset { get; }


        public VisualElement parentContainer_;

        public AnimGraphEditorWindow window_;

        protected GraphViewBase(IGraphNode graphAsset, VisualElement container)
        {
            GraphAsset = graphAsset;

            parentContainer_ = container;

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            SetupContextMenu();
            style.flexGrow = 1;

            // Grid background
            var gridStyleSheet = Resources.Load<StyleSheet>(GRID_BACKGROUND_STYLE_PATH);
            styleSheets.Add(gridStyleSheet);
            var gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);

        }

        private void SetupContextMenu()
        {
            var contextMenu = new ContextualMenuManipulator(evt =>
            {
                // 将菜单绑定到事件
                evt.menu.AppendAction("Create", action =>
                {
                    ShowSearchWindow(action.eventInfo.mousePosition);
                });
            });

            // 添加上下文菜单到GraphView
            this.AddManipulator(contextMenu);
        }

        private void ShowSearchWindow(Vector2 mousePosition)
        {
            // 创建搜索窗口提供者
            var searchWindowProvider = ScriptableObject.CreateInstance<MySearchWindowProvider>();
            // 显示搜索窗口

            SearchWindow.Open(new SearchWindowContext(mousePosition + window_.position.position), searchWindowProvider);
        }

        Vector2 GetLocalPosition(Vector2 screenPosition)
        {
            return screenPosition;
            //var temp = contentViewContainer.WorldToLocal(screenPosition)
            var temp = screenPosition;
            return new Vector2(temp.x + contentViewContainer.transform.position.x,
                temp.y + contentViewContainer.transform.position.y);
        }

    }

    public class MySearchWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            // 创建搜索树条目
            var entries = new List<SearchTreeEntry>
                {
                    new SearchTreeGroupEntry(new GUIContent("Create"), 0)
                };

            // 添加其他搜索树条目

            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
        {
            // 处理选择条目的逻辑

            return true;
        }
    }

    public class NodeTable {
        public static readonly Dictionary<Type, Type> EditorNode2Runtime = new Dictionary<Type, Type>()
        {
            {typeof(EditorNode_Mixer), typeof(Node_Mixer) },
        };
    }

}
