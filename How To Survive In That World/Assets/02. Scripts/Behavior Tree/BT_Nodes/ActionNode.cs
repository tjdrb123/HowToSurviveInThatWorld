using System;

public sealed class ActionNode : INode
{
    private readonly Func<INode.E_NodeState> _onUpdate;

    public ActionNode(Func<INode.E_NodeState> onUpdate)
    {
        _onUpdate = onUpdate;
    }

    public INode.E_NodeState Evaluate() => _onUpdate?.Invoke() ?? INode.E_NodeState.ENS_Failure;
}
