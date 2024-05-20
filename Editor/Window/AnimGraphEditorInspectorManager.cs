using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AnimGraph.Editor
{
    public class AnimGraphEditorInspectorManager
    {
        private readonly VisualElement viewContainer_;

        private InspectorBase inspector_;


        public AnimGraphEditorInspectorManager(VisualElement viewContainer)
        {
            viewContainer_ = viewContainer;

            var toolbar = new Toolbar();
            viewContainer_.Add(toolbar);
            var inspectorLabel = new Label("Inspector");
            toolbar.Add(inspectorLabel);
        }

        public void SetInspectTarget(IReadOnlyList<ISelectable> selection)
        {
            InspectorBase newInspector = null;
            if (selection != null && selection.Count == 1)
            {
                if (selection[0] is EditorNodeBase graphNode)
                {
                    newInspector = graphNode.GetInspector();
                }
                else if (selection[0] is StateTransitionEdge transitionEdge)
                {
                    newInspector = transitionEdge.GetInspector();
                }
            }

            if (newInspector == inspector_)
            {
                return;
            }

            if (inspector_ != null)
            {
                viewContainer_.Remove(inspector_);
                //inspector_.OnDataChanged -= OnDataChanged;
                inspector_ = null;
            }

            if (newInspector != null)
            {
                inspector_ = newInspector;
                //inspector_.OnDataChanged += OnDataChanged;
                viewContainer_.Add(inspector_);
            }
        }

        // TODO: Update inspector
/*        public void Update(DataCategories changedDataCategories)
        {
        }*/
    }
}
