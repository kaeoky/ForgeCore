using System;
using System.IO;
using System.Linq;
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

                if (fileContent.Contains("MonoBehaviour") || fileContent.Contains("ScriptableObject"))
                {
                    if (!fileContent.Contains("using Sirenix.OdinInspector"))
                    {
                        // Add the 'using Sirenix.OdinInspector' statement only for relevant files
                        fileContent = "using Sirenix.OdinInspector;\n" + fileContent;
                    }

                    // Convert MonoBehaviour to SerializedMonoBehaviour
                    fileContent = ConvertClass(fileContent, "MonoBehaviour", "SerializedMonoBehaviour");
                    // Convert ScriptableObject to SerializedScriptableObject
                    fileContent = ConvertClass(fileContent, "ScriptableObject", "SerializedScriptableObject");

                    // Handle generic MonoBehaviour (Singleton<T>)
                    fileContent = ConvertGenericMonoBehaviour(fileContent, "MonoBehaviour", "SerializedMonoBehaviour");

                    // Replace field attributes [Header] with [Title] and [SerializeField] with [SerializeField]
                    bool hasHeaderReplacement = fileContent.Contains("[Header");
                    fileContent = ReplaceFieldAttributes(fileContent);

                    // If we replaced [Header] with [Title], add the '' statement
                    if (hasHeaderReplacement && !fileContent.Contains("using Sirenix.Serialization"))
                    {
                        fileContent = "\n" + fileContent;
                    }

                    // Write the changes back to the file
                    File.WriteAllText(file, fileContent);
                }
            }

            AssetDatabase.Refresh();
            Debug.Log("Conversion completed!");
        }

        private static string ReplaceFieldAttributes(string fileContent)
        {
            // Replace [Header] with [Title]
            fileContent = Regex.Replace(fileContent, @"\[Header\(", "[Header(");

            // Replace [SerializeField] with [SerializeField]
            fileContent = Regex.Replace(fileContent, @"\[SerializeField\]", "[SerializeField]");

            return fileContent;
        }

        private static string ConvertClass(string fileContent, string oldType, string newType)
        {
            // Replace class declarations that inherit from the specified type (MonoBehaviour or ScriptableObject)
            return Regex.Replace(fileContent, $@"\bpublic\s+class\s+(\w+)\s*:\s*{oldType}\b", 
                match => $"public class {match.Groups[1].Value} : {newType}");
        }

        private static string ConvertGenericMonoBehaviour(string fileContent, string oldType, string newType)
        {
            // Handle generic MonoBehaviour (like Singleton<T>) by replacing its base type
            return Regex.Replace(fileContent, $@"\bpublic\s+class\s+(\w+)<.*>\s*:\s*{oldType}\b", 
                match => $"public class {match.Groups[1].Value}<T> : {newType}");
        }

        [MenuItem("Tools/ForgeCore/Revert to Original")]
        private static void RevertToOriginal()
        {
            string folderPath = "Assets/ForgeCore";
            string[] files = Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                string fileContent = File.ReadAllText(file);

                // Revert SerializedMonoBehaviour to MonoBehaviour
                fileContent = RevertClass(fileContent, "SerializedMonoBehaviour", "MonoBehaviour");
                // Revert SerializedScriptableObject to ScriptableObject
                fileContent = RevertClass(fileContent, "SerializedScriptableObject", "ScriptableObject");

                // Handle generic MonoBehaviour (Singleton<T>)
                fileContent = RevertGenericMonoBehaviour(fileContent, "SerializedMonoBehaviour", "MonoBehaviour");

                // Revert field attributes [Title] to [Header] and [SerializeField] to [SerializeField]
                fileContent = RevertFieldAttributes(fileContent);

                // Remove '' if it was added during conversion
                if (fileContent.Contains(""))
                {
                    bool wasHeaderReplaced = fileContent.Contains("[Title");
                    if (wasHeaderReplaced)
                    {
                        string pattern = @"\s*";
                        fileContent = Regex.Replace(fileContent, pattern, string.Empty);
                    }
                }

                // Write the changes back to the file
                File.WriteAllText(file, fileContent);
            }

            AssetDatabase.Refresh();
            Debug.Log("Reversion completed!");
        }

        private static string RevertFieldAttributes(string fileContent)
        {
            // Revert [Title] to [Header]
            fileContent = Regex.Replace(fileContent, @"\[Title\(", "[Header(");

            // Revert [SerializeField] to [SerializeField]
            fileContent = Regex.Replace(fileContent, @"\[OdinSerialize\]", "[SerializeField]");

            return fileContent;
        }

        private static string RevertClass(string fileContent, string oldType, string newType)
        {
            // Revert class declarations that inherit from the specified type (MonoBehaviour or ScriptableObject)
            return Regex.Replace(fileContent, $@"\bpublic\s+class\s+(\w+)\s*:\s*{oldType}\b", 
                match => $"public class {match.Groups[1].Value} : {newType}");
        }

        private static string RevertGenericMonoBehaviour(string fileContent, string oldType, string newType)
        {
            // Handle generic MonoBehaviour (like Singleton<T>) by reverting its base type
            return Regex.Replace(fileContent, $@"\bpublic\s+class\s+(\w+)<.*>\s*:\s*{oldType}\b", 
                match => $"public class {match.Groups[1].Value}<T> : {newType}");
        }

        private static bool IsOdinInspectorPresent()
        {
            // Check if Odin Inspector is included in the project by searching for its assembly
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return assemblies.Any(a => a.GetName().Name.Contains("Sirenix"));
        }
    }
}
