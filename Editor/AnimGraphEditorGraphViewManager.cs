using GBG.AnimationGraph.Editor.GraphView;
using GBG.AnimationGraph.Graph;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace AnimGraph.Editor
{
    public class AnimGraphEditorGraphViewManager
    {
        public event Action<int> OnActiveGraphChanged;

        public event Action OnWantsToSaveChanges;

        private IGraphNode _graphAsset;

        private readonly VisualElement _viewContainer;

        private readonly ToolbarBreadcrumbs _graphViewBreadcrumbs;

        private readonly Stack<GraphViewBase> _openedGraphViews = new Stack<GraphViewBase>();

        private readonly List<string> _openedGraphGuids = new List<string>();

        public AnimGraphEditorWindow window;

        public AnimGraphEditorGraphViewManager(VisualElement viewContainer)
        {
            _viewContainer = viewContainer;

            var graphViewToolbar = new Toolbar();
            _viewContainer.Add(graphViewToolbar);
            _graphViewBreadcrumbs = new ToolbarBreadcrumbs();
            graphViewToolbar.Add(_graphViewBreadcrumbs);
        }

        public void OpenGraphView(Node_SubGraph graphAsset)
        {
            _graphAsset = graphAsset;


            GraphViewBase graphView = new SubGraphView(graphAsset, _viewContainer);
            graphView.window_ = window;

            _viewContainer.Add(graphView);
            _openedGraphViews.Push(graphView);
            _graphViewBreadcrumbs.PushItem(graphAsset.name_, () => { CloseGraphViews(graphAsset.name_); });
            _graphViewBreadcrumbs[_graphViewBreadcrumbs.childCount - 1].style.overflow = Overflow.Hidden;


        }

        public void CloseGraphViews(string stopAtGuid)
        {
            for (int i = _openedGraphGuids.Count - 1; i >= 0; i--)
            {
                var graph = _openedGraphViews.Peek();

                _openedGraphViews.Pop();
                _openedGraphGuids.RemoveAt(i);
                _graphViewBreadcrumbs.PopItem();

                if (_viewContainer.Contains(graph))
                {
                    _viewContainer.Remove(graph);
                }
            }
        }
        

    }
}
