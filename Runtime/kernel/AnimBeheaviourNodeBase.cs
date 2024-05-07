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
    public class AnimBehaviourNodeBase : AnimNodeBase
    {
        public Playable mainPlayable_;
        public BehaviourAdapter adapter_;

        public AnimBehaviourNodeBase() : base()
        {

        }

        public override void InitNode(Animator animator, PlayableGraph graph)
        {
            base.InitNode(animator, graph);
        }

        public override void CreatePlayable(Animator animator, PlayableGraph graph)
        {
            outputPlayable_ = ScriptPlayable<BehaviourAdapter>.Create(graph);
            adapter_ = ((ScriptPlayable<BehaviourAdapter>)outputPlayable_).GetBehaviour();
            adapter_.Init(this);
        }
    }
}
