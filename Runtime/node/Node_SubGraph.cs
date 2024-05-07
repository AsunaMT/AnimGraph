using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

namespace AnimGraph
{
    [Serializable]
    public class Node_SubGraph : AnimBehaviourNodeBase, IGraphNode
    {
        public string name_;

        [SerializeField]
        public List<ConnectionInfo> connections_ = new List<ConnectionInfo>();

        [SerializeField]
        public List<AnimNodeBase> animNodes_ = new List<AnimNodeBase>();

        [SerializeField]
        public List<DataNodeBase> dataNodes_ = new List<DataNodeBase>();

        public Dictionary<int, AnimNodeBase> animId2Node_ = new Dictionary<int, AnimNodeBase>();

        public Dictionary<int, DataNodeBase> dataId2Node_ = new Dictionary<int, DataNodeBase>();

        public int rootId_ = -1;

        public Vector2 outPosition_ = Vector2.zero;


        public AnimNodeBase rootNode_ => animNodes_[rootId_];

        public Node_SubGraph() : base()
        {
            input_ = null;
            isGraph_ = true;
        }

        public void InitTable()
        {
            for(int i = 0; i < animNodes_.Count; i++)
            {
                animId2Node_.Add(animNodes_[i].id_, animNodes_[i]);
            }
            for(int i = 0; i < dataNodes_.Count; i++)
            {
                dataId2Node_.Add(dataNodes_[i].id_, dataNodes_[i]);
            }
        }

        public override void InitNode(Animator animator, PlayableGraph graph)
        {
            base.InitNode(animator, graph);
            InitTable();
            animNodes_.ForEach(node =>
            {
                node.InitNode(animator, graph);
            });
        }

        public void AddNode(GraphNodeBase node)
        {
            if (node.isAnim_)
            {
                int id;
                if (animNodes_.Count == 0)
                {
                    id = 0;
                }
                else
                {
                    id = animNodes_[^1].id_ + 1;
                }
                node.SetId(id);
                animId2Node_.Add(id, (AnimNodeBase)node);
                animNodes_.Add((AnimNodeBase)node);
            }
            else
            {
                int id;
                if (dataNodes_.Count == 0)
                {
                    id = 0;
                }
                else
                {
                    id = dataNodes_[^1].id_ + 1;
                }
                node.SetId(id);
                dataId2Node_.Add(id, (DataNodeBase)node);
                dataNodes_.Add((DataNodeBase)node);
            }
        }

        public void RemoveNode(GraphNodeBase node)
        {
            if (node.isAnim_)
            {
                animNodes_.Remove((AnimNodeBase)node);
                animId2Node_.Remove(node.id_);
            }
            else
            {
                dataNodes_.Remove((DataNodeBase)node);
                dataId2Node_.Remove(node.id_);
            }
        }

        public void AddConnection(ConnectionInfo connectionInfo)
        {
            connections_.Add(connectionInfo);
        }

        public void RemoveConnection(int targetId, int targetPort)
        {
            var connToRemove = connections_.FirstOrDefault(conn =>
                    conn.targetId == targetId &&
                    conn.targetPort == targetPort
                );

            if (connToRemove != null)
            {
                connections_.Remove(connToRemove);
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
            foreach(var connection in connections_)
            {
                GraphNodeBase source;
                GraphNodeBase target;
                if (connection.sourceIsAnim)
                {
                    source = animId2Node_[connection.sourceId];
                } else
                {
                    source = dataId2Node_[connection.sourceId];
                }
                if (connection.targetIsAnim)
                {
                    target = animId2Node_[connection.targetId];
                }
                else
                {
                    target = dataId2Node_[connection.targetId];
                }
                target.input_[connection.targetPort].node = source;
            }

            rootNode_.InitConnection(animator, graph);
            outputPlayable_.AddInput(rootNode_.outputPlayable_, 0, 1f);
        }
    }
}