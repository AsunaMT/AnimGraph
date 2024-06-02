using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;

namespace AnimGraph
{
    [Serializable]
    public abstract class DataNodeBase : GraphNodeBase
    {
        [SerializeReference]
        public Value val_;
        public PinType valType_;

        public Type GetOutType()
        {
            switch (valType_)
            {
                case PinType.EBool:
                    return typeof(bool);
                case PinType.EInt:
                    return typeof(int);
                case PinType.EFloat:
                    return typeof(float);
            }
            return null;
        }


        public bool GetBool()
        {
            Execute();
            return val_.Bool();
        }

        public int GetInt()
        {
            Execute();
            return val_.Int();
        }

        public float GetFloat()
        {
            Execute();
            return val_.Float();
        }


        public override void InitConnection(Animator animator, PlayableGraph graph)
        {
            
        }

        public override void InitNode(Animator animator, PlayableGraph graph, Dictionary<string, Variable> variables)
        {
            
        }
    }
}
