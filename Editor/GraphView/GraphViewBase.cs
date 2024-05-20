using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimGraph.Editor 
{
    public abstract class GraphViewBase : GraphView
    {
        private const string GRID_BACKGROUND_STYLE_PATH = "GridBackground";
        public abstract EditorNodeBase RootNode { get; }

        public IGraphNode GraphAsset { get; }


        public VisualElement parentContainer_;

        public AnimGraphEditorWindow window_;

        public event Action<IReadOnlyList<ISelectable>> OnSelectionChanged;

        public event Action<IGraphNode> OpenGraphEvent;

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

        public override void AddToSelection(ISelectable selectable)
        {
            base.AddToSelection(selectable);
            OnSelectionChanged?.Invoke(selection);
        }

        public override void RemoveFromSelection(ISelectable selectable)
        {
            base.RemoveFromSelection(selectable);
            OnSelectionChanged?.Invoke(selection);
        }

        public override void ClearSelection()
        {
            base.ClearSelection();
            OnSelectionChanged?.Invoke(selection);
        }

        Vector2 GetLocalPosition(Vector2 screenPosition)
        {
            return screenPosition;
            //var temp = contentViewContainer.WorldToLocal(screenPosition)
            var temp = screenPosition;
            return new Vector2(temp.x + contentViewContainer.transform.position.x,
                temp.y + contentViewContainer.transform.position.y);
        }

        protected void OnOpenGraph(IGraphNode graphNode)
        {
            OpenGraphEvent?.Invoke(graphNode);
        }

        public void EditTransition(Transition transition)
        {
            OpenGraphEvent?.Invoke(transition.entity);
        }

        protected void OnDoubleClickNode(EditorNodeBase node)
        {
            if (node.node_ is IGraphNode graphNode)
            {
                OnOpenGraph(graphNode);
            }
        }
    }

    public class NodeTable {

        public static readonly Dictionary<Type, Type> EditorNode2Runtime = new Dictionary<Type, Type>()
        {
            {typeof(EditorNode_Mixer), typeof(Node_Mixer) },
            {typeof(EditorNode_AnimClip), typeof(Node_AnimClip) },
            {typeof(EditorNode_SubGraph), typeof(Node_SubGraph) },
            {typeof(EditorNode_StateMachine), typeof(Node_StateMachine) },
            {typeof(EditorNode_Function), typeof(Node_Function) },
            {typeof(EditorNode_GetVar), typeof(Node_GetVar) },
            {typeof(EditorNode_SetVar), typeof(Node_SetVar) },
            {typeof(EditorNode_MixByBool), typeof(Node_MixByBool) },
        };

        public static readonly Dictionary<Type, Type> RuntimeNode2Editor = new Dictionary<Type, Type>()
        {
            {typeof(Node_Mixer), typeof(EditorNode_Mixer) },
            {typeof(Node_AnimClip), typeof(EditorNode_AnimClip) },
            {typeof(Node_SubGraph), typeof(EditorNode_SubGraph) },
            {typeof(Node_StateMachine), typeof(EditorNode_StateMachine) },
            {typeof(Node_Function), typeof(EditorNode_Function) },
            {typeof(Node_GetVar), typeof(EditorNode_GetVar) },
            {typeof(Node_SetVar), typeof(EditorNode_SetVar) },
            {typeof(Node_MixByBool), typeof(EditorNode_MixByBool) },
        };
    }

}
