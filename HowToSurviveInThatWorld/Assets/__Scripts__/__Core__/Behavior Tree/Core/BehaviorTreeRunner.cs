using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeRunner : MonoBehaviour
{
    private BehaviorTree _tree;
    
    void Start()
    {
        _tree = ScriptableObject.CreateInstance<BehaviorTree>();

        var log1 = ScriptableObject.CreateInstance<TestDebugLogNode>();
        log1.Test = "Test1";

        var stop1 = ScriptableObject.CreateInstance<WaitNode>();
        
        var log2 = ScriptableObject.CreateInstance<TestDebugLogNode>();
        log2.Test = "Test2";
        
        var stop2 = ScriptableObject.CreateInstance<WaitNode>();
        
        var log3 = ScriptableObject.CreateInstance<TestDebugLogNode>();
        log3.Test = "Test3";
        
        var stop3 = ScriptableObject.CreateInstance<WaitNode>();

        var sequence = ScriptableObject.CreateInstance<SequenceNode>();
        sequence.children.Add(log1);
        sequence.children.Add(stop1);
        sequence.children.Add(log2);
        sequence.children.Add(stop2);
        sequence.children.Add(log3);
        sequence.children.Add(stop3);

        var loop = ScriptableObject.CreateInstance<RepeatNode>();
        loop.child = sequence;

        _tree.rootNode = loop;
    }
    
    void Update()
    {
        _tree.Update();
    }
}
