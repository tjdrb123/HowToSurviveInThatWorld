using System;

// 자식의 상태에 상관없이 항상 성공을 반환한다.
// 고장이 예상되는 분기의 테스트 처리용.
// 반대는 Inverter를 이용해 처리.
public sealed class OldSucceed : OldINode
{
    private readonly OldINode _children;

    public OldSucceed(OldINode children)
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
                return OldINode.E_NodeState.ENS_Success;
            case OldINode.E_NodeState.ENS_Success:
                return OldINode.E_NodeState.ENS_Success;
            case OldINode.E_NodeState.ENS_Failure:
                return OldINode.E_NodeState.ENS_Success;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
