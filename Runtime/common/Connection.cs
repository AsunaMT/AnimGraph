using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimGraph
{
    [Serializable]
    public class ConnectionInfo
    {
        public bool sourceIsAnim;
        public bool targetIsAnim;
        public int sourceId;
        public int targetId;
        public int targetPort;
    }
}
