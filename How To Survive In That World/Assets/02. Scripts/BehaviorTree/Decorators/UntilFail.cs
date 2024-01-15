using UnityEngine;

// 자식 노드가 실패를 반환할 때까지 계속해서 재검사 하는 데코레이터 노드.
// 실패를 반환하면 성공을 반환한다.
public sealed class UntilFail : INode
{
    private readonly INode _children;

    public UntilFail(INode children)
    {
        _children = children;
    }

    public INode.E_NodeState Evaluate()
    {
        if (_children == null)
            return INode.E_NodeState.ENS_Failure;

        INode.E_NodeState children = _children.Evaluate();

        if (children == INode.E_NodeState.ENS_Failure)
        {
            return INode.E_NodeState.ENS_Success;
        }
        
        return INode.E_NodeState.ENS_Running;
    }
}
