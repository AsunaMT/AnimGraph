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

            style.flexGrow = 1;

            // Grid background
            var gridStyleSheet = Resources.Load<StyleSheet>(GRID_BACKGROUND_STYLE_PATH);
            styleSheets.Add(gridStyleSheet);
            var gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);

        }

        public abstract void AddConnection(EditorNodeBase source, EditorNodeBase target, int index);
        public abstract void RemoveConnection(int targetId, int index);

        Vector2 GetLocalPosition(Vector2 screenPosition)
        {
            return screenPosition;
            //var temp = contentViewContainer.WorldToLocal(screenPosition)
            var temp = screenPosition;
            return new Vector2(temp.x + contentViewContainer.transform.position.x,
                temp.y + contentViewContainer.transform.position.y);
        }

    }

    public class NodeTable {

        public static readonly Dictionary<Type, Type> EditorNode2Runtime = new Dictionary<Type, Type>()
        {
            {typeof(EditorNode_Mixer), typeof(Node_Mixer) },
        };

        public static readonly Dictionary<Type, Type> RuntimeNode2Editor = new Dictionary<Type, Type>()
        {
            {typeof(Node_Mixer), typeof(EditorNode_Mixer) },
        };
    }

}
