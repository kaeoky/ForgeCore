using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace ForgeCore.PluginSupport.OdinInspector
{
    public static class OdinConverter
    {
        [MenuItem("Tools/ForgeCore/Convert to Odin")]
        private static void ConvertToOdin()
        {
            string folderPath = "Assets/ForgeCore";
            string[] files = Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                string fileContent = File.ReadAllText(file);

                // Check if the file contains classes derived from MonoBehaviour or ScriptableObject, including generics
                bool hasMonoOrScriptableObjectClass = false;

                // Regex to match both non-generic and generic classes inheriting from MonoBehaviour or ScriptableObject
                foreach (Match match in Regex.Matches(fileContent,
                             @"public\s+class\s+\w+(\<.*\>)?\s*:\s*(MonoBehaviour|ScriptableObject)"))
                {
                    hasMonoOrScriptableObjectClass = true;
                    break;
                }

                // Only modify files with relevant class types (MonoBehaviour or ScriptableObject)
                if (hasMonoOrScriptableObjectClass)
                {
                    // Check if "using Sirenix.OdinInspector" is already present
                    if (!fileContent.Contains("using Sirenix.OdinInspector"))
                    {
                        // Add 'using Sirenix.OdinInspector;' at the top if not present
                        fileContent = "using Sirenix.OdinInspector;\n" + fileContent;
                    }

                    // Convert MonoBehaviour to SerializedMonoBehaviour
                    fileContent = ConvertClass(fileContent, "MonoBehaviour", "SerializedMonoBehaviour");
                    // Convert ScriptableObject to SerializedScriptableObject
                    fileContent = ConvertClass(fileContent, "ScriptableObject", "SerializedScriptableObject");

                    // Handle generic MonoBehaviour (Singleton<T>) - converted as well
                    fileContent = ConvertGenericMonoBehaviour(fileContent, "MonoBehaviour", "SerializedMonoBehaviour");

                    // Write the changes back to the file
                    File.WriteAllText(file, fileContent);
                }
            }

            AssetDatabase.Refresh();
            Debug.Log("Conversion completed!");
        }

        private static string ConvertClass(string fileContent, string oldType, string newType)
        {
            // Replace occurrences of the old type with the new type
            return fileContent.Replace($" : {oldType}", $" : {newType}");
        }

        private static string ConvertGenericMonoBehaviour(string fileContent, string oldType, string newType)
        {
            // Regex to match MonoBehaviour types that are generic (e.g., MonoBehaviour<T>)
            string pattern = $@"\b{oldType}<(.*?)>";
            string replacement = $"{newType}<$1>";

            return Regex.Replace(fileContent, pattern, replacement);
        }

        [MenuItem("Tools/ForgeCore/Revert to Original")]
        private static void RevertToOriginal()
        {
            string folderPath = "Assets/ForgeCore";
            string[] files = Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                // Skip the OdinConverter.cs file to prevent modifying it
                if (file.EndsWith("OdinConverter.cs"))
                    continue;

                string fileContent = File.ReadAllText(file);

                // Revert SerializedMonoBehaviour to MonoBehaviour
                fileContent = RevertClass(fileContent, "SerializedMonoBehaviour", "MonoBehaviour");
                // Revert SerializedScriptableObject to ScriptableObject
                fileContent = RevertClass(fileContent, "SerializedScriptableObject", "ScriptableObject");

                // Handle generic MonoBehaviour (SerializedMonoBehaviour<T>) - reverted as well
                fileContent = RevertGenericMonoBehaviour(fileContent, "SerializedMonoBehaviour", "MonoBehaviour");

                // Remove 'using Sirenix.OdinInspector;' if it is the only using statement
                if (fileContent.Contains("using Sirenix.OdinInspector;"))
                {
                    string pattern = @"using Sirenix.OdinInspector;\s*";
                    fileContent = Regex.Replace(fileContent, pattern, string.Empty);

                    // If only "using Sirenix.OdinInspector;" was present, remove it
                    if (fileContent.Trim().StartsWith("using") && !fileContent.Contains("using"))
                    {
                        fileContent = fileContent.Replace("using Sirenix.OdinInspector;", "").Trim();
                    }
                }

                // Write the changes back to the file
                File.WriteAllText(file, fileContent);
            }

            AssetDatabase.Refresh();
            Debug.Log("Reversion completed!");
        }

        private static string RevertClass(string fileContent, string oldType, string newType)
        {
            // Replace occurrences of the old type with the new type
            return fileContent.Replace($" : {oldType}", $" : {newType}");
        }

        private static string RevertGenericMonoBehaviour(string fileContent, string oldType, string newType)
        {
            // Regex to match MonoBehaviour types that are generic (e.g., SerializedMonoBehaviour<T>)
            string pattern = $@"\b{oldType}<(.*?)>";
            string replacement = $"{newType}<$1>";

            return Regex.Replace(fileContent, pattern, replacement);
        }
    }
}
