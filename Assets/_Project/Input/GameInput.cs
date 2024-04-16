using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Project.Input
{
    public class GameInput : MonoBehaviour
    {
        private static GameInput _instance;
        public static GameInput Instance
        {
            get
            {
                // attempt to locate the singleton
                if (_instance == null)
                {
                    _instance = (GameInput)FindObjectOfType(typeof(GameInput));
                }

                // create a new singleton
                if (_instance == null)
                {
                    _instance = (new GameObject("GameManager")).AddComponent<GameInput>();
                }

                // return singleton
                return _instance;
            }
        }

        [ReadOnlyInspector]
        public bool _fire;
        [ReadOnlyInspector]
        public bool _firePressed;
        [ReadOnlyInspector]
        public bool _fireReleased;
        [ReadOnlyInspector]
        public bool _fireAlt;
        [ReadOnlyInspector]
        public bool _fireAltPressed;
        [ReadOnlyInspector]
        public bool _fireAltReleased;
        [ReadOnlyInspector]
        public bool _jump;
        [ReadOnlyInspector]
        public bool _jumpPressed;
        [ReadOnlyInspector]
        public bool _jumpReleased;
        [ReadOnlyInspector]
        public Vector3 _horizontalMovement;
        [ReadOnlyInspector]
        public Vector3 _verticalMovement;
        [ReadOnlyInspector]
        public Vector3 _look;
        [ReadOnlyInspector]
        public Vector3 _lookPosition;
        [ReadOnlyInspector]
        public string _controlScheme;

        private void Start()
        {
            // initialize instance reference
            _ = Instance;
        }

        public void OnControlsChanged(PlayerInput playerInput)
        {
            _controlScheme = playerInput.currentControlScheme;
        }

        public void OnHorizontalMove(InputAction.CallbackContext context)
        {
            _horizontalMovement = context.ReadValue<Vector2>();
        }

        public void OnVerticalMove(InputAction.CallbackContext context)
        {
            _verticalMovement = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            _look = context.ReadValue<Vector2>();
        }

        public void OnLookPosition(InputAction.CallbackContext context)
        {
            _lookPosition = context.ReadValue<Vector2>();
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            if (!ShouldReceiveInput()) return;
            _fire = context.action.IsPressed();
            _firePressed = context.action.WasPressedThisFrame();
            _fireReleased = context.action.WasReleasedThisFrame();
        }

        public void OnFireAlt(InputAction.CallbackContext context)
        {
            if (!ShouldReceiveInput()) return;
            _fireAlt = context.action.IsPressed();
            _fireAltPressed = context.action.WasPressedThisFrame();
            _fireAltReleased = context.action.WasReleasedThisFrame();
        }
        
        public void OnJump(InputAction.CallbackContext context)
        {
            if (!ShouldReceiveInput()) return;
            _jump = context.action.IsPressed();
            _jumpPressed = context.action.WasPressedThisFrame();
            _jumpReleased = context.action.WasReleasedThisFrame();
        }

        private bool ShouldReceiveInput()
        {
            return true;
        }
    }
}