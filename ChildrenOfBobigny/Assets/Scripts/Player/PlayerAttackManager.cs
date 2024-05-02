using UnityEngine;
using static PlayerMovementManager;

public class PlayerAttackManager : Singleton<PlayerAttackManager>
{
    #region COMPONENTS
    private ControlsMap _controlsMap;
    [SerializeField] private Animator _graphAnimator;
    #endregion

    #region VARIABLES

    #region ACCESSEURS

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

        _controlsMap.Gameplay.Attack.performed += ctx => StartAttacking();
        _controlsMap.Gameplay.Attack.canceled += ctx => StopAttacking();
    }

    private void StartAttacking()
    {
        if(PlayerMovementManager.Instance.CurrentMovementState != MovementState.Dashing)
        {
            PlayerMovementManager.Instance.StopReadMovementDirection();
            PlayerMovementManager.Instance.CurrentMovementState = MovementState.Attacking;
            _graphAnimator.SetBool("Attacking", true);
        }
    }

    private void StopAttacking()
    {
        _graphAnimator.SetBool("Attacking", false);
    }

    public void IsAttackCancelled()
    {
        if (!_graphAnimator.GetBool("Attacking"))
        {
            AttackedFinished();
        }
    }

    public void AttackFinishing()
    {
        StopAttacking();
        AttackedFinished();
    }

    private void AttackedFinished()
    {
        PlayerMovementManager.Instance.CurrentMovementState = MovementState.Idling;
        if (_controlsMap.Gameplay.Movement.IsPressed())
        {
            PlayerMovementManager.Instance.ReadMovementDirection(PlayerMovementManager.Instance.MovementDirection);
        }
    }
}
