using UnityEngine;

// 자식 노드의 상태에 상관없이 지정된 횟수만큼 반복하고 완료 후 Success를 반환.
// 추후 적용 후 리팩토링 진행예정.
public sealed class Repeat : INode
{
    private readonly INode _children;

    private int _repeatCount;
    private int _currentCount = 0;

    public Repeat(INode children)
    {
        _children = children;
    }

    public INode.E_NodeState Evaluate()
    {
        if (_children == null)
            return INode.E_NodeState.ENS_Failure;
        
        // 지정된 횟수만큼 반복
        if (_currentCount < _repeatCount)
        {
            _currentCount++;

            return INode.E_NodeState.ENS_Running;
        }
        else
        {
            _currentCount = 0;
            return INode.E_NodeState.ENS_Success;
        }
    }
}
