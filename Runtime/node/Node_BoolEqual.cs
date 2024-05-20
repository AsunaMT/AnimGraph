using AnimGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AnimGraph
{
    public class Node_BoolEqual : OperatorNodeBase
    {
        public Node_BoolEqual()
        {
#if UNITY_EDITOR
            operatorName = "&&";
#endif
            val_ = new Value(false);
            valType_ = PinType.EBool;
            input_ = new List<NodePin>()
            {
                new NodePin()
                {
                    index = 0,
                    name = "left",
                    pinTye = PinType.EBool,
                },
                new NodePin()
                {
                    index = 1,
                    name = "right",
                    pinTye = PinType.EBool,
                }
            };
        }

        public override void Execute()
        {
            bool left = input_[0].Vaild && input_[0].GetBool();
            bool right = input_[1].Vaild && input_[1].GetBool();
            val_.SetBool(left == right);
        }
    }
}
