
using Core.Editor;
using UnityEngine.UIElements;

namespace StatSystem.Editor
{
    public class StatCollectionEditor : SO_StatCollectionEditor<StatDefinitionSO>
    {
        public new class UxmlFactory : UxmlFactory<StatCollectionEditor, UxmlTraits> {}
    }
}