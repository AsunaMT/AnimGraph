using AnimGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AnimGraph
{
    public class Node_FloatSub : OperatorNodeBase
    {
        public Node_FloatSub()
        {
#if UNITY_EDITOR
            operatorName = "-";
#endif
            val_ = new Value(0f);
            valType_ = PinType.EFloat;
            input_ = new List<NodePin>()
            {
                new NodePin()
                {
                    index = 0,
                    name = "left",
                    pinTye = PinType.EFloat,
                },
                new NodePin()
                {
                    index = 1,
                    name = "right",
                    pinTye = PinType.EFloat,
                }
            };
        }

        public override void Execute()
        {
            float left = input_[0].Vaild ? input_[0].GetFloat() : 0f;
            float right = input_[1].Vaild ? input_[1].GetFloat() : 0f;
            val_.SetFloat(left - right);
        }
    }
}
