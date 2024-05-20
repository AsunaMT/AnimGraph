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

        public List<Variable> varList_;
        public SubGraphView(Node_SubGraph graphAsset, VisualElement container, List<Variable> varList)
            : base(graphAsset, container)
        {
            varList_ = varList ?? new List<Variable>();
            SetupContextMenu();
            graphAsset.InitTable();
            // Root node
            PoseOutputNode = new EditorNode_AnimOutput(graphAsset);
            //PoseOutputNode.SetPosition(new Rect(graph.outPosition_, Vector2.one));
            AddElement(PoseOutputNode);

            if (graph.animNodes_ != null && graph.animNodes_.Count > 0)
            {
                foreach (var node in graph.animNodes_)
                {
                    AddEditorNode(node, false);
                }
            }
            
            if (graph.dataNodes_ != null && graph.dataNodes_.Count > 0)
            {
                foreach (var node in graph.dataNodes_)
                {
                    AddEditorNode(node, false);
                }
            }

            if (graph.connections_ != null && graph.connections_.Count > 0)
            {
                foreach (var con in graph.connections_)
                {
                    var source = graph.GetNode(con.sourceIsAnim, con.sourceId);
                    var target = graph.GetNode(con.targetIsAnim, con.targetId);
                    var editorSource = runtimeNode2Editor_[source];
                    var editorTarget = runtimeNode2Editor_[target];
                    var edge = editorTarget.input_[con.targetPort].ConnectTo<FlowingGraphEdge>(editorSource.outPut_);
                    AddElement(edge);
                }
            }
            if (graph.rootId_ != -1)
            {
                var rootNode = graph.rootNode_;
                var editorRootNode = runtimeNode2Editor_[rootNode];
                var outEdge = PoseOutputNode.PoseInputPort.ConnectTo<FlowingGraphEdge>(editorRootNode.outPut_);
                AddElement(outEdge);
            }

            foreach(var node in runtimeNode2Editor_.Values)
            {
                node.ActivateConnectAble();
            }
            
        }

        /*        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
                {
                    base.BuildContextualMenu(evt);
                    if (selection.Count > 0) 
                    {
                        evt.menu.AppendAction("Delete", DeleteSelectedNodes, DropdownMenuAction.AlwaysEnabled);
                    }
                }*/

        /*        private void DeleteSelectedNodes(DropdownMenuAction action)
                {
                    foreach(var sel in selection)
                    {
                        if(sel is EditorNodeBase editorNode)
                        {
                            if(editorNode is not EditorNode_AnimOutput)
                            {
                                graph.RemoveNode(editorNode.node_);
                            }
                        }
                    }
                }*/

        public override EventPropagation DeleteSelection()
        {
            foreach (var sel in selection)
            {
                if (sel is EditorNodeBase editorNode)
                {
                    if (editorNode is not EditorNode_AnimOutput)
                    {
                        graph.RemoveNode(editorNode.node_);
                    }
                }
            }
            return base.DeleteSelection();
        }

        public void AddNode(GraphNodeBase node, Vector2 position)
        {
            Vector2 localMousePosition = contentViewContainer.WorldToLocal(position - window_.position.position);
            node.SetPosition(localMousePosition);
            AddEditorNode(node, true);
            graph.AddNode(node);
        }

        public void AddEditorNode(GraphNodeBase node, bool isActive)
        {
            object create;
            if (node.GetType().IsSubclassOf(typeof(OperatorNodeBase)))
            {
                create = new EditorNode_Operator((OperatorNodeBase)node, this);
            }
            else
            {
                Type nodeType = NodeTable.RuntimeNode2Editor[node.GetType()];
                if (nodeType == null) return;
                create = Activator.CreateInstance(nodeType, node, this);
            }
            if (create is EditorNodeBase editorNode)
            {
                AddElement(editorNode);
                runtimeNode2Editor_.Add(node, editorNode);
                if (isActive)
                {
                    editorNode.ActivateConnectAble();
                }
                editorNode.OnDoubleClicked += OnDoubleClickNode;
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
                //evt.menu.AppendAction("Delete", DeleteSelectedNodes);
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
                if (!type.IsAbstract && type.IsSubclassOf(typeof(DataNodeBase)) && !type.GetInterfaces().Contains(typeof(IStateMachinePart))
                    && !type.Equals(typeof(Node_Function)) && !type.Equals(typeof(Node_GetVar)) && !type.Equals(typeof(Node_SetVar)))
                {
                    var nodeEntry = new SearchTreeEntry(new GUIContent(type.Name))
                    {
                        level = 2,
                        userData = type
                    };
                    entries.Add(nodeEntry);
                }
            }
            entries.Add(new SearchTreeEntry(new GUIContent("Function-Bool"))
            {
                level = 2,
                userData = typeof(Node_Function)
            });
            entries.Add(new SearchTreeEntry(new GUIContent("Function-Int"))
            {
                level = 2,
                userData = typeof(Node_Function)
            });
            entries.Add(new SearchTreeEntry(new GUIContent("Function-Float"))
            {
                level = 2,
                userData = typeof(Node_Function)
            });
            foreach (var variable in graphView.varList_)
            {
                entries.Add(new SearchTreeEntry(new GUIContent(string.Format("Get {0}", variable.name)))
                {
                    level = 2,
                    userData = variable
                });
                entries.Add(new SearchTreeEntry(new GUIContent(string.Format("Set {0}", variable.name)))
                {
                    level = 2,
                    userData = variable
                });
            }
             return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
        {
            if (entry.content.text.StartsWith("Get"))
            {
                Variable variable = entry.userData as Variable;
                Node_GetVar getter = new Node_GetVar(variable);
                graphView.AddNode(getter, context.screenMousePosition);
                return true;
            }
            else if (entry.content.text.StartsWith("Set"))
            {
                Variable variable = entry.userData as Variable;
                Node_SetVar setter = new Node_SetVar(variable);
                graphView.AddNode(setter, context.screenMousePosition);
                return true;
            }

            Type type = entry.userData as Type;

            if (type != null)
            {
                if (type.Equals(typeof(Node_Function)))
                {
                    string name = entry.content.text;
                    PinType retType;
                    switch (name[9])
                    {
                        case 'B':
                            retType = PinType.EBool;
                            break;
                        case 'I':
                            retType = PinType.EInt;
                            break;
                        case 'F':
                            retType = PinType.EFloat;
                            break;
                        default:
                            return false;
                    }
                    Node_Function func = new Node_Function(retType);
                    graphView.AddNode(func, context.screenMousePosition);
                    return true;
                }
                else
                {
                    object instance = Activator.CreateInstance(type);
                    if (instance is GraphNodeBase node)
                    {
                        graphView.AddNode(node, context.screenMousePosition);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
