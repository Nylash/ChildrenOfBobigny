using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static Utilities;

public class PlayerMovementManager : Singleton<PlayerMovementManager>
{
    #region COMPONENTS
    private ControlsMap _controlsMap;
    private CharacterController _controller;
    [SerializeField] private Animator _graphAnimator;
    #endregion

    #region VARIABLES
    private PlayerBehaviorState _currentBehavior = PlayerBehaviorState.IDLE;
    private Vector2 _movementDirection;

    #region ACCESSORS
    public PlayerBehaviorState CurrentMovementState { get => _currentBehavior; set => _currentBehavior = value; }
    public Vector2 MovementDirection { get => _movementDirection; }
    #endregion
    #endregion

    #region CONFIGURATION
    [Header("CONFIGURATION")]
    [SerializeField] private Data_Player _playerData;
    #endregion

    #region EVENTS
    public UnityEvent event_inputMovementIsStopped;
    #endregion

    private void OnEnable()
    {
        _controlsMap.Gameplay.Enable();
        if (event_inputMovementIsStopped == null)
            event_inputMovementIsStopped = new UnityEvent();
    } 
    private void OnDisable() => _controlsMap.Gameplay.Disable();

    protected override void OnAwake()
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
        //Basic movement using Unity CharacterController, and apply gravity on Y to avoid floating
        //Then set Angle float in running blend tree (angle between _movementDirection and AimDirection)
        if (_currentBehavior == PlayerBehaviorState.MOVE)
        {
            _controller.Move(new Vector3(_movementDirection.x, -_playerData.GravityForce, _movementDirection.y) * _playerData.MovementSpeed * Time.deltaTime);
            _graphAnimator.SetFloat("Angle", Mathf.Abs(Vector2.Angle(_movementDirection, PlayerAimManager.Instance.AimDirection)));
        }
        //Always applying gravity, also when not moving, only exception is while dashing
        else if (_currentBehavior != PlayerBehaviorState.DASH && !_controller.isGrounded)
        {
            _controller.Move(new Vector3(0, -_playerData.GravityForce, 0) * _playerData.MovementSpeed * Time.deltaTime);
        }
    }

    private IEnumerator Dashing()
    {
        //Prevent dashing while attacking/casting TODO : Instead this, cancel attack then dash
        if(_currentBehavior != PlayerBehaviorState.ATTACK && _currentBehavior != PlayerBehaviorState.CAST)
        {
            if (_playerData.DashIsReady)
            {
                StartDashing();

                float startTime = Time.time;
                //Replace Update movement logic by this during DashDuration
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
        _currentBehavior = PlayerBehaviorState.DASH;
        _playerData.DashIsReady = false;
    }

    private void StopDashing()
    {
        _graphAnimator.SetBool("Dashing", false);
        _currentBehavior = PlayerBehaviorState.IDLE;
        ////If movement input is pressed we directly start using it
        if (_controlsMap.Gameplay.Movement.IsPressed())
        {
            ReadMovementDirection(_movementDirection);
        }
    }

    public void ReadMovementDirection(Vector2 direction)
    {
        //Avoid dead zone reading (performed is called also in dead zone :/)
        if (direction != Vector2.zero)
        {
            //Update _movementDirection even while attacking, so we can aim with it
            _movementDirection = direction;
            if (_currentBehavior != PlayerBehaviorState.ATTACK && _currentBehavior != PlayerBehaviorState.CAST)
            {
                _currentBehavior = PlayerBehaviorState.MOVE;
                _graphAnimator.SetBool("Running", true);
            }
        }
    }

    public void StopReadMovementDirection()
    {
        //Prevent switch behavior by canceling movement input
        if (_currentBehavior != PlayerBehaviorState.ATTACK && _currentBehavior != PlayerBehaviorState.CAST)
        {
            _currentBehavior = PlayerBehaviorState.IDLE;
            _graphAnimator.SetBool("Running", false);
        }
        //Still notify this to update new LastStickDirection
        event_inputMovementIsStopped.Invoke();
    }
}
