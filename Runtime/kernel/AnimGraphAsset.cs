using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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

        [SerializeReference]
        public List<Variable> varList_ = new List<Variable>();

        public Dictionary<string, Variable> varTable_ = new Dictionary<string, Variable>();

        private bool init_ = false;

        public void Init(Animator animator)
        {
            mainGraph_ ??= new Node_SubGraph("AnimGraph");
            varList_ ??= new List<Variable>();
            InitVarTable();
            animator_ = animator;
            animator_.applyRootMotion = false;
            graph_ = PlayableGraph.Create();
            mainGraph_.InitNode(animator_, graph_, varTable_);
            mainGraph_.CreatePlayable(animator_, graph_);
            mainGraph_.InitConnection(animator_, graph_);
            output_ = AnimationPlayableOutput.Create(graph_, "AnimGraph", animator);
            output_.SetSourcePlayable(mainGraph_.outputPlayable_);
            init_ = true;
        }

        public void EditorInit()
        {
            if (!init_)
            {
                InitVarTable();
                graph_ = PlayableGraph.Create();
                mainGraph_?.InitNode(null, graph_, varTable_);
            }
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
            if (varTable_.TryGetValue(name, out var res))
            {
                return res.value.Bool();
            }
            else
            {
                return false;
            }
        }

        public int GetInt(string name)
        {
            if (varTable_.TryGetValue(name, out var res))
            {
                return res.value.Int();
            }
            else
            {
                return 0;
            }
        }

        public float GetFloat(string name)
        {
            if (varTable_.TryGetValue(name, out var res))
            {
                return res.value.Float();
            }
            else
            {
                return 0;
            }
        }

        public string GetString(string name)
        {
            if (varTable_.TryGetValue(name, out var res))
            {
                return res.value.String();
            }
            else
            {
                return null;
            }
        }

        public void SetBool(string name, bool value)
        {
            if (varTable_.TryGetValue(name, out var res))
            {
                res.value.SetBool(value);
            }
        }

        public void SetInt(string name, int value)
        {
            if (varTable_.TryGetValue(name, out var res))
            {
                res.value.SetInt(value);
            }
        }

        public void SetFloat(string name, float value)
        {
            if (varTable_.TryGetValue(name, out var res))
            {
                res.value.SetFloat(value);
            }
        }

        public void SetString(string name, string value)
        {
            if (varTable_.TryGetValue(name, out var res))
            {
                res.value.SetString(value);
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
            varTable_ = new Dictionary<string, Variable>();
            for (int i = 0; i < varList_.Count; i++)
            {
                varTable_.Add(varList_[i].name, varList_[i]);
            }
        }

#if UNITY_EDITOR

        public void AddBool()
        {
            int index = varList_.Count;
            var variable = new Variable("Bool_" + index.ToString(), false);
            varTable_.Add(variable.name, variable);
            varList_.Add(variable);
        }

        public void AddInt()
        {
            int index = varList_.Count;
            var variable = new Variable("Int_" + index.ToString(), 0);
            varTable_.Add(variable.name, variable);
            varList_.Add(variable);
        }

        public void AddFloat()
        {
            int index = varList_.Count;
            var variable = new Variable("Float_" + index.ToString(), 0f);
            varTable_.Add(variable.name, variable);
            varList_.Add(variable);
        }

        public void ChangeVarName(Variable target, string newName)
        {
            varTable_.Remove(target.name);
            varTable_.Add(newName, target);
            target.name = newName;
        }

        public void DeleteVar(Variable target)
        {
            varList_.Remove(target);
            varTable_.Remove(target.name);
        }

#endif
    }
}
