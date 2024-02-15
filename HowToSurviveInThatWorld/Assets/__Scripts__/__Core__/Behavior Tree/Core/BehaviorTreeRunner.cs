using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeRunner : MonoBehaviour
{
    /*===========================================================================================================*/
    
    public BehaviorTree tree;
    
    /*===========================================================================================================*/
    
    void Start()
    {
        tree = tree.Clone();
    }
    
    void Update()
    {
        tree.Update();
    }
}
