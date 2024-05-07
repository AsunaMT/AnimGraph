using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace AnimGraph.Editor
{
    public class SubGraphView : GraphViewBase
    {
        public override EditorNodeBase RootNode => PoseOutputNode;

        public EditorNode_AnimOutput PoseOutputNode { get; }

        public Node_SubGraph graph => (Node_SubGraph)GraphAsset;

        public Dictionary<GraphNodeBase, EditorNodeBase> runtimeNode2Editor_ = new Dictionary<GraphNodeBase, EditorNodeBase>();
        public SubGraphView(Node_SubGraph graphAsset, VisualElement container)
            : base(graphAsset, container)
        {
            SetupContextMenu();

            // Root node
            PoseOutputNode = new EditorNode_AnimOutput(graphAsset);
            //PoseOutputNode.SetPosition(new Rect(graph.outPosition_, Vector2.one));
            AddElement(PoseOutputNode);


            foreach (var node in graph.animNodes_)
            {
                AddEditorNode(node);
            }
            // Nodes
            /*            var nodeTable = new Dictionary<string, MixerGraphEditorNode>(GraphLayer.Nodes.Count + 1);
                        foreach (var nodeData in GraphLayer.Nodes)
                        {
                            var node = PlayableEditorNodeFactory.CreateNode(GraphAsset, nodeData, false);
                            node.OnDoubleClicked += OnDoubleClickNode;
                            AddElement(node);
                            nodeTable.Add(node.Guid, node);
                        }

                        // Edges
                        if (!string.IsNullOrEmpty(PoseOutputNode.RootPlayableNodeGuid))
                        {
                            var rootPlayableNode = nodeTable[PoseOutputNode.RootPlayableNodeGuid];
                            var edge = PoseOutputNode.PoseInputPort.ConnectTo<FlowingGraphEdge>(rootPlayableNode.OutputPort);
                            AddElement(edge);
                        }*/


        }

        public void AddNode(GraphNodeBase node, Vector2 position)
        {
            Vector2 localMousePosition = contentViewContainer.WorldToLocal(position - window_.position.position);
            node.SetPosition(localMousePosition);
            AddEditorNode(node);
            graph.AddNode(node);
        }

        public void AddEditorNode(GraphNodeBase node)
        {
            Type nodeType = NodeTable.RuntimeNode2Editor[node.GetType()];
            if (nodeType == null) return;
            if (Activator.CreateInstance(nodeType, node, this) is EditorNodeBase editorNode)
            {
                AddElement(editorNode);
                runtimeNode2Editor_.Add(node, editorNode);
            }
        }
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            foreach (var port in ports)
            {
                if (port.node != startPort.node &&
                    port.direction != startPort.direction &&
                    port.portType == startPort.portType)
                {
                    compatiblePorts.Add(port);
                }
            }

            return compatiblePorts;
        }


        private void SetupContextMenu()
        {
            var contextMenu = new ContextualMenuManipulator(evt =>
            {
                // 将菜单绑定到事件
                evt.menu.AppendAction("Create", action =>
                {
                    ShowSearchWindow(action.eventInfo.mousePosition);
                });
            });

            // 添加上下文菜单到GraphView
            this.AddManipulator(contextMenu);
        }

        private void ShowSearchWindow(Vector2 mousePosition)
        {
            var searchWindowProvider = ScriptableObject.CreateInstance<AnimGraphSearchWindowProvider>();
            searchWindowProvider.graphView = this;
            SearchWindow.Open(new SearchWindowContext(mousePosition + window_.position.position), searchWindowProvider);
        }

        public override void AddConnection(EditorNodeBase source, EditorNodeBase target, int index)
        {
            ConnectionInfo con = new ConnectionInfo
            {
                sourceIsAnim = source.node_.isAnim_,
                targetIsAnim = target.node_.isAnim_,
                sourceId = source.node_.id_,
                targetId = target.node_.id_,
                targetPort = index
            };
            graph.AddConnection(con);
        }

        public override void RemoveConnection(int targetId, int index)
        {
            graph.RemoveConnection(targetId, index);
        }
    }

    public class AnimGraphSearchWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        public SubGraphView graphView;
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry>();

            var createGroup = new SearchTreeGroupEntry(new GUIContent("Create"), 0);
            entries.Add(createGroup);
            var animNodeGroup = new SearchTreeGroupEntry(new GUIContent("Anim Node"), 1);
            entries.Add(animNodeGroup);

            Assembly rtAssembly = null;
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                // 过滤出运行时程序集
                if (assembly.GetName().Name.Equals("Assembly-CSharp"))
                {
                    rtAssembly = assembly;
                    break;
                }
            }
            Type[] types = rtAssembly.GetTypes();
            foreach (Type type in types)
            {
                if (!type.IsAbstract && type.IsSubclassOf(typeof(AnimNodeBase)) && !type.GetInterfaces().Contains(typeof(IStateMachinePart)))
                {
                    var nodeEntry = new SearchTreeEntry(new GUIContent(type.Name))
                    {
                        level = 2,
                        userData = type
                    };
                    entries.Add(nodeEntry);
                }
            }
            var dataNodeGroup = new SearchTreeGroupEntry(new GUIContent("Data Node"), 1);
            entries.Add(dataNodeGroup);
            foreach (Type type in types)
            {
                if (!type.IsAbstract && type.IsSubclassOf(typeof(DataNodeBase)) && !type.GetInterfaces().Contains(typeof(IStateMachinePart)))
                {
                    var nodeEntry = new SearchTreeEntry(new GUIContent(type.Name))
                    {
                        level = 2,
                        userData = type
                    };
                    entries.Add(nodeEntry);
                }
            }
            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
        {
            Type type = entry.userData as Type;

            if (type != null)
            {
                object instance = Activator.CreateInstance(type);
                if (instance is GraphNodeBase node)
                {
                    graphView.AddNode(node, context.screenMousePosition);
                    return true;
                }
            }
            return false;
        }
    }
}
