using AnimGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AnimGraph
{
    public class Node_Not : OperatorNodeBase
    {
        public Node_Not()
        {
#if UNITY_EDITOR
            operatorName = "Not";
#endif
            val_ = new Value(false);
            valType_ = PinType.EBool;
            input_ = new List<NodePin>()
            {
                new NodePin()
                {
                    index = 0,
                    name = "input",
                    pinTye = PinType.EBool,
                },
            };
        }

        public override void Execute()
        {
            bool input = input_[0].Vaild && input_[0].GetBool();
            val_.SetBool(!input);
        }
    }
}
