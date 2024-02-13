using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BehaviorTree : ScriptableObject
{
    public Node rootNode;
    public Node.E_NodeState treeState = Node.E_NodeState.Running;

    public Node.E_NodeState Update()
    {
        if (rootNode.state == Node.E_NodeState.Running)
        {
            treeState = rootNode.Update();
        }
        
        return treeState;
    }
}
