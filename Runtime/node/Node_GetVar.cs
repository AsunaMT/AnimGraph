using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace AnimGraph
{
    [Serializable]
    public class Node_GetVar : DataNodeBase
    {
        [SerializeReference]
        public Variable variable_;
        public string varName_;
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

        public override void InitNode(Animator animator, PlayableGraph graph, Dictionary<string, Variable> variables)
        {
            base.InitNode(animator, graph, variables);

            variable_ = variables[variable_.name];
            val_ = variable_.value;
            valType_ = variable_.type switch
            {
                VarType.EBool => PinType.EBool,
                VarType.EInt => PinType.EInt,
                VarType.EFloat => PinType.EFloat,
                _ => PinType.EBool,
            };
        }
    }
}
