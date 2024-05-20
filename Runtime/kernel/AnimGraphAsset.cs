using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace AnimGraph
{
    [Serializable]
    [CreateAssetMenu(fileName = "AnimGraph", menuName = "MyAsset/AnimGraph")]
    public class AnimGraphAsset : ScriptableObject
    {
        public Node_SubGraph mainGraph_ = new Node_SubGraph("AnimGraph");

        private PlayableGraph graph_;

        private Animator animator_;

        private PlayableOutput output_;

        public List<Variable> varList_ = new List<Variable>();

        public Dictionary<string, int> varName2Index_ = new Dictionary<string, int>();

        public void Init(Animator animator)
        {
            mainGraph_ ??= new Node_SubGraph("AnimGraph");
            varList_ ??= new List<Variable>();
            InitVarTable();
            animator_ = animator;
            animator_.applyRootMotion = false;
            graph_ = PlayableGraph.Create();
            mainGraph_.InitNode(animator_, graph_);
            mainGraph_.CreatePlayable(animator_, graph_);
            mainGraph_.InitConnection(animator_, graph_);
            output_ = AnimationPlayableOutput.Create(graph_, "AnimGraph", animator);
            output_.SetSourcePlayable(mainGraph_.outputPlayable_);
        }

        public void Play()
        {
            graph_.Play();
            mainGraph_.Play();
        }

        public void Pause()
        {
            mainGraph_.Pause();
        }

        public void Dispose()
        {
            mainGraph_?.Dispose();
            graph_.Destroy();
        }

        public bool GetBool(string name)
        {
            if (varName2Index_.TryGetValue(name, out var index))
            {
                return varList_[index].value.Bool();
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
                return varList_[index].value.Int();
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
                return varList_[index].value.Float();
            }
            else
            {
                return 0;
            }
        }

        public string GetString(string name)
        {
            if (varName2Index_.TryGetValue(name, out var index))
            {
                return varList_[index].value.String();
            }
            else
            {
                return null;
            }
        }

        public void SetBool(string name, bool value)
        {
            if (varName2Index_.TryGetValue(name, out var index))
            {
                varList_[index].value.SetBool(value);
            }
        }

        public void SetInt(string name, int value)
        {
            if (varName2Index_.TryGetValue(name, out var index))
            {
                varList_[index].value.SetInt(value);
            }
        }

        public void SetFloat(string name, float value)
        {
            if (varName2Index_.TryGetValue(name, out var index))
            {
                varList_[index].value.SetFloat(value);
            }
        }

        public void SetString(string name, string value)
        {
            if (varName2Index_.TryGetValue(name, out var index))
            {
                varList_[index].value.SetString(value);
            }
        }

        public void SetVar(string name, bool value)
        {
            SetBool(name, value);
        }

        public void SetVar(string name, int value)
        {
            SetInt(name, value);
        }

        public void SetVar(string name, float value)
        {
            SetFloat(name, value);
        }

        public void SetVar(string name, string value)
        {
            SetString(name, value);
        }

        public void InitVarTable()
        {
            for (int i = 0; i < varList_.Count; i++)
            {
                varName2Index_.Add(varList_[i].name, i);
            }
        }

#if UNITY_EDITOR

        public void AddBool()
        {
            int index = varList_.Count;
            var variable = new Variable("Bool_" + index.ToString(), false);
            varName2Index_.Add(variable.name, index);
            varList_.Add(variable);
        }

        public void AddInt()
        {
            int index = varList_.Count;
            var variable = new Variable("Int_" + index.ToString(), 0);
            varName2Index_.Add(variable.name, index);
            varList_.Add(variable);
        }

        public void AddFloat()
        {
            int index = varList_.Count;
            var variable = new Variable("Float_" + index.ToString(), 0f);
            varName2Index_.Add(variable.name, index);
            varList_.Add(variable);
        }

        public void ChangeVarName(Variable target, string newName)
        {
            int index = varName2Index_[target.name];
            varName2Index_.Remove(target.name);
            varName2Index_.Add(newName, index);
            target.name = newName;
        }

        public void DeleteVar(Variable target)
        {
            varList_.Remove(target);
            varName2Index_.Remove(target.name);
        }

#endif
    }
}
