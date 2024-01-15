using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SelectorNode의 Random Version
// 하위 노드중 하나를 랜덤으로 진행. (자식의 진행이 끝나야 다시 랜덤부여)
public sealed class RandomSelector : MonoBehaviour
{
    private readonly List<INode> _children;
    private int _currentIndex = -1;

    public RandomSelector(List<INode> children)
    {
        _children = children;
    }
    
    // 나중에 실행여부 확인하고 리팩토링 진행예정.
    public INode.E_NodeState Evaluate()
    {
        if (_children == null || _children.Count == 0)
            return INode.E_NodeState.ENS_Failure;
        
        // ChildNode Running Check
        if (_currentIndex != -1 && _children[_currentIndex].Evaluate() == INode.E_NodeState.ENS_Running)
            return INode.E_NodeState.ENS_Running;
        
        // Random ChildNode Apply
        int randomIndex = Random.Range(0, _children.Count);
        _currentIndex = randomIndex;
        INode.E_NodeState randomChild = _children[_currentIndex].Evaluate();
        
        // ChildNode Running Reset
        if (randomChild != INode.E_NodeState.ENS_Running)
            _currentIndex = -1;
        
        return randomChild;
    }
}
