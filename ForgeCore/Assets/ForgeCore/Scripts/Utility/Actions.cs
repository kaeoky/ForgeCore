using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ForgeCore.Utility
{
    public static class Actions
    {
        #region Input

        public static readonly UnityEvent<PlayerInput> OnControlsChanged = new();

        #endregion
    }
}