using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SelectorNode의 Random Version
// 하위 노드중 하나를 랜덤으로 진행. (자식의 진행이 끝나야 다시 랜덤부여)
public sealed class OldRandomSelector : MonoBehaviour
{
    private readonly List<OldINode> _children;
    private int _currentIndex = -1;

    public OldRandomSelector(List<OldINode> children)
    {
        _children = children;
    }
    
    // 나중에 실행여부 확인하고 리팩토링 진행예정.
    public OldINode.E_NodeState Evaluate()
    {
        if (_children == null || _children.Count == 0)
            return OldINode.E_NodeState.ENS_Failure;
        
        // ChildNode Running Check
        if (_currentIndex != -1 && _children[_currentIndex].Evaluate() == OldINode.E_NodeState.ENS_Running)
            return OldINode.E_NodeState.ENS_Running;
        
        // Random ChildNode Apply
        int randomIndex = Random.Range(0, _children.Count);
        _currentIndex = randomIndex;
        OldINode.E_NodeState randomChild = _children[_currentIndex].Evaluate();
        
        // ChildNode Running Reset
        if (randomChild != OldINode.E_NodeState.ENS_Running)
            _currentIndex = -1;
        
        return randomChild;
    }
}
