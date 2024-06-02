using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine;

namespace AnimGraph
{
    [Serializable]
    public class Node_Mixer : AnimBehaviourNodeBase
    {
        public int inputCount_;

        public float weight_;

        AnimationMixerPlayable mixer_ => (AnimationMixerPlayable)mainPlayable_;

        public Node_Mixer() : base()
        {
            inputCount_ = 2;
            input_ = new List<NodePin>
            {
                new NodePin
                {
                    index = 0,
                    name = "Base Anim",
                    pinTye = PinType.EAnim,
                },
                new NodePin
                {
                    index = 1,
                    name = "Layer Anim",
                    pinTye = PinType.EAnim,
                },               
                new NodePin
                {
                    index = 2,
                    name = "Weight",
                    pinTye = PinType.EFloat,
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
            weight_ = input_[2].Vaild ? input_[2].GetFloat() : weight_;

            mixer_.SetInputWeight(0, weight_);
            mixer_.SetInputWeight(1, 1f - weight_);
        }

        public override void InitNode(Animator animator, PlayableGraph graph, Dictionary<string, Variable> variables)
        {
            base.InitNode(animator, graph, variables);
        }

        public override void InitConnection(Animator animator, PlayableGraph graph)
        {
            base.InitConnection(animator, graph);
#if UNITY_EDITOR
            string shouldNode ="";
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
            weight_ = input_[2].Vaild ? input_[2].GetFloat() : weight_;
            mixer_.AddInput(input_[0].GetPlayable(), 0, 1 - weight_);
            mixer_.AddInput(input_[1].GetPlayable(), 0, weight_);
        }
    }
}
