using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ForgeCore.Events
{
    public static class Actions
    {
        #region Input

        public static UnityEvent<PlayerInput> OnControlsChanged = new();

        #endregion
    }
}