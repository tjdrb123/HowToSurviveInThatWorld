using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;

public class BehaviorTreeEditor : EditorWindow
{
    /*===========================================================================================================*/
    
    private BehaviorTreeView _treeView;
    private InspectorView _inspectorView;
    private IMGUIContainer _dataContextView;
    
    // 렌더링 하기 위해 직렬화 가능한 객체 속성
    private SerializedObject _treeObject;
    private SerializedProperty _dataContextProperty;
    
    /*===========================================================================================================*/
    
    [MenuItem("BehaviorTree/BehaviorTree Editor")]
    public static void OpenWindow()
    {
        BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviorTreeEditor");
    }

    // BT SO 더블클릭, Editor Open
    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceId, int line)
    {
        if (Selection.activeObject is BehaviorTree)
        {
            OpenWindow();
            
            return true;
        }

        return false;
    }
    

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/__Scripts__/__Core__/Behavior Tree/UI Builders/BehaviorTreeEditor.uxml");
        visualTree.CloneTree(root);
        
        // Allocate StyleSheet 
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/__Scripts__/__Core__/Behavior Tree/UI Builders/BehaviorTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        _treeView = root.Q<BehaviorTreeView>();
        _inspectorView = root.Q<InspectorView>();
        _dataContextView = root.Q<IMGUIContainer>();
        _dataContextView.onGUIHandler = () => // 이 컨테이너 내에서, 'treeObject' 와 'dataContextProperty' 를 사용하여 블랙보드의 프로퍼티를 인스펙터에서 편집할 수 있게 한다.
        {
            if (_treeObject == null || _treeObject.targetObject == null)
                return;
            
            _treeObject.Update();
            EditorGUILayout.PropertyField(_dataContextProperty); // 코드에 변경사항이 있을 수 있으므로, UI를 렌더링하기 전에 이를 반영한한다.
            _treeObject.ApplyModifiedProperties(); // 수정된 속성을 적용하여 UI에서 변경한 내용을 직렬화된 객체에 다시 적용.
        };
        
        _treeView.OnNodeSelected = OnNodeSelectionChanged;
        
        OnSelectionChange();
    }

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }
    
    // 플레이 모드 상태가 변경될 때 호출되는 메서드
    private void OnPlayModeStateChanged(PlayModeStateChange obj)
    {
        switch (obj)
        {
            case PlayModeStateChange.EnteredEditMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingEditMode:
                break;
            case PlayModeStateChange.EnteredPlayMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                break;
        }
    }

    // 선택시 변경 이벤트 함수
    private void OnSelectionChange()
    {
        BehaviorTree tree = Selection.activeObject as BehaviorTree;

        if (_treeView == null)
            return;

        if (!tree)
        {
            if (Selection.activeGameObject) // 화성화된 게임 오브젝트가 null이 아니라면
            {
                BehaviorTreeRunner runner = Selection.activeGameObject.GetComponent<BehaviorTreeRunner>();
                if (runner)
                    tree = runner.tree;
            }
        }

        if (Application.isPlaying)
        {
            if (tree)
                _treeView.PopulateView(tree);
        }
        else
        {
            if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
            {
                _treeView.PopulateView(tree);
            }
        }

        // 트리를 선택하여 항목이 변경될 때마다 초기화 하므로 새로운 직렬화 객체 할당.
        if (tree != null)
        {
            _treeObject = new SerializedObject(tree);
            _dataContextProperty = _treeObject.FindProperty("dataContext");
        }
    }

    // 노드가 선택될 때 호출, Inspector View를 입력받은 node 정보로 업데이트.
    private void OnNodeSelectionChanged(NodeView node)
    {
        _inspectorView.UpdateSelection(node);
    }

    // 에디터 창에서 초당 10번 정도 호출되는 메서드
    private void OnInspectorUpdate()
    {
        _treeView?.UpdateNodeStates();
    }
}
