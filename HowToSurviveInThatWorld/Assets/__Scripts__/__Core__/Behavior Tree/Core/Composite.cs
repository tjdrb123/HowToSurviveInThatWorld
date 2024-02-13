using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 노드간의 다리같은 존재. (AND, OR, RANDOM)
// 여러 자식을 가질 수 있다.
public abstract class Composite : Node
{
    public List<Node> children = new List<Node>();
}
