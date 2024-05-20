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
    public class VariableField : VisualElement
    {
        // public const string PARAM_NAME_MATCH_REGEX = "^[a-zA-Z_][a-zA-Z0-9_]*$";

        public event Action<Variable> OnVarValueChanged;

        public event Action<Variable> OnWantsToRenameVar;

        public event Action<Variable> OnWantsToDeleteVar;


        private readonly Label typeLabel;

        private readonly Label nameLabel;

        private readonly FloatField floatField;

        private readonly IntegerField intField;

        private readonly Toggle boolField;

        private Variable varInfo;

         
        public VariableField()
        {
            style.flexGrow = 1;
            style.flexDirection = FlexDirection.Row;
            style.justifyContent = Justify.SpaceBetween;

            RegisterCallback<MouseDownEvent>(OnMouseClicked);

            // Type
            typeLabel = new Label
            {
                style =
                {
                    width = 12,
                    fontSize = 12,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    marginRight = 10,
                }
            };
            Add(typeLabel);

            // Name
            nameLabel = new Label
            {
                style =
                {
                    flexGrow = 1,
                    unityTextAlign = TextAnchor.MiddleLeft,
                }
            };
            Add(nameLabel);

            // Float value
            floatField = new FloatField
            {
                style =
                {
                    width = Length.Percent(35),
                }
            };
            floatField.RegisterValueChangedCallback(OnFloatValueChanged);

            // Int value
            intField = new IntegerField
            {
                style =
                {
                    width = Length.Percent(35),
                }
            };
            intField.RegisterValueChangedCallback(OnIntValueChanged);

            // Bool value
            boolField = new Toggle
            {
                style =
                {
                    width = Length.Percent(35),
                }
            };
            boolField.RegisterValueChangedCallback(OnBoolValueChanged);
        }

        public void SetVariable(Variable variableInfo)
        {
            varInfo = variableInfo;

            // Type
            typeLabel.text = varInfo.type.ToString()[1..];

            // Name
            nameLabel.text = varInfo.name;

            // Value
            if (Contains(floatField)) Remove(floatField);
            if (Contains(intField)) Remove(intField);
            if (Contains(boolField)) Remove(boolField);
            switch (varInfo.type)
            {
                // Add value field and set value
                case VarType.EFloat:
                    floatField.SetValueWithoutNotify(varInfo.GetFloat());
                    Add(floatField);
                    break;

                case VarType.EInt:
                    intField.SetValueWithoutNotify(varInfo.GetInt());
                    Add(intField);
                    break;

                case VarType.EBool:
                    boolField.SetValueWithoutNotify(varInfo.GetBool());
                    Add(boolField);
                    break;

                default:
                    throw new ArgumentException();
            }
        }


        private void OnMouseClicked(MouseDownEvent evt)
        {
            // Left mouse button double click to rename param
            if (evt.button == 0 && evt.clickCount > 1)
            {
                OnWantsToRenameVar?.Invoke(varInfo);
                return;
            }

            // Right mouse button click to show contextual menu
            if (evt.button == 1)
            {
                var menuPos = evt.mousePosition;
                var menu = new GenericDropdownMenu();

                // Rename
                menu.AddItem("Rename", false, () => { OnWantsToRenameVar?.Invoke(varInfo); });

                // Delete
                menu.AddItem("Delete", false, () => { OnWantsToDeleteVar?.Invoke(varInfo); });

                menu.DropDown(new Rect(menuPos, Vector2.zero), this);
            }
        }


        private void OnFloatValueChanged(ChangeEvent<float> _)
        {
            varInfo?.SetFloat(floatField.value);
            OnVarValueChanged?.Invoke(varInfo);
        }

        private void OnIntValueChanged(ChangeEvent<int> _)
        {
            varInfo?.SetInt(intField.value);
            OnVarValueChanged?.Invoke(varInfo);
        }

        private void OnBoolValueChanged(ChangeEvent<bool> _)
        {
            varInfo?.SetBool(boolField.value);
            OnVarValueChanged?.Invoke(varInfo);
        }
    }
}
