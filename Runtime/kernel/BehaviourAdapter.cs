using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Playables;

namespace AnimGraph
{
    public class BehaviourAdapter : PlayableBehaviour
    {
        private AnimBehaviourNodeBase node_;

        public void Init(AnimBehaviourNodeBase node)
        {
            node_ = node;
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            node_?.Execute();
        }

        public void Play()
        {
            node_?.Play();
        }

        public void Pause()
        {
            node_?.Pause();
        }

        public T GetAnimBehaviour<T>() where T : AnimBehaviourNodeBase
        {
            return node_ as T;
        }
    }
}
