using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

public class BehaviorTreeView : GraphView
{
    private BehaviorTree _tree;
    
    public new class UxmlFactory : UxmlFactory<BehaviorTreeView, GraphView.UxmlTraits> { }
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

    internal void PopulateView(BehaviorTree tree)
    {
        _tree = tree;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);  // 두 개 이상 생성 대비 삭제
        graphViewChanged += OnGraphViewChanged;
        
        _tree.nodes.ForEach(n => CreateNodeView(n)); // 다시 생성
    }
    
    // 뷰 체인지 이벤트 함수
    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(element =>
            {
                if (element is NodeView nodeView)
                    _tree.DeleteNode(nodeView.node);
            });
        }

        return graphViewChange;
    }
    
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

    private void CreateNode(System.Type type)
    {
        Node node = _tree.CreateNode(type);
        CreateNodeView(node);
    }

    private void CreateNodeView(Node node)
    {
        NodeView nodeView = new NodeView(node);
        AddElement(nodeView);
    }
}
