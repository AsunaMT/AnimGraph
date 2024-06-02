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

        [SerializeField]
        [SerializeReference]
        public List<DataNodeBase> nodes_ = new List<DataNodeBase>();

        public Dictionary<int, DataNodeBase> id2Node_ = new Dictionary<int, DataNodeBase>();

        public int rootId_ = -1;
#if UNITY_EDITOR
        public Vector3 outPosition_ = Vector3.zero;
#endif
        public PinType retType => valType_;

        public DataNodeBase rootNode_ => id2Node_[rootId_];

        public string Name => name_;

        public bool Valid => rootId_ != -1;

        public Node_Function(PinType retType) : base()
        {
            input_ = null;
            isGraph_ = true;
            name_ = "Function";
            valType_ = retType;
        }

        public Node_Function(string name, PinType retType) : base()
        {
            input_ = null;
            isGraph_ = true;
            name_ = name;
            valType_ = retType;
        }

        public void InitTable()
        {
            if(nodes_ == null)
            {
                nodes_ = new List<DataNodeBase>();
            }
            id2Node_ = new Dictionary<int, DataNodeBase>();
            for (int i = 0; i < nodes_.Count; i++)
            {
                id2Node_.Add(nodes_[i].id_, nodes_[i]);
            }
        }

        public override void InitNode(Animator animator, PlayableGraph graph, Dictionary<string, Variable> variables)
        {
            base.InitNode(animator, graph, variables);
            InitTable();
            nodes_.ForEach(node =>
            {
                node.InitNode(animator, graph, variables);
            });
        }

        public void AddNode(DataNodeBase node)
        {
            if (node == null) return;
            int id;
            if (nodes_.Count == 0)
            {
                id = 0;
            }
            else
            {
                id = nodes_[^1].id_ + 1;
            }
            node.SetId(id);
            id2Node_.Add(id, node);
            nodes_.Add(node);
        }

        public void RemoveNode(DataNodeBase node)
        {
            if (node == null) return;
            nodes_.Remove(node);
            id2Node_.Remove(node.id_);
        }

        public override void Execute()
        {
            if (Valid)
            {
                rootNode_?.Execute();
            }
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
            if (Valid)
            {
                rootNode_?.InitConnection(animator, graph);
            }
            val_ = rootNode_?.val_;
            valType_ = rootNode_== null ? 0 : valType_;
        }

        public GraphNodeBase GetNode(int id)
        {
            return id2Node_[id];
        }

        public void AddConnection(ConnectionInfo con)
        {
            connections_.Add(con);
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
    }
}
