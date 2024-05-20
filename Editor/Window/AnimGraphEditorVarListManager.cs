using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimGraph.Editor
{
    public class AnimGraphEditorVarListManager
    {
        private AnimGraphAsset graphAsset;

        public AnimGraphEditorVarListManager(VisualElement viewContainer)
        {
            var view = new OneColumnView();
            viewContainer.Add(view);
            CreateVarListView(view);
            
        }

        public void Initialize(AnimGraphAsset graphAsset)
        {
            this.graphAsset = graphAsset;

            varListView.itemsSource = this.graphAsset.varList_;
        }


        #region Variable

        private Toolbar varLiatToolbar;

        private ListView varListView;


        private void CreateVarListView(OneColumnView view)
        {
            // Toolbar
            varLiatToolbar = new Toolbar { style = { justifyContent = Justify.SpaceBetween, } };
            view.MainPane.Add(varLiatToolbar);

            // Parameter label
            var varListPaneLabel = new Label("Variables");
            varLiatToolbar.Add(varListPaneLabel);

            // Add parameter button
            var addVariableButton = new ToolbarButton { text = "+" };
            // addParamButton.clickable ??= new Clickable((Action)null);
            addVariableButton.clickable.clickedWithEventInfo += OnAddVariableButtonClicked;
            varLiatToolbar.Add(addVariableButton);

            // Parameter list view
            varListView = new ListView
            {
                reorderable = true,
                reorderMode = ListViewReorderMode.Animated,
                fixedItemHeight = 24,
                makeItem = MakeVarListItem,
                bindItem = BindVarListItem,
                selectionType = SelectionType.Single,
            };
            varListView.itemIndexChanged += OnVarIndexChanged;
            view.MainPane.Add(varListView);
        }

        private void OnAddVariableButtonClicked(EventBase evt)
        {
            var menu = new GenericDropdownMenu();
            menu.AddItem("Bool", false, () =>
            {
                graphAsset.AddBool();
                varListView.RefreshItems();
            });
            menu.AddItem("Int", false, () =>
            {
                graphAsset.AddInt();
                varListView.RefreshItems();
            });
            menu.AddItem("Float", false, () =>
            {
                graphAsset.AddFloat();
                varListView.RefreshItems();
            });

            var menuPos = Vector2.zero;
            if (evt is IMouseEvent mouseEvt)
            {
                menuPos = mouseEvt.mousePosition;
            }
            else if (evt is IPointerEvent pointerEvt)
            {
                menuPos = pointerEvt.position;
            }
            else if (evt.target is VisualElement visualElem)
            {
                menuPos = visualElem.layout.center;
            }

            menu.DropDown(new Rect(menuPos, Vector2.zero), varLiatToolbar);
        }

        private VisualElement MakeVarListItem()
        {
            var varField = new VariableField();
            varField.OnVarValueChanged += OnVarValueChanged;
            varField.OnWantsToRenameVar += OnWantsToRenameVar;
            varField.OnWantsToDeleteVar += OnWantsToDeleteVar;
            return varField;
        }

        private void BindVarListItem(VisualElement listItem, int index)
        {
            var varField = (VariableField)listItem;
            var variable = graphAsset.varList_[index];
            varField.SetVariable(variable);
        }

        private void OnVarIndexChanged(int from, int to)
        {
        }

        private void OnVarValueChanged(Variable _)
        {
        }

        private void OnWantsToDeleteVar(Variable target)
        {
            graphAsset.DeleteVar(target);
            varListView.RefreshItems();
        }

        private void OnWantsToRenameVar(Variable target)
        {
            var conflictingNames = from variable in graphAsset.varList_
                                   where !variable.name.Equals(target.name)
                                   select variable.name;
            RenameWindow.Open(target.name, conflictingNames, (oldName, newName) =>
            {
                if (oldName.Equals(newName)) return;
                graphAsset.ChangeVarName(target, newName);
                varListView.RefreshItems();
            });
        }

        #endregion

    }
}
