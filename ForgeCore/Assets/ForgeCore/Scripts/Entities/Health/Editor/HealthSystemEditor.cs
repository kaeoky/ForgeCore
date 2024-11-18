using ForgeCore.Utility.Editor;
using UnityEditor;
using UnityEngine;

namespace ForgeCore.Entities.Health.Editor
{
#if UNITY_EDITOR
    [CustomEditor(typeof(HealthSystem), true)]
    public class HealthSystemEditor : UnityEditor.Editor
    {
        private bool _showDefaultInspector;
        
        private HealthSystem _healthSystem;

        private void OnEnable()
        {
            _healthSystem = (HealthSystem)target;
        }

        public override void OnInspectorGUI()
        {
            EditorUtil.DrawTitle("Health System");
            
            EditorUtil.ProgressBar(_healthSystem.health / _healthSystem.maxHealth,
                $"{_healthSystem.health}/{_healthSystem.maxHealth} HP", 35f);
            
            EditorGUILayout.BeginHorizontal();
            _healthSystem.health = EditorGUILayout.Slider(_healthSystem.health, 0.1f, _healthSystem.maxHealth);
            _healthSystem.maxHealth = EditorGUILayout.FloatField(_healthSystem.maxHealth, GUILayout.Width(50f));
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(15f);
            EditorGUILayout.LabelField("Actions");
            EditorUtil.DrawUILine();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Kill", GUILayout.Height(30f), GUILayout.Width(100f)))
                _healthSystem.Damage(_healthSystem.health);
            if (GUILayout.Button("Heal", GUILayout.Height(30f), GUILayout.Width(100f)))
                _healthSystem.Heal(_healthSystem.maxHealth - _healthSystem.health);
            EditorGUILayout.EndHorizontal();
            
            ShowDefaultInspector();
        }

        private void ShowDefaultInspector()
        {
            EditorGUILayout.Space(50f);
            EditorUtil.DrawUILine();
            _showDefaultInspector = EditorGUILayout.Foldout(_showDefaultInspector, "Default Inspector");
            if (_showDefaultInspector)
            {
                base.OnInspectorGUI();
            }
        }
    }
#endif
}