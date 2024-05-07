using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace AnimGraph
{
    public class Node_Function : DataNodeBase, IGraphNode
    {

        public string name_;

        public List<ConnectionInfo> connections_ = new List<ConnectionInfo>();

        public List<DataNodeBase> nodes_ = new List<DataNodeBase>();

        public Dictionary<int, DataNodeBase> id2Node_ = new Dictionary<int, DataNodeBase>();

        public int rootId_;

        public Vector3 outPosition_ = Vector3.zero;

        public DataNodeBase rootNode_ => id2Node_[rootId_];

        public Node_Function() : base()
        {
            input_ = null;
            isGraph_ = true;
        }

        public void InitTable()
        {
            for (int i = 0; i < nodes_.Count; i++)
            {
                id2Node_.Add(nodes_[i].id_, nodes_[i]);
            }
        }

        public override void InitNode(Animator animator, PlayableGraph graph)
        {
            base.InitNode(animator, graph);
            InitTable();
            nodes_.ForEach(node =>
            {
                node.InitNode(animator, graph);
            });
        }

        public void AddNode(DataNodeBase node)
        {
            int id = nodes_[^1].id_ + 1;
            node.SetId(id);
            id2Node_.Add(id, node);
            nodes_.Add(node);
        }

        public void RemoveNode(DataNodeBase node)
        {
            nodes_.Remove(node);
            id2Node_.Remove(node.id_);
        }

        public override void Execute()
        {

        }

        public override void InitConnection(Animator animator, PlayableGraph graph)
        {
            base.InitConnection(animator, graph);
            foreach (var connection in connections_)
            {
                DataNodeBase source = nodes_[connection.sourceId];
                DataNodeBase target = nodes_[connection.targetId];
                target.input_[connection.targetPort].node = source;
            }
            rootNode_.InitConnection(animator, graph);
        }
    }
}
