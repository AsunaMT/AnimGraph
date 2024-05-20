using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace AnimGraph
{
    public class Node_IntGreater : OperatorNodeBase
    {
        public Node_IntGreater()
        {
#if UNITY_EDITOR
            operatorName = ">";
#endif
            val_ = new Value(0f);
            valType_ = PinType.EBool;
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
            val_.SetBool(left > right);
        }
    }
}
