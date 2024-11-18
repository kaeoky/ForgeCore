using UnityEngine;

namespace ForgeCore.Utility
{
    public static class Bootstrapper
    {
        // Log Settings
        private const bool LOGEnabled = true;
        private const string LOGColor = "purple";
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute()
        {
            var systems = Resources.LoadAll<MonoBehaviour>("Systems");

            if (systems.Length <= 0)
            {
                 if (LOGEnabled) 
                     Loggah.Log("No systems found", LOGColor);
                 return;
            }

            foreach (var system in systems)
            {
                if (LOGEnabled)
                    Loggah.Log($"Loaded <b>{system.name}</b>", LOGColor);
                Object.Instantiate(system);
            }
        }
    }
}