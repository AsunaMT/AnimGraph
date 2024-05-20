using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Playables;

namespace AnimGraph
{
    [Serializable]
    public class Node_GetVar : DataNodeBase
    {
        public readonly Variable variable_;
        public Node_GetVar(Variable variable) 
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
        }
    }
}
