using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    /*===========================================================================================================*/

    public Action<NodeView> OnNodeSelected;
    public Node node;

    public Port inputPort;
    public Port outputPort;
    
    /*===========================================================================================================*/

    public NodeView(Node node)
    {
        this.node = node;
        this.title = node.name;
        this.viewDataKey = node.guid;

        style.left = node.position.x;
        style.top = node.position.y;
        
        CreateInputPort();
        CreateOutputPort();
    }
    
    private void CreateInputPort()
    {
        if (node is LeafAction)
        {
            inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is Composite)
        {
            inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is Decorator)
        {
            inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        }

        if (inputPort != null)
        {
            inputPort.portName = "";
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
            outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
        }
        else if (node is Decorator)
        {
            outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        }
        
        if (outputPort != null)
        {
            outputPort.portName = "";
            outputContainer.Add(outputPort);
        }
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        
        node.position.x = newPos.xMin;
        node.position.y = newPos.yMin;
    }

    public override void OnSelected()
    {
        base.OnSelected();
        OnNodeSelected?.Invoke(this);
    }
    
    
}
