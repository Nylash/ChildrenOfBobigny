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

    private void OnEnable()
    {
        _controlsMap.Gameplay.Enable();
        _playerData.event_attackSpeedUpdated.AddListener(UpdateAttackSpeed);
    }
    private void OnDisable()
    {
        _controlsMap.Gameplay.Disable();
        _playerData.event_attackSpeedUpdated.RemoveListener(UpdateAttackSpeed);
    }

    protected override void OnAwake()
    {
        _controlsMap = new ControlsMap();

        _controlsMap.Gameplay.Attack.performed += ctx => StartAttacking();
        _controlsMap.Gameplay.Attack.canceled += ctx => StopAttacking();

        UpdateAttackSpeed(_playerData.AttackSpeed);
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
        if (_controlsMap.Gameplay.Attack.IsPressed())
        {
            StartAttacking();
        }
        else if (_controlsMap.Gameplay.Movement.IsPressed())
        {
            PlayerMovementManager.Instance.ReadMovementDirection(PlayerMovementManager.Instance.MovementDirection);
        }
    }

    private void UpdateAttackSpeed(float value)
    {
        _graphAnimator.SetFloat("AttackSpeed", value);
    }
}
