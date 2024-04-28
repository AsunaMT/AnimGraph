using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace AnimGraph
{
    public class Node_AnimClip : AnimNodeBase
    {
        public AnimationClip clipAsset_;
        public AnimationClipPlayable clip_ => (AnimationClipPlayable)outputPlayable_;

        public override void CreatePlayable(Animator animator, PlayableGraph graph)
        {
            outputPlayable_ = AnimationClipPlayable.Create(graph, clipAsset_);
        }

        public override void Execute()
        {
            
        }
    }
}
