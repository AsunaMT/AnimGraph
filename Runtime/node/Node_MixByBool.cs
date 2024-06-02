using AnimGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine;

namespace AnimGraph
{
    [Serializable]
    public class Node_MixByBool : AnimBehaviourNodeBase
    {
        public int inputCount_;

        public bool control_;

        [NonSerialized]
        private bool old_;

        AnimationMixerPlayable mixer_ => (AnimationMixerPlayable)mainPlayable_;

        public Node_MixByBool() : base()
        {
            inputCount_ = 2;
            input_ = new List<NodePin>
            {
                new NodePin
                {
                    index = 0,
                    name = "True Anim",
                    pinTye = PinType.EAnim,
                },
                new NodePin
                {
                    index = 1,
                    name = "False Anim",
                    pinTye = PinType.EAnim,
                },
                new NodePin
                {
                    index = 2,
                    name = "Bool",
                    pinTye = PinType.EBool,
                },
            };
        }

        public override void CreatePlayable(Animator animator, PlayableGraph graph)
        {
            base.CreatePlayable(animator, graph);
            mainPlayable_ = AnimationMixerPlayable.Create(graph);
            outputPlayable_.AddInput(mainPlayable_, 0, 1f);
        }

        public override void Execute()
        {
            control_ = input_[2].Vaild ? input_[2].GetBool() : true;
            if (old_ != control_)
            {
                Refresh();
            }
            old_ = control_;
        }

        void Refresh()
        {
            if (control_)
            {
                mixer_.SetInputWeight(0, 1f);
                mixer_.SetInputWeight(1, 0f);

                input_[0].GetAnim().SetTime(0f);
                input_[0].GetAnim().Play();

                input_[1].GetAnim().Pause();
            }
            else
            {
                mixer_.SetInputWeight(0, 0f);
                mixer_.SetInputWeight(1, 1f);

                input_[0].GetAnim().Pause();

                input_[1].GetAnim().SetTime(0f);
                input_[1].GetAnim().Play();
            }
        }

        public override void Play()
        {
            control_ = input_[2].Vaild ? input_[2].GetBool() : true;
            Refresh();
            base.Play();
        }

        public override void Pause()
        {
            base.Pause();
        }

        public override void SetTime(float time)
        {
            base.SetTime(time);
        }

        public override void InitNode(Animator animator, PlayableGraph graph, Dictionary<string, Variable> variables)
        {
            base.InitNode(animator, graph, variables);
        }

        public override void InitConnection(Animator animator, PlayableGraph graph)
        {
            base.InitConnection(animator, graph);
#if UNITY_EDITOR
            string shouldNode = "";
            int index = 0;
            bool error = false;
            if (error = !input_[0].Vaild || !input_[0].node.isAnim_)
            {
                index = 0;
                shouldNode = "AnimNode";
            }
            else if (error = !input_[1].Vaild || !input_[1].node.isAnim_)
            {
                index = 1;
                shouldNode = "AnimNode";
            }
            else if (error = input_[2].Vaild && input_[2].node.isAnim_)
            {
                index = 2;
                shouldNode = "DataNode";
            }
            if (error)
            {
                Debug.LogError(string.Format("input {0} of MixerNode(id = {1}) should be {2}!", index, id_, shouldNode));
                return;
            }
#endif
            input_[0].node.InitConnection(animator, graph);
            input_[1].node.InitConnection(animator, graph);
            control_ = input_[2].Vaild ? input_[2].GetBool() : true;
            float weight = control_ ? 1f : 0f;
            mixer_.AddInput(input_[0].GetPlayable(), 0, 1 - weight);
            mixer_.AddInput(input_[1].GetPlayable(), 0, weight);
            old_ = !control_;
        }
    }
}
