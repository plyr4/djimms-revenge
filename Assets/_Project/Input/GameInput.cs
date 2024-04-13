using UnityEngine;
using UnityEngine.InputSystem;

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

        public bool _fire;
        public bool _firePressed;
        public Vector3 _horizontalMovement;
        public Vector3 _look;
        public Vector3 _lookPosition;

        private void Start()
        {
            // initialize instance reference
            _ = Instance;
        }

        public void OnHorizontalMove(InputAction.CallbackContext context)
        {
            _horizontalMovement = context.ReadValue<Vector2>();
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
        }

        private bool ShouldReceiveInput()
        {
            return true;
        }
    }
}