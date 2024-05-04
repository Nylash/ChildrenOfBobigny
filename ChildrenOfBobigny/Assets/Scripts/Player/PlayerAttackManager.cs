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
        //Prevent attacking while dashing
        if(PlayerMovementManager.Instance.CurrentMovementState != MovementState.Dashing)
        {
            _graphAnimator.SetBool("Attacking", true);
            //Prevent moving while attacking
            PlayerMovementManager.Instance.StopReadMovementDirection();
            PlayerMovementManager.Instance.CurrentMovementState = MovementState.Attacking;
        }
    }

    private void StopAttacking()
    {
        _graphAnimator.SetBool("Attacking", false);
    }

    public void IsAttackCancelled()
    {
        //Is input release ? Animation event at the end of two firsts attacks
        if (!_graphAnimator.GetBool("Attacking"))
        {
            AttackedFinished();
        }
    }

    //Animation event at the end of the last attack
    public void AttackFinishing()
    {
        StopAttacking();
        AttackedFinished();
    }

    private void AttackedFinished()
    {
        PlayerMovementManager.Instance.CurrentMovementState = MovementState.Idling;
        /*//If attack input still pressed we start attacking right away (remove this or add delay to modify gameplay)
        if (_controlsMap.Gameplay.Attack.IsPressed())
        {
            StartAttacking();
        }
        //If movement input is pressed we directly start using it
        else */if (_controlsMap.Gameplay.Movement.IsPressed())
        {
            PlayerMovementManager.Instance.ReadMovementDirection(PlayerMovementManager.Instance.MovementDirection);
        }
    }

    private void UpdateAttackSpeed(float value)
    {
        _graphAnimator.SetFloat("AttackSpeed", value);
    }
}
