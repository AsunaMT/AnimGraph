using AnimGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimGraph
{
    public abstract class OperatorNodeBase : DataNodeBase
    {
#if UNITY_EDITOR
        public string operatorName;
#endif
    }
}
