using System;
using System.Collections.Generic;

// 자식의 노드가 Failure 상태를 반활할 때 까지 진행하는 노드. (AND, &&)
// 여러 행동중 순서대로 진행할 때 용이.
public sealed class SelectorNode : INode
{
    private readonly List<INode> _children;

    public SelectorNode(List<INode> children)
    {
        _children = children;
    }

    public INode.E_NodeState Evaluate()
    {
        if (_children == null || _children.Count == 0)
            return INode.E_NodeState.ENS_Failure;

        foreach (var child in _children)
        {
            switch (child.Evaluate())
            {
                case INode.E_NodeState.ENS_Running:
                    return INode.E_NodeState.ENS_Running;
                case INode.E_NodeState.ENS_Success:
                    return INode.E_NodeState.ENS_Success;
                case INode.E_NodeState.ENS_Failure:
                    continue;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return INode.E_NodeState.ENS_Failure;
    }
}
