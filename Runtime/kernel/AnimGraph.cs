using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            mainGraph_.CreatePlayable(animator_, graph_);
            mainGraph_.InitConnection(animator_, graph_);
            output_ = AnimationPlayableOutput.Create(graph_, "AnimGraph", animator);
            output_.SetSourcePlayable(mainGraph_.outputPlayable_);
        }

        public void Play()
        {
            mainGraph_.Play();
        }

        public void Pause()
        {
            mainGraph_.Pause();
        }

        public bool GetBool(string name)
        {
            if (varName2Index_.TryGetValue(name, out var index))
            {
                return varList_[index].Bool();
            }
            else
            {
                return false;
            }
        }

        public int GetInt(string name)
        {
            if (varName2Index_.TryGetValue(name, out var index))
            {
                return varList_[index].Int();
            }
            else
            {
                return 0;
            }
        }

        public float GetFloat(string name)
        {
            if (varName2Index_.TryGetValue(name, out var index))
            {
                return varList_[index].Float();
            }
            else
            {
                return 0;
            }
        }

        public void SetBool(string name, bool value)
        {
            if (varName2Index_.TryGetValue(name, out var index))
            {
                varList_[index].SetBool(value);
            }
        }

        public void SetInt(string name, int value)
        {
            if (varName2Index_.TryGetValue(name, out var index))
            {
                varList_[index].SetInt(value);
            }
        }

        public void SetFloat(string name, float value)
        {
            if (varName2Index_.TryGetValue(name, out var index))
            {
                varList_[index].SetFloat(value);
            }
        }
    }
}
