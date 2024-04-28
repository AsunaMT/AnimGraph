using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace AnimGraph
{
    [Serializable]
    [CreateAssetMenu(fileName = "AnimGraph", menuName = "MyAsset/AnimGraph")]
    public class AnimGraph : ScriptableObject
    {
        public Node_SubGraph mainGraph_;

        private PlayableGraph graph_;

        private Animator animator_;

        public PlayableOutput output_;

        public List<Variable> varList_;

        public Dictionary<string, int> varName2Index_ = new Dictionary<string, int>();

        public void Init(Animator animator)
        {
            animator_ = animator;
            graph_ = PlayableGraph.Create();
            mainGraph_.InitNode(animator_, graph_);
            output_ = AnimationPlayableOutput.Create(graph_, "AnimGraph", animator);
            output_.SetSourcePlayable(mainGraph_.outputPlayable_);
        }
    }
}
