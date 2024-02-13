using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu()]
public class BehaviorTree : ScriptableObject
{
    public Node rootNode;
    public Node.E_NodeState treeState = Node.E_NodeState.Running;
    public List<Node> nodes = new List<Node>();

    public Node.E_NodeState Update()
    {
        if (rootNode.state == Node.E_NodeState.Running)
        {
            treeState = rootNode.Update();
        }
        
        return treeState;
    }

    // 노드 객체 생성
    public Node CreateNode(System.Type type)
    {
        Node node = ScriptableObject.CreateInstance(type) as Node;
        
        if (node == null)
            Debug.LogError("node CreateInstance failed");

        node.name = type.Name;
        node.guid = GUID.Generate().ToString();
        nodes.Add(node);
        
        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();

        return node;
    }

    // 노드 삭제
    public void DeleteNode(Node node)
    {
        nodes.Remove(node);
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }
}
