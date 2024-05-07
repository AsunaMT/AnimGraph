using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Playables;

namespace AnimGraph
{
    [Serializable]
    public class DataNodeBase : GraphNodeBase
    {
        public Variable val_;
        public PinType valType_;
    }
}
