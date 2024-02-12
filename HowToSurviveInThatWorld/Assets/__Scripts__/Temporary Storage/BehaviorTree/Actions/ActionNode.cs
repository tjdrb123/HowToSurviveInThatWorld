using System;

// 실제로 어떠한 행위를 하는 노드. (Leaf Node 라고도 불림)
// Func() 델리게이트를 통해 행위를 전달받아서 실행.
public sealed class OldActionNode : OldINode
{
    private readonly Func<OldINode.E_NodeState> _onUpdate;

    public OldActionNode(Func<OldINode.E_NodeState> onUpdate)
    {
        _onUpdate = onUpdate;
    }

    public OldINode.E_NodeState Evaluate() => _onUpdate?.Invoke() ?? OldINode.E_NodeState.ENS_Failure;
}
