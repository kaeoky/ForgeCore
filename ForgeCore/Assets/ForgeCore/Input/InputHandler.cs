using ForgeCore.Utility;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ForgeCore.Input
{
    public class InputHandler : Singleton<InputHandler>
    {
        #region Variables

        [Header("Input Handler")] [SerializeField]
        private bool logEnabled;
        
        [field: SerializeField]
        public Vector2 Move { get; private set; }
        
        public string CurrentControlScheme => _playerInput.currentControlScheme;
        public bool IsCurrentDeviceMouse => _playerInput.currentControlScheme == "KeyboardMouse";
        
        private PlayerInput _playerInput;
        
        #endregion

        #region Startup

        protected override void Awake()
        {
            base.Awake();

            _playerInput = GetComponent<PlayerInput>();
            _playerInput.SwitchCurrentActionMap("Player");
            _playerInput.controlsChangedEvent.AddListener(OnControlsChanged);
        }

        #endregion

        #region Implementation

        private void OnMove(InputValue value)
        {
            Move = value.Get<Vector2>();
        }

        #endregion

        #region Events

        private void OnControlsChanged(PlayerInput playerInput)
        {
            if (logEnabled)
                Loggah.Log($"Controls changed to <b>{playerInput.currentControlScheme}</b>");
            Actions.OnControlsChanged?.Invoke(playerInput);
        }

        #endregion
    }
}