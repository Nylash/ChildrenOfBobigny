using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovementManager : MonoBehaviour
{
    public static PlayerMovementManager instance;

    #region COMPONENTS
    private ControlsMap _controlsMap;
    private CharacterController _controller;
    [SerializeField] private Animator _animator;
    #endregion

    #region VARIABLES
    private MovementState _currentMovementState = MovementState.Idling;
    private Vector2 _movementDirection;
    private Vector2 _dashDirection;
    private bool _isDashReady = true;

    public MovementState CurrentMovementState { get => _currentMovementState; }
    public Vector2 DashDirection { get => _dashDirection; }
    #endregion

    #region CONFIGURATION
    [Header("CONFIGURATION")]
    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private float _dashSpeed = 20;
    [SerializeField] private float _dashCD = 1;
    [SerializeField] private float _dashDuration = .3f;
    [SerializeField] private float _gravityForce = 9.81f;
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

        _controlsMap.Gameplay.Movement.performed += ctx => ReadMovementDirection(ctx);
        _controlsMap.Gameplay.Movement.canceled += ctx => StopReadMovementDirection();
        _controlsMap.Gameplay.Dash.performed += ctx => StartCoroutine(Dashing());

        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if(_currentMovementState == MovementState.Moving)
        {
            if (_controller.isGrounded)
                _controller.Move(new Vector3(_movementDirection.x, 0, _movementDirection.y) * _movementSpeed * Time.deltaTime);
            else
                _controller.Move(new Vector3(_movementDirection.x, -_gravityForce, _movementDirection.y) * _movementSpeed * Time.deltaTime);
            _animator.SetFloat("Angle", Mathf.Abs(Vector2.Angle(_movementDirection, PlayerAimManager.instance.AimDirection)));
        }
        else if (_currentMovementState != MovementState.Dashing && !_controller.isGrounded)
        {
            _controller.Move(new Vector3(0, -_gravityForce, 0) * _movementSpeed * Time.deltaTime);
        }
    }

    private IEnumerator Dashing()
    {
        if (_isDashReady)
        {
            _animator.SetBool("Dashing", true);
            _currentMovementState = MovementState.Dashing;
            _isDashReady = false;
            if(_movementDirection != Vector2.zero)
                _dashDirection = _movementDirection;
            float startTime = Time.time;
            while( Time.time < startTime + _dashDuration) 
            {
                _controller.Move(new Vector3(_dashDirection.x, 0, _dashDirection.y) * _dashSpeed * Time.deltaTime);
                yield return null;
            }
            _animator.SetBool("Dashing", false);
            _currentMovementState = MovementState.Moving;
            yield return new WaitForSeconds(_dashCD - _dashDuration);
            _isDashReady = true;
        }
    }

    private void ReadMovementDirection(InputAction.CallbackContext ctx)
    {
        _movementDirection = ctx.ReadValue<Vector2>().normalized;
        _currentMovementState = MovementState.Moving;
        _animator.SetBool("Running", true);
    }

    private void StopReadMovementDirection()
    {
        _dashDirection = _movementDirection;
        _movementDirection = Vector2.zero;
        _currentMovementState = MovementState.Idling;
        _animator.SetBool("Running", false);
    }

    public enum MovementState
    {
        Idling, Moving, Dashing
    }
}
