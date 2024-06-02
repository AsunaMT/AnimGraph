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
    public abstract class AnimBehaviourNodeBase : AnimNodeBase
    {
        public Playable mainPlayable_;
        public BehaviourAdapter adapter_;

        public AnimBehaviourNodeBase() : base()
        {

        }

        public override void InitNode(Animator animator, PlayableGraph graph, Dictionary<string, Variable> variables)
        {
            base.InitNode(animator, graph, variables);
        }

        public override void CreatePlayable(Animator animator, PlayableGraph graph)
        {
            outputPlayable_ = ScriptPlayable<BehaviourAdapter>.Create(graph);
            adapter_ = ((ScriptPlayable<BehaviourAdapter>)outputPlayable_).GetBehaviour();
            adapter_.Init(this);
        }
    }
}
