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
    public abstract class AnimNodeBase : GraphNodeBase
    {
        public Playable outputPlayable_;

        public AnimNodeBase()
        {
            isAnim_ = true;
        }

        public override void InitNode(Animator animator, PlayableGraph graph)
        {

        }

        public override void InitConnection(Animator animator, PlayableGraph graph)
        {

        }

        public virtual void CreatePlayable(Animator animator, PlayableGraph graph)
        {

        }

        public virtual void Play()
        {
            outputPlayable_.Play();
        }

        public virtual void Pause()
        {
            outputPlayable_.Pause();
        }

        public virtual void SetTime(float time)
        {
            outputPlayable_.SetTime(time);
        }

        
    }
}
