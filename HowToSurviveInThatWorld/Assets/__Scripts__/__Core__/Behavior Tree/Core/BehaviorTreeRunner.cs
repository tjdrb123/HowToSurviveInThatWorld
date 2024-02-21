using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviorTreeRunner : MonoBehaviour
{
    /*===========================================================================================================*/
    
    public BehaviorTree tree;

    private BasicZombieData _basicZombieData;
    
    /*===========================================================================================================*/
    
    void Start()
    {
        _basicZombieData = createBehaviorTreeZombieData();
        tree = tree.Clone();
        tree.Bind(_basicZombieData);
    }
    
    void Update()
    {
        tree.Update();
    }

    BasicZombieData createBehaviorTreeZombieData()
    {
        return BasicZombieData.CreateBasicZombieData(gameObject);
    }
}
