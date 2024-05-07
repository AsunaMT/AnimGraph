using AnimGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Playables;

namespace AnimGraph
{
    public enum PinType
    {
        EAnim,
        EFloat,
        EInt,
        EBool,
    };

    [Serializable]
    public class NodePin
    {
        public int index;
        public PinType pinTye;
        public string name;
        [NonSerialized]
        public GraphNodeBase node;

        public bool Vaild => node != null;

        public AnimNodeBase GetAnim()
        {
            return (AnimNodeBase)node;
        }

        public Playable GetPlayable()
        {
            return ((AnimNodeBase)node).outputPlayable_;
        }

        public DataNodeBase GetData()
        {
            return (DataNodeBase)node;
        }

        public bool GetBool()
        {
            return ((DataNodeBase)node).val_.Bool();
        }

        public int GetInt()
        {
            return ((DataNodeBase)node).val_.Int();
        }

        public float GetFloat()
        {
            return ((DataNodeBase)node).val_.Float();
        }

        public string GetString()
        {
            return ((DataNodeBase)node).val_.String();
        }
    }
}
