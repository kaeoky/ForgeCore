using UnityEditor;
using UnityEngine;

namespace ForgeCore.Utility.Editor.MenuItems
{
    public static class MenuItemUtility
    {
        private const string DefaultPath = "Assets/ForgeCore/Resources/ForgeAssets/";
        
        public static GameObject InstantiatePrefabFromPath(string prefabPath)
        {
            prefabPath = string.Concat(DefaultPath, prefabPath);
            
            // Load the prefab from the specified path
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            if (prefab == null)
            {
                Debug.LogError($"Prefab not found at path: {prefabPath}");
                return null; // Early exit on failure
            }
    
            // Instantiate the prefab at the currently selected transform's position (or in the scene root if none selected)
            var instantiatedObject = PrefabUtility.InstantiatePrefab(prefab, Selection.activeTransform) as GameObject;

            // Set the newly instantiated object as the active selection
            Selection.activeGameObject = instantiatedObject;

            return instantiatedObject;
        }
    }
}