using System;
using System.Collections.Generic;

// 자식의 노드가 Failure 싱테가 아니면 반환하는 노드. (OR, ||)
// 여러 행동중 하나만 실행할 때 용이.
public sealed class OldSequenceNode : OldINode
{
    private readonly List<OldINode> _children;

    public OldSequenceNode(List<OldINode> children)
    {
        _children = children;
    }

    public OldINode.E_NodeState Evaluate()
    {
        if (_children == null || _children.Count == 0)
            return OldINode.E_NodeState.ENS_Failure;

        foreach (var child in _children)
        {
            switch (child.Evaluate())
            {
                case OldINode.E_NodeState.ENS_Running:
                    return OldINode.E_NodeState.ENS_Running;
                case OldINode.E_NodeState.ENS_Success:
                    continue;
                case OldINode.E_NodeState.ENS_Failure:
                    return OldINode.E_NodeState.ENS_Failure;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        return OldINode.E_NodeState.ENS_Success;
    }
}
