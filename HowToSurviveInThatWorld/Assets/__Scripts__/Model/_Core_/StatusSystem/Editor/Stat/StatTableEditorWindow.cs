using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace StatSystem.Editor
{
    public class StatTableEditorWindow : EditorWindow
    {
        #region Fields

        // Serialized UXML / USS
        [SerializeField] private VisualTreeAsset _VisualTree;
        [SerializeField] private StyleSheet _StyleSheet;
        
        private StatTableSO m_Database;
        private StatCollectionEditor m_Current;

        #endregion



        #region Show

        [MenuItem("Window/StatSystem/StatDatabase")]
        public static void ShowWindow()
        {
            StatTableEditorWindow window = GetWindow<StatTableEditorWindow>();
            window.minSize = new Vector2(800, 600);
            window.titleContent = new GUIContent("StatDatabase");
        }
        
        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            if (EditorUtility.InstanceIDToObject(instanceId) is StatTableSO)
            {
                ShowWindow();
                return true;
            }

            return false;
        }
        
        private void OnSelectionChange()
        {
            m_Database = Selection.activeObject as StatTableSO;
        }

        #endregion



        #region Create Function

        public void CreateGUI()
        {
            OnSelectionChange();

            // Root Element 설정
            VisualElement root = rootVisualElement;

            // Visual Tree Config
            _VisualTree.CloneTree(root);
            root.styleSheets.Add(_StyleSheet);

            StatCollectionEditor stats = root.Q<StatCollectionEditor>("stats");
            stats.Initialize(m_Database, m_Database.Stats);
            Button statsTab = root.Q<Button>("stats-tab");
            statsTab.clicked += () =>
            {
                m_Current.style.display = DisplayStyle.None;
                stats.style.display = DisplayStyle.Flex;
                m_Current = stats;
            };

            StatCollectionEditor primaryStats = root.Q<StatCollectionEditor>("primary-stats");
            primaryStats.Initialize(m_Database, m_Database.Primary);
            Button primaryStatsTab = root.Q<Button>("primary-stats-tab");
            primaryStatsTab.clicked += () =>
            {
                m_Current.style.display = DisplayStyle.None;
                primaryStats.style.display = DisplayStyle.Flex;
                m_Current = primaryStats;
            };

            StatCollectionEditor attributes = root.Q<StatCollectionEditor>("attributes");
            attributes.Initialize(m_Database, m_Database.Attributes);
            Button attributesTab = root.Q<Button>("attributes-tab");
            attributesTab.clicked += () =>
            {
                m_Current.style.display = DisplayStyle.None;
                attributes.style.display = DisplayStyle.Flex;
                m_Current = attributes;
            };

            m_Current = stats;
        }

        #endregion
    }
}