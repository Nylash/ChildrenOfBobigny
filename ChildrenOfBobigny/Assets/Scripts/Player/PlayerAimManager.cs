using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimManager : Singleton<PlayerAimManager>
{
    #region COMPONENTS
    [Header("COMPONENTS")]
    private ControlsMap _controlsMap;
    [SerializeField] private Animator _animatorOrientation;
    private PlayerInput _playerInput;
    #endregion

    #region VARIABLES
    private Vector2 _stickDirection;
    private Vector2 _mouseDirection;
    private Vector2 _aimDirection;

    #region ACCESSEURS
    public Vector2 AimDirection { get => _aimDirection; }
    #endregion
    #endregion

    #region CONFIGURATION
    [Header("CONFIGURATION")]
    [SerializeField] private PlayerData _playerData;
    #endregion

    private void OnEnable() => _controlsMap.Gameplay.Enable();
    private void OnDisable() => _controlsMap.Gameplay.Disable();

    private void Awake()
    {
        _controlsMap = new ControlsMap();

        _controlsMap.Gameplay.AimStick.performed += ctx => _stickDirection = ctx.ReadValue<Vector2>();
        _controlsMap.Gameplay.AimMouse.performed += ctx => _mouseDirection = ctx.ReadValue<Vector2>();
        _controlsMap.Gameplay.AimStick.canceled += ctx => _stickDirection = Vector2.zero;

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

            if(_aimDirection != Vector2.zero)
            {
                _animatorOrientation.SetFloat("InputX", Mathf.MoveTowards(_animatorOrientation.GetFloat("InputX"), _aimDirection.x, _playerData.RotationSpeed));
                _animatorOrientation.SetFloat("InputY", Mathf.MoveTowards(_animatorOrientation.GetFloat("InputY"), _aimDirection.y, _playerData.RotationSpeed));
            }
            else
            {
                _animatorOrientation.SetFloat("InputX", Mathf.MoveTowards(_animatorOrientation.GetFloat("InputX"), PlayerMovementManager.Instance.MovementDirection.x, _playerData.RotationSpeed));
                _animatorOrientation.SetFloat("InputY", Mathf.MoveTowards(_animatorOrientation.GetFloat("InputY"), PlayerMovementManager.Instance.MovementDirection.y, _playerData.RotationSpeed));
            }
        }
        else
        {
            _animatorOrientation.SetFloat("InputX", PlayerMovementManager.Instance.DashDirection.x);
            _animatorOrientation.SetFloat("InputY", PlayerMovementManager.Instance.DashDirection.y);
        }
    }
}
