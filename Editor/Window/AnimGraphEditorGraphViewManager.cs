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


        public AnimGraphEditorWindow window;

        public Action<IReadOnlyList<ISelectable>> OnGraphViewSelectionChanged { get; internal set; }

        public AnimGraphEditorGraphViewManager(VisualElement viewContainer)
        {
            _viewContainer = viewContainer;

            var graphViewToolbar = new Toolbar();
            _viewContainer.Add(graphViewToolbar);
            _graphViewBreadcrumbs = new ToolbarBreadcrumbs();
            graphViewToolbar.Add(_graphViewBreadcrumbs);
        }

        public void OpenGraphView(IGraphNode graphAsset)
        {
            if(graphAsset == _graphAsset)
            {
                return;
            }
            _graphAsset = graphAsset;

            GraphViewBase graphView;
            if (graphAsset is Node_SubGraph subGraph)
            {
                graphView = new SubGraphView(subGraph, _viewContainer, window._graphAsset.varList_);
            }
            else if(graphAsset is Node_Function func)
            {
                graphView = new FunctionGraphView(func, _viewContainer, window._graphAsset.varList_);
            } 
            else if(graphAsset is Node_StateMachine stateMachine)
            {
                graphView = new StateMachineGraphView(stateMachine, _viewContainer);
            } 
            else
            {
                return;
            }

            var activeGraphView = GetCurGraphView();
            if (activeGraphView != null)
            {
                _viewContainer.Remove(activeGraphView);
            }

            graphView.window_ = window;
            _viewContainer.Add(graphView);
            _openedGraphViews.Push(graphView);
            _graphViewBreadcrumbs.PushItem(graphAsset.Name, () => { CloseGraphViews(graphAsset); });
            _graphViewBreadcrumbs[_graphViewBreadcrumbs.childCount - 1].style.overflow = Overflow.Hidden;
            graphView.OnSelectionChanged += OnGraphViewSelectionChanged;
            graphView.OpenGraphEvent += OpenGraphView;
            OnGraphViewSelectionChanged?.Invoke(null);
        }

        public void CloseGraphViews(IGraphNode stopAtGraph)
        {
            for (int i = _openedGraphViews.Count - 1; i >= 0; i--)
            {
                var graph = _openedGraphViews.Peek();
                if (graph.GraphAsset == stopAtGraph)
                {
                    _viewContainer.Add(graph);
                    _graphAsset = stopAtGraph;
                    return;
                }
                _openedGraphViews.Pop();
                _graphViewBreadcrumbs.PopItem();

                if (_viewContainer.Contains(graph))
                {
                    _viewContainer.Remove(graph);
                }
            }
        }

        private GraphViewBase GetCurGraphView()
        {
            return _openedGraphViews.Count > 0 ? _openedGraphViews.Peek() : null;
        }

    }
}
