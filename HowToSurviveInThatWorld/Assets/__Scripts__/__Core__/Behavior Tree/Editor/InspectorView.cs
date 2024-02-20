using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor;

public class InspectorView : VisualElement
{
    private Editor _editor;
    
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> {}
    
    public InspectorView()
    {
        
    }

    // 다른 노드를 선택했을 때 호출, 현재 정보를 지우고 새로 생성.
    internal void UpdateSelection(NodeView nodeView)
    {
        Clear();

        UnityEngine.Object.DestroyImmediate(_editor);
        _editor = Editor.CreateEditor(nodeView.node);
        IMGUIContainer container = new IMGUIContainer(() =>
        {
            if (_editor.target) // Undo로 인해 명확한 target Object 없이 렌더링(GUI Update) 시도시 오류가 발생 하기 때문에 조건문으로 체크
                _editor.OnInspectorGUI(); 
        });
        Add(container);
    }
}
