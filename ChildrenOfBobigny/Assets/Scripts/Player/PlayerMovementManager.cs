using System.Collections;
using UnityEngine;

public class PlayerMovementManager : Singleton<PlayerMovementManager>
{
    #region COMPONENTS
    private ControlsMap _controlsMap;
    private CharacterController _controller;
    [SerializeField] private Animator _graphAnimator;
    #endregion

    #region VARIABLES
    private MovementState _currentMovementState = MovementState.Idling;
    private Vector2 _movementDirection;

    #region ACCESSEURS
    public MovementState CurrentMovementState { get => _currentMovementState; set => _currentMovementState = value; }
    public Vector2 MovementDirection { get => _movementDirection; }
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

        _controlsMap.Gameplay.Movement.performed += ctx => ReadMovementDirection(ctx.ReadValue<Vector2>().normalized);
        _controlsMap.Gameplay.Movement.canceled += ctx => StopReadMovementDirection();
        _controlsMap.Gameplay.Dash.performed += ctx => StartCoroutine(Dashing());

        _controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        _playerData.DashIsReady = true;
    }

    private void Update()
    {
        if(_currentMovementState == MovementState.Moving)
        {
            _controller.Move(new Vector3(_movementDirection.x, -_playerData.GravityForce, _movementDirection.y) * _playerData.MovementSpeed * Time.deltaTime);
            _graphAnimator.SetFloat("Angle", Mathf.Abs(Vector2.Angle(_movementDirection, PlayerAimManager.Instance.AimDirection)));
        }
        else if (_currentMovementState != MovementState.Dashing && !_controller.isGrounded)
        {
            _controller.Move(new Vector3(0, -_playerData.GravityForce, 0) * _playerData.MovementSpeed * Time.deltaTime);
        }
    }

    private IEnumerator Dashing()
    {
        if(_currentMovementState != MovementState.Attacking)
        {
            if (_playerData.DashIsReady)
            {
                StartDashing();
                float startTime = Time.time;
                while (Time.time < startTime + _playerData.DashDuration)
                {
                    _controller.Move(new Vector3(_movementDirection.x, 0, _movementDirection.y) * _playerData.DashSpeed * Time.deltaTime);
                    yield return null;
                }
                StopDashing();
                yield return new WaitForSeconds(_playerData.DashCD - _playerData.DashDuration);
                _playerData.DashIsReady = true;
            }
        }
    }

    private void StartDashing()
    {
        _graphAnimator.SetBool("Dashing", true);
        _currentMovementState = MovementState.Dashing;
        _playerData.DashIsReady = false;
    }

    private void StopDashing()
    {
        _graphAnimator.SetBool("Dashing", false);
        _currentMovementState = MovementState.Idling;
        if (_controlsMap.Gameplay.Movement.IsPressed())
        {
            ReadMovementDirection(_movementDirection);
        }
    }

    public void ReadMovementDirection(Vector2 direction)
    {
        _movementDirection = direction;
        if (_currentMovementState != MovementState.Attacking)
        {
            _currentMovementState = MovementState.Moving;
            _graphAnimator.SetBool("Running", true);
        }
    }

    public void StopReadMovementDirection()
    {
        if (_currentMovementState != MovementState.Attacking)
        {
            _currentMovementState = MovementState.Idling;
            _graphAnimator.SetBool("Running", false);
        }
    }

    public enum MovementState
    {
        Idling, Moving, Dashing, Attacking
    }
}
