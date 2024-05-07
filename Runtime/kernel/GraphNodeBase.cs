using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;

namespace AnimGraph
{
    [Serializable]
    public abstract class GraphNodeBase
    {
        public bool isAnim_;

        public bool isGraph_;

        public int id_;

        public List<NodePin> input_;

#if UNITY_EDITOR

        public Vector2 position_ = Vector2.zero;

        public void SetPosition(Vector2 position)
        {
            position_ = position;
        }

#endif

        public void SetId(int id)
        {
            id_ = id;
        }

        public virtual void InitNode(Animator animator, PlayableGraph graph)
        {

        }

        public virtual void InitConnection(Animator animator, PlayableGraph graph)
        {

        }

        public virtual void ConnectTo(GraphNodeBase node, int index)
        {

        }

        public virtual void Execute()
        {

        }
    }


}