using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 자식 노드를 꾸며준다.
// 하나의 자식만 가질 수 있다.
public abstract class Decorator : Node
{
    public Node child;
    
    public override Node Clone()
    {
        Decorator node = Instantiate(this);
        node.child = child.Clone();
        return node;
    }
}
