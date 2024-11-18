using UnityEditor;
using UnityEngine;

namespace ForgeCore.Utility.Editor.MenuItems
{
    public static class MenuItemEditor
    {
        [MenuItem("GameObject/ForgeCore/FPS Display")]
        private static void CreateFpsDisplay()
        {
            // Create Canvas if no Canvas is selected
            if (!Selection.activeGameObject?.GetComponent<Canvas>())
                CreateCanvas();

            // Instantiate Object from Project Files
            MenuItemUtility.InstantiatePrefabFromPath("FPSDisplay.prefab");
        }

        [MenuItem("GameObject/ForgeCore/Default Canvas")]
        private static void CreateCanvas()
        {
            MenuItemUtility.InstantiatePrefabFromPath("DefaultCanvas.prefab");
        }
    }
}