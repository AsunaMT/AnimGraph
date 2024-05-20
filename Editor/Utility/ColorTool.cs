using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

namespace AnimGraph.Editor
{
    public static class ColorTool
    {
        private static readonly Dictionary<PinType, Color> _colors = new Dictionary<PinType, Color>
        {
            { PinType.EAnim, Color.cyan },
            { PinType.EBool, Color.red },
            { PinType.EInt, Color.green },
            { PinType.EFloat, Color.blue },
            //{ typeof(StateMachineEntryEditorNode), new Color(30 / 255f, 110 / 255f, 55 / 255f) },9
        };

        public static Color GetColor(PinType type)
        {
            if (_colors.TryGetValue(type, out var color))
            {
                return color;
            }

            return GetRandomColor();
        }

        public static Color GetSeparatorColor()
        {
            return new Color(35 / 255f, 35 / 255f, 35 / 255f);
        }

        public static Color GetRandomColor(byte rgbMax = 200, byte minInterval = 25, int? randomSeed = null)
        {
            if (randomSeed != null)
            {
                Random.InitState(randomSeed.Value);
            }

            byte r, g, b;
            while (true)
            {
                r = (byte)Random.Range(0, rgbMax + 1);
                g = (byte)Random.Range(0, rgbMax + 1);
                b = (byte)Random.Range(0, rgbMax + 1);

                if (Math.Abs(r - g) >= minInterval &&
                    Math.Abs(g - b) >= minInterval &&
                    Math.Abs(b - r) >= minInterval)
                {
                    break;
                }
            }

            return new Color(r / 255f, g / 255f, b / 255f, 1.0f);
        }
    }
}
