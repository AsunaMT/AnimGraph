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
        //标识节点是否是动画节点
        public bool isAnim_;
        //标识节点是否是图节点
        public bool isGraph_;
        //节点在图中的id
        public int id_;
        //输入
        public List<NodePin> input_;
#if UNITY_EDITOR
        //节点在编辑器中的位置
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
        //初始化节点接口，会在AnimGraph运行时初始化时调用
        public abstract void InitNode(Animator animator, PlayableGraph graph);
        //连接节点内部的Playable，在所有动画节点执行完CreatePlayable后调用
        public abstract void InitConnection(Animator animator, PlayableGraph graph);
        //连接节点
        public virtual void ConnectTo(GraphNodeBase node, int port)
        {
            if(node.input_ == null || node.input_.Count <= port)
            {
                return;
            }
            node.input_[port].node = this;
        }
        //执行节点逻辑
        public virtual void Execute()
        {

        }
        //释放节点所用到的需要释放的资源
        public virtual void Dispose()
        {

        }
    }


}