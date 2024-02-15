using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Linq;

public class BehaviorTreeView : GraphView
{
    /*===========================================================================================================*/
    public Action<NodeView> OnNodeSelected;
    private BehaviorTree _tree;
    
    /*===========================================================================================================*/
    
    public new class UxmlFactory : UxmlFactory<BehaviorTreeView, GraphView.UxmlTraits> { }
    
    /*===========================================================================================================*/
    public BehaviorTreeView()
    {
        Insert(0, new GridBackground()); // 백그라운드 드로우
        
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/__Scripts__/__Core__/Behavior Tree/Editor/BehaviorTreeEditor.uss");
        styleSheets.Add(styleSheet); // 스타일 시트 직접참조
    }
    
    /*===========================================================================================================*/
    
    // GUID(고유 식별자)를 사용하여 파라미터에 해당하는 NodeView 반환.
    NodeView FindNodeView(Node node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }

    // 파라미터 tree 객체를 사용해 그래프뷰를 채운다.
    internal void PopulateView(BehaviorTree tree)
    {
        _tree = tree;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);  // 두 개 이상 생성 대비 삭제
        graphViewChanged += OnGraphViewChanged;

        // RootNode 생성, 하나의 RootNode 보장
        if (tree.rootNode == null)
        {
            tree.rootNode = tree.CreateNode(typeof(Root)) as Root; // 생성후 트리의 루트로 설정. (다운캐스팅)(.GetType() : 실제 자신을 반환)
            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }
        
        // Creates Node View
        _tree.nodes.ForEach(n => CreateNodeView(n)); // 다시 생성
        
        // 각 노드에 대해 자식 노드를 얻고, 각 노드에 대한 연결(Edge) 생성 후 보모-자식 노드의 출입력 포트 연결 후 그래프 뷰에 추가.
        // Create Edges
        _tree.nodes.ForEach(n =>
        {
            var children = tree.GetChildren(n);
            children.ForEach(c =>
            {
                NodeView parentView = FindNodeView(n);
                NodeView childView = FindNodeView(c);

                Edge edge = parentView.outputPort.ConnectTo(childView.inputPort);
                AddElement(edge);
            });
        });
    }
    
    /*===========================================================================================================*/
    
    // 노드 입출력 호환 검사
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList()
            .Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
    }
    
    /*===========================================================================================================*/
    
    // 뷰 체인지 이벤트 함수
    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(element =>
            {
                if (element is NodeView nodeView)
                    _tree.DeleteNode(nodeView.node);
                
                // 연결된 노드 자식 삭제
                if (element is Edge edge)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    _tree.RemoveChild(parentView.node, childView.node);
                }
            });
        }
        
        // 생성된 Edge가 있다면, 각 Edge에 대한 연결된 노드 간의 부모-자식 관계 추가
        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                _tree.AddChild(parentView.node, childView.node);
            });
        }
        
        return graphViewChange;
    }
    
    /*===========================================================================================================*/
    
    // 메뉴 재정의
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        {
            var types = TypeCache.GetTypesDerivedFrom<LeafAction>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (t) => CreateNode(type));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<Composite>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (t) => CreateNode(type));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<Decorator>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (t) => CreateNode(type));
            }
        }
    }
    
    /*===========================================================================================================*/

    private void CreateNode(System.Type type)
    {
        Node node = _tree.CreateNode(type);
        CreateNodeView(node);
    }

    private void CreateNodeView(Node node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
    }
    
    /*===========================================================================================================*/
}
