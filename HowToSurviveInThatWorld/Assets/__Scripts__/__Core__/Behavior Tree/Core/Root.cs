using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 부모가 없는 유일한 노드 & 시작점.
// 하나의 자식만 가질 수 있다.
public class Root : Node
{
    public Node child;
    
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override E_NodeState OnUpdate()
    {
        return child.Update();
    }

    public override Node Clone()
    {
        Root node = Instantiate(this);
        node.child = child.Clone();
        return node;
    }
}
