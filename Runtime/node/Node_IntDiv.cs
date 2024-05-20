using AnimGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AnimGraph
{
    public class Node_IntDiv : OperatorNodeBase
    {
        public Node_IntDiv()
        {
#if UNITY_EDITOR
            operatorName = "÷";
#endif
            val_ = new Value(0f);
            valType_ = PinType.EInt;
            input_ = new List<NodePin>()
            {
                new NodePin()
                {
                    index = 0,
                    name = "left",
                    pinTye = PinType.EInt,
                },
                new NodePin()
                {
                    index = 1,
                    name = "right",
                    pinTye = PinType.EInt,
                }
            };
        }

        public override void Execute()
        {
            int left = input_[0].Vaild ? input_[0].GetInt() : 0;
            int right = input_[1].Vaild ? input_[1].GetInt() : 0;
            val_.SetInt(left / right);
        }
    }
}
