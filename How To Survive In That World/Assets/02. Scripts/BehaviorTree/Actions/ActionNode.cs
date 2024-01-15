using System;

// 실제로 어떠한 행위를 하는 노드. (Leaf Node 라고도 불림)
// Func() 델리게이트를 통해 행위를 전달받아서 실행.
public sealed class ActionNode : INode
{
    private readonly Func<INode.E_NodeState> _onUpdate;

    public ActionNode(Func<INode.E_NodeState> onUpdate)
    {
        _onUpdate = onUpdate;
    }

    public INode.E_NodeState Evaluate() => _onUpdate?.Invoke() ?? INode.E_NodeState.ENS_Failure;
}
