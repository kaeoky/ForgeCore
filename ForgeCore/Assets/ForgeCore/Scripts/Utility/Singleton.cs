using UnityEngine;

namespace ForgeCore.Utility
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        [Header("Singleton"), SerializeField]
        private bool isPersistent;

        [SerializeField]  
        private string appendix = "<Singleton>";
        
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance)
            {
                Debug.LogWarning($"<b>{typeof(T)}</b> already exists.");
                Destroy(gameObject);
                return;
            }
            
            Instance = this as T;
            name = $"{typeof(T).Name} {appendix}";
            
            if (isPersistent)
                DontDestroyOnLoad(this);
        }
    }
}
