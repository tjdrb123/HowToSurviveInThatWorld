using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviorTreeEditor : EditorWindow
{
    private BehaviorTreeView _treeView;
    private InspectorView _inspectorView;
    
    [MenuItem("BehaviorTree/BehaviorTree Editor")]
    public static void OpenWindow()
    {
        BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviorTreeEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/__Scripts__/__Core__/Behavior Tree/Editor/BehaviorTreeEditor.uxml");
        visualTree.CloneTree(root);
        
        // Allocate StyleSheet 
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/__Scripts__/__Core__/Behavior Tree/Editor/BehaviorTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        _treeView = root.Q<BehaviorTreeView>();
        _inspectorView = root.Q<InspectorView>();
        
        OnSelectionChange();
    }

    // 선택시 변경 이벤트 함수
    private void OnSelectionChange()
    {
        BehaviorTree tree = Selection.activeObject as BehaviorTree;

        if (tree)
        {
            _treeView.PopulateView(tree);
        }
    }
}
