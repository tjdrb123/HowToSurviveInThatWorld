using System;

// 자식 노드의 상태를 반전시킨다.
public sealed class OldInverter : OldINode
{
    private readonly OldINode _children;

    public OldInverter(OldINode children)
    {
        _children = children;
    }

    public OldINode.E_NodeState Evaluate()
    {
        if (_children == null)
            return OldINode.E_NodeState.ENS_Failure;

        switch (_children.Evaluate())
        {
            case OldINode.E_NodeState.ENS_Running:
                return OldINode.E_NodeState.ENS_Running;
            case OldINode.E_NodeState.ENS_Success:
                return OldINode.E_NodeState.ENS_Failure;
            case OldINode.E_NodeState.ENS_Failure:
                return OldINode.E_NodeState.ENS_Success;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
