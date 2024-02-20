using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviorTreeRunner : MonoBehaviour
{
    /*===========================================================================================================*/
    
    public BehaviorTree tree;
    
    /*===========================================================================================================*/
    
    void Start()
    {
        tree = tree.Clone();
        tree.Bind(GetComponent<NavMeshAgent>());
    }
    
    void Update()
    {
        tree.Update();
    }
}
