using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimGraph
{
    [Serializable]
    public class Node_SetVar : DataNodeBase
    {
        public Variable variable_;
        public Node_SetVar(Variable variable)
        {
            variable_ = variable;
            val_ = variable.value;
            valType_ = variable.type switch
            {
                VarType.EBool => PinType.EBool,
                VarType.EInt => PinType.EInt,
                VarType.EFloat => PinType.EFloat,
                _ => PinType.EBool,
            };
            input_ = new List<NodePin>()
            {
                new NodePin()
                {
                    index = 0,
                    name = "Input",
                    pinTye = valType_,
                }
            };
        }
    }
}
