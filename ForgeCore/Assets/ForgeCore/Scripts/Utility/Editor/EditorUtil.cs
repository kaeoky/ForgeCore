using UnityEditor;
using UnityEngine;

namespace ForgeCore.Utility.Editor
{
#if UNITY_EDITOR
    public static class EditorUtil
    {
        public static void DrawTitle(string title, bool hasSignature = true, string signature = "made by @Christoph Netzer")
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(title, new GUIStyle(GUI.skin.label)
            {
                fontSize = 19,
                fixedHeight = 25,
                fontStyle = FontStyle.Bold
            });
            if (hasSignature)
            {
                EditorGUILayout.LabelField(signature, new GUIStyle(GUI.skin.label)
                {
                    fontSize = 11,
                    fixedHeight = 25,
                    alignment = TextAnchor.UpperLeft
                });
            }
            DrawUILine(default, 1, 0);
            EditorGUILayout.Space(15);
        }
        
        public static void ProgressBar(float value, string label, float height = 18f)
        {
            Rect rect = GUILayoutUtility.GetRect(18f, height, "TextField");
            EditorGUI.ProgressBar(rect, value, label);
        }
        
        public static void DrawUILine(Color color = default, int thickness = 1, int padding = 10, int margin = 0)
        {
            color = color != default ? color : Color.grey;
            Rect r = EditorGUILayout.GetControlRect(false, GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding * 0.5f;
 
            switch (margin)
            {
                // expand to maximum width
                case < 0:
                    r.x = 0;
                    r.width = EditorGUIUtility.currentViewWidth;
 
                    break;
                case > 0:
                    // shrink line width
                    r.x += margin;
                    r.width -= margin * 2;
 
                    break;
            }
 
            EditorGUI.DrawRect(r, color);
        }
    }
#endif
}
