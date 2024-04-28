using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace AnimGraph.Editor
{
    public class GraphPort : Port
    {
        public EditorNodeBase OwnerNode => (EditorNodeBase)node;

        // public GraphNode ConnectedNode
        // {
        //     get
        //     {
        //         if (!connected) return null;
        //
        //         return (GraphNode)(direction == Direction.Input
        //             ? connections.First().output.node
        //             : connections.First().input.node);
        //     }
        // }

        public event Action<Edge> OnConnected;

        public event Action<Edge> OnDisconnected;


        protected GraphPort(Direction portDirection, Type portType)
            : base(Orientation.Horizontal, portDirection, Capacity.Single, portType)
        {

        }


        public override void Connect(Edge edge)
        {
            base.Connect(edge);
            OnConnected?.Invoke(edge);
        }

        public override void Disconnect(Edge edge)
        {
            base.Disconnect(edge);
            OnDisconnected?.Invoke(edge);
        }


        public static GraphPort Create<TEdge>(Direction direction, Type portType)
            where TEdge : Edge, new()
        {
            var connectorListener = new EdgeConnectorListener();
            var port = new GraphPort(direction, portType)
            {
                m_EdgeConnector = new EdgeConnector<TEdge>(connectorListener),
            };
            port.AddManipulator(port.m_EdgeConnector);
            return port;
        }
    }
}
