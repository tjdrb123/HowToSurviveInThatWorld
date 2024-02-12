using UnityEngine;

// 자식 노드가 실패를 반환할 때까지 계속해서 재검사 하는 데코레이터 노드.
// 실패를 반환하면 성공을 반환한다.
public sealed class OldUntilFail : OldINode
{
    private readonly OldINode _children;

    public OldUntilFail(OldINode children)
    {
        _children = children;
    }

    public OldINode.E_NodeState Evaluate()
    {
        if (_children == null)
            return OldINode.E_NodeState.ENS_Failure;

        OldINode.E_NodeState children = _children.Evaluate();

        if (children == OldINode.E_NodeState.ENS_Failure)
        {
            return OldINode.E_NodeState.ENS_Success;
        }
        
        return OldINode.E_NodeState.ENS_Running;
    }
}
