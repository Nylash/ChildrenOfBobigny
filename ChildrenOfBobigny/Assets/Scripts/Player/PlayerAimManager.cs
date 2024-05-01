using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimManager : MonoBehaviour
{
    public static PlayerAimManager instance;

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

    public Vector2 AimDirection { get => _aimDirection; }
    #endregion

    #region CONFIGURATION
    [Header("CONFIGURATION")]
    [SerializeField][Range(0, 1)] private float _rotationSpeed = .075f;
    #endregion

    private void OnEnable() => _controlsMap.Gameplay.Enable();
    private void OnDisable() => _controlsMap.Gameplay.Disable();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        _controlsMap = new ControlsMap();

        _controlsMap.Gameplay.AimStick.performed += ctx => _stickDirection = ctx.ReadValue<Vector2>();
        _controlsMap.Gameplay.AimMouse.performed += ctx => _mouseDirection = ctx.ReadValue<Vector2>();
        _controlsMap.Gameplay.AimStick.canceled += ctx => _stickDirection = Vector2.zero;

        _playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if(PlayerMovementManager.instance.CurrentMovementState != PlayerMovementManager.MovementState.Dashing)
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
                _animatorOrientation.SetFloat("InputX", Mathf.MoveTowards(_animatorOrientation.GetFloat("InputX"), _aimDirection.x, _rotationSpeed));
                _animatorOrientation.SetFloat("InputY", Mathf.MoveTowards(_animatorOrientation.GetFloat("InputY"), _aimDirection.y, _rotationSpeed));
            }
            else
            {
                _animatorOrientation.SetFloat("InputX", Mathf.MoveTowards(_animatorOrientation.GetFloat("InputX"), PlayerMovementManager.instance.MovementDirection.x, _rotationSpeed));
                _animatorOrientation.SetFloat("InputY", Mathf.MoveTowards(_animatorOrientation.GetFloat("InputY"), PlayerMovementManager.instance.MovementDirection.y, _rotationSpeed));
            }
        }
        else
        {
            _animatorOrientation.SetFloat("InputX", PlayerMovementManager.instance.DashDirection.x);
            _animatorOrientation.SetFloat("InputY", PlayerMovementManager.instance.DashDirection.y);
        }
    }
}
