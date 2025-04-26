using System;
using Example.Core.Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Example.Input
{
    /// <summary>
    /// Service for managing input actions
    /// </summary>
    public class InputService : MonoBehaviour, IGameService
    {
        private const float MOUSE_HOLD_TIME = 0.25f;
        
        public event Action<int> OnHudItemSelectAction; 
        public event Action<Vector3> OnClick;
        public event Action<Vector3> OnMouseHold;
        public event Action<Vector3> OnMouseHoldStopped;
        public event Action<Vector3> OnRightClick;
        public event Action<Vector3> OnCameraMoving;
        public event Action OnInventoryButton;

        private float _clickTime = 0;
        private Vector2 _startHoldPosition = Vector2.zero;
        
        public Vector2 HoldInitPosition => _startHoldPosition;

        private InputAsset _inputAsset;
        private InputAction _leftMouseAction;
        private InputAction _rightMouseAction;
        private InputAction _scrollMouseAction;
        private InputAction _cameraMovingAction;
        private InputAction _inventoryAction;
        private InputAction _shiftAction;
        private InputAction _holdCTRLAction;
        private InputAction _hudItemSelectAction;

        public bool IsShiftPressed()
        {
            return _shiftAction.IsPressed();
        }
        
        public bool IsCTRLPressed()
        {
            return _holdCTRLAction.IsPressed();
        }
        
        private void Awake()
        {
            _inputAsset = new InputAsset();
            
            _leftMouseAction = _inputAsset.Player.LeftMouseClick;
            _rightMouseAction = _inputAsset.Player.RightMouseClick;
            _scrollMouseAction = _inputAsset.Player.ScrollMouse;
            _cameraMovingAction = _inputAsset.Player.CameraMoving;
            _inventoryAction = _inputAsset.Player.Inventory;
            _shiftAction = _inputAsset.Player.Shift;
            _holdCTRLAction = _inputAsset.Player.HoldCTRL;
            _hudItemSelectAction = _inputAsset.Player.HudItemSelectAction;
            _inputAsset.Enable();
        }

        public void Update()
        {
            if (_inventoryAction.WasPerformedThisFrame())
            {
                OnInventoryButton?.Invoke();
                return;
            }

            if (_hudItemSelectAction.WasPerformedThisFrame())
            {
                OnHudItemSelectAction?.Invoke((int)_hudItemSelectAction.ReadValue<float>());
                return;
            }
            
            if (_leftMouseAction.IsPressed())
            {
                if(_startHoldPosition == Vector2.zero)
                    _startHoldPosition = UnityEngine.Input.mousePosition;
                
                _clickTime += Time.deltaTime;

                if (_clickTime > MOUSE_HOLD_TIME)
                {
                    OnMouseHold?.Invoke(UnityEngine.Input.mousePosition);
                }
            }

            if (_leftMouseAction.WasReleasedThisFrame())
            {
                if (_clickTime < MOUSE_HOLD_TIME)
                {
                    OnClick?.Invoke(UnityEngine.Input.mousePosition);
                }
                else
                {
                    OnMouseHoldStopped?.Invoke(UnityEngine.Input.mousePosition);
                }

                _startHoldPosition = Vector3.zero;
                _clickTime = 0;
            }

            if (_rightMouseAction.WasPressedThisFrame())
            {
                OnRightClick?.Invoke(UnityEngine.Input.mousePosition);
            }

            if (_cameraMovingAction.IsInProgress())
            {
                Vector3 cameraMovement = _cameraMovingAction.ReadValue<Vector2>();

                if (_scrollMouseAction.IsInProgress())
                {
                    cameraMovement.z = _scrollMouseAction.ReadValue<Vector2>().y;
                }
                
                OnCameraMoving?.Invoke(cameraMovement);
            }
            else
            {
                if (_scrollMouseAction.IsInProgress())
                {
                    OnCameraMoving?.Invoke(new Vector3(0, 0, _scrollMouseAction.ReadValue<Vector2>().y));
                }
            }
        }
    }
}
