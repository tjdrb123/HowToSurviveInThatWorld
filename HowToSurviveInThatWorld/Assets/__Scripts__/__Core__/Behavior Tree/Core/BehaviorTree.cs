using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu()]
public class BehaviorTree : ScriptableObject
{
    /*===========================================================================================================*/
    
    public Node rootNode;
    public Node.E_NodeState treeState = Node.E_NodeState.Running;
    public List<Node> nodes = new List<Node>();

    /*===========================================================================================================*/
    
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

    // 입력 부모 타입에 대한 자식 추가
    public void AddChild(Node parent, Node child)
    {
        Root root = parent as Root;
        if (root)
            root.child = child;
        
        Decorator decorator = parent as Decorator;
        if (decorator)
            decorator.child = child;
        
        Composite composite = parent as Composite;
        if (composite)
            composite.children.Add(child);
    }

    // 입력 부모 타입에 대한 자식 제거
    public void RemoveChild(Node parent, Node child)
    {
        Root root = parent as Root;
        if (root)
            root.child = null;
        
        Decorator decorator = parent as Decorator;
        if (decorator)
            decorator.child = null;
        
        Composite composite = parent as Composite;
        if (composite)
            composite.children.Remove(child);
    }

    // 입력 부모에 대한 자식 리스트 반환
    public List<Node> GetChildren(Node parent)
    {
        List<Node> children = new List<Node>();

        Root root = parent as Root;
        if (root && root.child != null)
            children.Add(root.child);
        
        Decorator decorator = parent as Decorator;
        if (decorator && decorator.child != null)
            children.Add(decorator.child);
        
        Composite composite = parent as Composite;
        if (composite)
            return composite.children;

        return children;
    }

    public BehaviorTree Clone()
    {
        BehaviorTree tree = Instantiate(this);
        tree.rootNode = tree.rootNode.Clone();
        return tree;
    }
}
