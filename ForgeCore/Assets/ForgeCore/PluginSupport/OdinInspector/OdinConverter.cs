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

                // Handle generic MonoBehaviour (Singleton<T>)
                fileContent = ConvertGenericMonoBehaviour(fileContent, "MonoBehaviour", "SerializedMonoBehaviour");

                // Write the changes back to the file
                File.WriteAllText(file, fileContent);
            }

            AssetDatabase.Refresh();
            Debug.Log("Conversion completed!");
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

        private static string ConvertClass(string fileContent, string oldClassType, string newClassType)
        {
            // Regex to find classes that derive from MonoBehaviour or ScriptableObject
            string pattern = $@"\bpublic class (\w+)\s*:\s*{oldClassType}\b";
            string replacement = $@"public class $1 : {newClassType}";

            // This should catch all MonoBehaviour and ScriptableObject classes
            return Regex.Replace(fileContent, pattern, replacement);
        }

        private static string ConvertGenericMonoBehaviour(string fileContent, string oldClassType, string newClassType)
        {
            // Regex to find generic MonoBehaviour classes like Singleton<T>
            string pattern = $@"\bpublic class (\w+)<T>\s*:\s*{oldClassType}\s*where\s*T\s*:\s*Component\b";
            string replacement = $@"public class $1<T> : {newClassType} where T : Component";

            return Regex.Replace(fileContent, pattern, replacement);
        }

        private static string RevertClass(string fileContent, string oldClassType, string newClassType)
        {
            // Regex to revert SerializedMonoBehaviour to MonoBehaviour or SerializedScriptableObject to ScriptableObject
            string pattern = $@"\bpublic class (\w+)\s*:\s*{oldClassType}\b";
            string replacement = $@"public class $1 : {newClassType}";

            return Regex.Replace(fileContent, pattern, replacement);
        }

        private static string RevertGenericMonoBehaviour(string fileContent, string oldClassType, string newClassType)
        {
            // Regex to revert generic MonoBehaviour classes like Singleton<T>
            string pattern = $@"\bpublic class (\w+)<T>\s*:\s*{oldClassType}\s*where\s*T\s*:\s*Component\b";
            string replacement = $@"public class $1<T> : {newClassType} where T : Component";

            return Regex.Replace(fileContent, pattern, replacement);
        }
    }
}
