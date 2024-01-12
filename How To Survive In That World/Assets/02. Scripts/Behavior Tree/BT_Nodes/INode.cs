using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INode
{
    public enum E_NodeState
    {
        ENS_Running,
        ENS_Success,
        ENS_Failure
    }

    public E_NodeState Evaluate();
}
