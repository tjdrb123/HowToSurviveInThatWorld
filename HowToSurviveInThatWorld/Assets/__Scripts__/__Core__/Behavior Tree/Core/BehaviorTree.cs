using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

[CreateAssetMenu()]
public class BehaviorTree : ScriptableObject
{
    /*===========================================================================================================*/

    public Node rootNode;
    public E_NodeState treeState = E_NodeState.Running;
    public List<Node> nodes = new List<Node>();
    public DataContext dataContext = new DataContext();

    /*===========================================================================================================*/

    public E_NodeState Update()
    {
        if (rootNode.state == E_NodeState.Running)
        {
            treeState = rootNode.Update();
        }

        return treeState;
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

    public void Traverse(Node node, System.Action<Node> visitor)
    {
        if (node)
        {
            visitor.Invoke(node);
            var children = GetChildren(node);
            children.ForEach((n) => Traverse(n, visitor));
        }
    }

    public BehaviorTree Clone()
    {
        BehaviorTree tree = Instantiate(this);
        tree.rootNode = tree.rootNode.Clone();
        tree.nodes = new List<Node>();

        Traverse(tree.rootNode, (n) => { tree.nodes.Add(n); });

        return tree;
    }

    // 모든 노드에 정보를 바인딩
    public void Bind(BasicZombieData zombieData)
    {
        Traverse(rootNode, node =>
        {
            node.zombieData = zombieData;
            node.dataContext = dataContext;
        });
    }



    #region MyRegion

#if UNITY_EDITOR

    // 노드 객체 생성
    public Node CreateNode(System.Type type)
    {
        Node node = ScriptableObject.CreateInstance(type) as Node;

        if (node == null)
            Debug.LogError("node CreateInstance failed");

        node.name = type.Name;
        node.guid = GUID.Generate().ToString();

        Undo.RecordObject(this, "Behavior Tree (CreateNode)"); // 변경사항 기록(되돌리기 위해)
        nodes.Add(node);

        if (!Application.isPlaying)
            AssetDatabase.AddObjectToAsset(node, this);

        Undo.RegisterCreatedObjectUndo(node, "BBehavior Tree (CreateNode)"); // 새로 생성된 객체에 대한 Undo 작업 등록.

        AssetDatabase.SaveAssets();

        return node;
    }

    // 노드 삭제
    public void DeleteNode(Node node)
    {
        Undo.RecordObject(this, "Behavior Tree (DeleteNode)");
        nodes.Remove(node);

        // AssetDatabase.RemoveObjectFromAsset(node); Undo를 위해 즉시 제거 기능을 주석처리
        Undo.DestroyObjectImmediate(node);

        AssetDatabase.SaveAssets();
    }

    // 입력 부모 타입에 대한 자식 추가
    public void AddChild(Node parent, Node child)
    {
        Root root = parent as Root;
        if (root)
        {
            Undo.RecordObject(root, "Behavior Tree (AddChild)");
            root.child = child;
            EditorUtility.SetDirty(root);
        }

        Decorator decorator = parent as Decorator;
        if (decorator)
        {
            Undo.RecordObject(decorator, "Behavior Tree (AddChild)");
            decorator.child = child;
            EditorUtility.SetDirty(decorator);
        }

        Composite composite = parent as Composite;
        if (composite)
        {
            Undo.RecordObject(composite, "Behavior Tree (AddChild)");
            composite.children.Add(child);
            EditorUtility.SetDirty(composite);
        }
    }

    // 입력 부모 타입에 대한 자식 제거
    public void RemoveChild(Node parent, Node child)
    {
        Root root = parent as Root;
        if (root)
        {
            Undo.RecordObject(root, "Behavior Tree (RemoveChild)");
            root.child = null;
            EditorUtility.SetDirty(root);
        }

        Decorator decorator = parent as Decorator;
        if (decorator)
        {
            Undo.RecordObject(decorator, "Behavior Tree (RemoveChild)");
            decorator.child = null;
            EditorUtility.SetDirty(decorator);
        }

        Composite composite = parent as Composite;
        if (composite)
        {
            Undo.RecordObject(composite, "Behavior Tree (RemoveChild)");
            composite.children.Remove(child);
            EditorUtility.SetDirty(composite);
        }
    }

#endif

    #endregion

}
