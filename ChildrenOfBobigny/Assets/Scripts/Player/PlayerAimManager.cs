using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimManager : Singleton<PlayerAimManager>
{
    #region COMPONENTS
    [Header("COMPONENTS")]
    private ControlsMap _controlsMap;
    [SerializeField] private Animator _orientationAnimator;
    private PlayerInput _playerInput;
    #endregion

    #region VARIABLES
    private Vector2 _stickDirection;
    private Vector2 _mouseDirection;
    private Vector2 _aimDirection;
    private Vector2 _lastStickDirection;

    #region ACCESSEURS
    public Vector2 AimDirection { get => _aimDirection; }
    #endregion
    #endregion

    #region CONFIGURATION
    [Header("CONFIGURATION")]
    [SerializeField] private PlayerData _playerData;
    #endregion

    private void OnEnable()
    {
        _controlsMap.Gameplay.Enable();
        PlayerMovementManager.Instance.event_inputMovementIsStopped.AddListener(UpdateLastStickDirection);
    }
    private void OnDisable()
    {
        if (!this.gameObject.scene.isLoaded) return;
        _controlsMap.Gameplay.Disable();
        PlayerMovementManager.Instance.event_inputMovementIsStopped.RemoveListener(UpdateLastStickDirection);
    }

    protected override void OnAwake()
    {
        _controlsMap = new ControlsMap();

        _controlsMap.Gameplay.AimStick.performed += ctx => _stickDirection = ctx.ReadValue<Vector2>();
        _controlsMap.Gameplay.AimStick.canceled += ctx => StopReadingStickDirection();
        _controlsMap.Gameplay.AimMouse.performed += ctx => _mouseDirection = ctx.ReadValue<Vector2>();

        _playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if(PlayerMovementManager.Instance.CurrentMovementState != PlayerMovementManager.MovementState.Dashing)
        {
            if (_playerInput.currentControlScheme == "Keyboard")
            {
                Ray ray = Camera.main.ScreenPointToRay(_mouseDirection);
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                float distance;
                if (plane.Raycast(ray, out distance))
                {
                    Vector3 target = ray.GetPoint(distance) - transform.position;
                    _aimDirection = new Vector2(target.x, target.z).normalized;
                }
            }
            else
            {
                _aimDirection = _stickDirection;
            }

            if(_aimDirection != Vector2.zero)//Mouse or when right stick is used
            {
                _orientationAnimator.SetFloat("InputX", Mathf.MoveTowards(_orientationAnimator.GetFloat("InputX"), _aimDirection.x, _playerData.RotationSpeed));
                _orientationAnimator.SetFloat("InputY", Mathf.MoveTowards(_orientationAnimator.GetFloat("InputY"), _aimDirection.y, _playerData.RotationSpeed));
            }
            else if(_controlsMap.Gameplay.Movement.IsPressed())//Right stick not used but left is, so we aim with it
            {
                _orientationAnimator.SetFloat("InputX", Mathf.MoveTowards(_orientationAnimator.GetFloat("InputX"), PlayerMovementManager.Instance.MovementDirection.x, _playerData.RotationSpeed));
                _orientationAnimator.SetFloat("InputY", Mathf.MoveTowards(_orientationAnimator.GetFloat("InputY"), PlayerMovementManager.Instance.MovementDirection.y, _playerData.RotationSpeed));
            }
            else//No direction is given (neither movement nor aim) so we look at the last one register
            {
                _orientationAnimator.SetFloat("InputX", Mathf.MoveTowards(_orientationAnimator.GetFloat("InputX"), _lastStickDirection.x, _playerData.RotationSpeed));
                _orientationAnimator.SetFloat("InputY", Mathf.MoveTowards(_orientationAnimator.GetFloat("InputY"), _lastStickDirection.y, _playerData.RotationSpeed));
            }
        }
        else
        {
            _orientationAnimator.SetFloat("InputX", PlayerMovementManager.Instance.MovementDirection.x);
            _orientationAnimator.SetFloat("InputY", PlayerMovementManager.Instance.MovementDirection.y);
        }
    }

    private void StopReadingStickDirection()
    {
        _lastStickDirection = _stickDirection;
        _stickDirection = Vector2.zero;
    }

    private void UpdateLastStickDirection()//If when player stop moving he wasn't aiming, we override _lastStickDirection with the movementDirection, so he keeps looking forward
    {
        if(_stickDirection == Vector2.zero) 
        {
            _lastStickDirection = PlayerMovementManager.Instance.MovementDirection;
        }
    }
}
