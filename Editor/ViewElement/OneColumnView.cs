using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.UIElements.Cursor;

namespace AnimGraph.Editor
{
    public class OneColumnView : VisualElement
    {
        public VisualElement MainPane { get; }


        //  RegisterCallback<GeometryChangedEvent>(OnSizeChanged);

        public OneColumnView()
        {

            style.flexGrow = 1;
            style.flexDirection = FlexDirection.Column;

            // Up pane
            MainPane = new VisualElement
            {
                name = "up-pane",
                style =
                {
                    width = Length.Percent(100),
                    height = Length.Percent(60),
                    minHeight = Length.Percent(100),
                    maxHeight = Length.Percent(100),
                    paddingLeft = 2,
                    paddingRight = 2,
                    paddingTop = 2,
                    paddingBottom = 2,
                }
            };
            Add(MainPane);

        }


/*        private static Cursor LoadCursor(MouseCursor cursorStyle)
        {
            object boxed = new Cursor();
            typeof(Cursor).GetProperty("defaultCursorId", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(boxed, (int)cursorStyle, null);

            return (Cursor)boxed;
        }*/
    }
}
