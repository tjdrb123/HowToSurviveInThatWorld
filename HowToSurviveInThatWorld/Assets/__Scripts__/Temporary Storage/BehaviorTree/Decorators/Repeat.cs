using UnityEngine;

// 자식 노드의 상태에 상관없이 지정된 횟수만큼 반복하고 완료 후 Success를 반환.
// 추후 적용과 실험 후 리팩토링 진행예정.
public sealed class OldRepeat : OldINode
{
    private readonly OldINode _children;

    private int _repeatCount;
    private int _currentCount = 0;

    public OldRepeat(OldINode children)
    {
        _children = children;
    }

    public OldINode.E_NodeState Evaluate()
    {
        if (_children == null)
            return OldINode.E_NodeState.ENS_Failure;
        
        // 지정된 횟수만큼 반복
        if (_currentCount < _repeatCount)
        {
            _children.Evaluate();
            _currentCount++;

            return OldINode.E_NodeState.ENS_Running;
        }
        else
        {
            _currentCount = 0;
            return OldINode.E_NodeState.ENS_Success;
        }
    }
}
