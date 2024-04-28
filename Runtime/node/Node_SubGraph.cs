using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace AnimGraph
{
    [Serializable]
    public class Node_SubGraph : AnimBehaviourNodeBase, IGraphNode
    {
        public string name_;

        public List<AnimNodeBase> animNodes_ = new List<AnimNodeBase>();

        public List<DataNodeBase> dataNodes_ = new List<DataNodeBase>();

        public Dictionary<int, int> animId2Index_ = new Dictionary<int, int>();

        public Dictionary<int, int> dataId2Index_ = new Dictionary<int, int>();

        public int rootId_;

        public Vector3 outPosition_ = Vector3.zero;


        public AnimNodeBase rootNode_ => animNodes_[animId2Index_[rootId_]];

        public Node_SubGraph() : base()
        {
            input_ = null;
            isGraph_ = true;
        }

        public void InitTable()
        {
            for(int i = 0; i < animNodes_.Count; i++)
            {
                animId2Index_.Add(animNodes_[i].id_, i);
            }
            for(int i = 0; i < dataNodes_.Count; i++)
            {
                dataId2Index_.Add(dataNodes_[i].id_, i);
            }
        }

        public override void InitNode(Animator animator, PlayableGraph graph)
        {
            base.InitNode(animator, graph);
            animNodes_.ForEach(node =>
            {
                if (!node.isGraph_)
                {
                    node.InitNode(animator, graph);
                }
                node.CreatePlayable(animator, graph);
            });
            
            InitConnection(animator, graph);
        }

        public void AddNode(GraphNodeBase node)
        {
            if (node.isAnim_)
            {
                int id = animNodes_[-1].id_ + 1;
                node.SetId(id);
                animId2Index_.Add(id, animNodes_.Count);
                animNodes_.Add((AnimNodeBase)node);
            }
            else
            {
                int id = dataNodes_[-1].id_ + 1;
                node.SetId(id);
                dataId2Index_.Add(id, dataNodes_.Count);
                dataNodes_.Add((DataNodeBase)node);
            }
        }

        public void RemoveNode(GraphNodeBase node)
        {
            if (node.isAnim_)
            {
                int index = animId2Index_[node.id_];
                animNodes_.RemoveAt(index);
                animId2Index_.Remove(node.id_);
                for(int i = index; i < animNodes_.Count; i++)
                {
                    animId2Index_[animNodes_[i].id_] = i;
                }
            }
            else
            {
                int index = dataId2Index_[node.id_];
                dataNodes_.RemoveAt(index);
                dataId2Index_.Remove(index);
                for(int i = index; i < dataNodes_.Count; i++)
                {
                    dataId2Index_[dataNodes_[i].id_] = i;
                }
            }
        }

        public override void CreatePlayable(Animator animator, PlayableGraph graph)
        {
            base.CreatePlayable(animator, graph);
            animNodes_.ForEach(node =>
            {
                node.CreatePlayable(animator, graph);
            });
        }

        public override void Execute()
        {
            
        }

        public override void InitConnection(Animator animator, PlayableGraph graph)
        {
            base.InitConnection(animator, graph);
            rootNode_.InitConnection(animator, graph);
            outputPlayable_.AddInput(rootNode_.outputPlayable_, 0, 1f);
        }
    }
}