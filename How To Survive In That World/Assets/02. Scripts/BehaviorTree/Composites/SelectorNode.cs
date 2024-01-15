using System;
using System.Collections.Generic;

// 자식의 노드가 Failure 싱테가 아니면 반환하는 노드. (OR, ||)
// 여러 행동중 하나만 실행할 때 용이.
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
                    continue;
                case INode.E_NodeState.ENS_Failure:
                    return INode.E_NodeState.ENS_Failure;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return INode.E_NodeState.ENS_Success;
    }
}
