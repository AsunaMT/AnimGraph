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
        public bool isAnim_ { get; protected set; }
        
        public bool isGraph_ { get; protected set; }

        public int id_ { get; protected set; }

        public List<NodePin> input_ { get; protected set; }

#if UNITY_EDITOR

        public Vector3 position_ { get; protected set; }

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