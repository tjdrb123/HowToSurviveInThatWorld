using System;

// 자식 노드의 상태를 반전시킨다.
public sealed class Inverter : INode
{
    private readonly INode _children;

    public Inverter(INode children)
    {
        _children = children;
    }

    public INode.E_NodeState Evaluate()
    {
        if (_children == null)
            return INode.E_NodeState.ENS_Failure;

        switch (_children.Evaluate())
        {
            case INode.E_NodeState.ENS_Running:
                return INode.E_NodeState.ENS_Running;
            case INode.E_NodeState.ENS_Success:
                return INode.E_NodeState.ENS_Failure;
            case INode.E_NodeState.ENS_Failure:
                return INode.E_NodeState.ENS_Success;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
