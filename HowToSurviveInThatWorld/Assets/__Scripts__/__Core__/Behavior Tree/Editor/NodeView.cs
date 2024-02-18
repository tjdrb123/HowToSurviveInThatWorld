using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    /*===========================================================================================================*/

    public Action<NodeView> OnNodeSelected;
    public Node node;

    public Port inputPort;
    public Port outputPort;
    
    /*===========================================================================================================*/

    // NodeView.UXML 을 클래스에 연결하기 위해 두번째 생성자 호출(base)
    public NodeView(Node node) : base("Assets/__Scripts__/__Core__/Behavior Tree/UI Builders/NodeView.uxml")
    {
        this.node = node;
        this.title = node.name;
        this.viewDataKey = node.guid;

        style.left = node.position.x;
        style.top = node.position.y;
        
        CreateInputPort();
        CreateOutputPort();
        SetupClasses();
    }

    // 노드 타입에 따라 CSS클래스 추가. 시각적 스타일을 타입별로 구분 
    private void SetupClasses()
    {
        if (node is LeafAction)
        {
            AddToClassList("action");
        }
        else if (node is Composite)
        {
            AddToClassList("composite");
        }
        else if (node is Decorator)
        {
            AddToClassList("decorator");
        }
        else if (node is Root)
        {
            AddToClassList("root");
        }
    }
    
    private void CreateInputPort()
    {
        if (node is LeafAction)
        {
            inputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is Composite)
        {
            inputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is Decorator)
        {
            inputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is Root)
        {
            
        }

        if (inputPort != null)
        {
            inputPort.portName = "";
            inputPort.style.flexDirection = FlexDirection.Column;   // 도트 열과 동일하게 설정
            inputContainer.Add(inputPort);
        }
    }

    private void CreateOutputPort()
    {
        if (node is LeafAction)
        {
            
        }
        else if (node is Composite)
        {
            outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
        }
        else if (node is Decorator)
        {
            outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        }
        else if (node is Root)
        {
            outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        }
        
        if (outputPort != null)
        {
            outputPort.portName = "";
            outputPort.style.flexDirection = FlexDirection.ColumnReverse; // 열 역방향 (input, output 단자 위치 보정, uss 수정으로는 방법이 없었음) 
            outputContainer.Add(outputPort);
        }
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        
        Undo.RecordObject(node, "Behavior Tree (Set Position)"); // 노드 위치 변경에 대한 "실행 취소" 작업 기록
        
        node.position.x = newPos.xMin;
        node.position.y = newPos.yMin;
        
        EditorUtility.SetDirty(node); // 노드가 변경됨을 에디터에 전달(저장)
    }

    public override void OnSelected()
    {
        base.OnSelected();
        OnNodeSelected?.Invoke(this);
    }
    
    // 에디터상 수평 위치 순서에 따라 노드 순서 변경 메서드
    public void SortChildren()
    {
        Composite composite = node as Composite;
        
        if (composite)
            composite.children.Sort(SortByHorizontalPosition);
    }

    private int SortByHorizontalPosition(Node left, Node right)
    {
        return left.position.x < right.position.x ? -1 : 1;
    }
    
}
