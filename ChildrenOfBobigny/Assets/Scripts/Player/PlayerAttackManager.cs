using System.Collections.Generic;
using UnityEngine;
using static Utilities;

public class PlayerAttackManager : Singleton<PlayerAttackManager>
{
    #region COMPONENTS
    private ControlsMap _controlsMap;
    [SerializeField] private Animator _graphAnimator;
    [SerializeField] private WeaponEvents _weaponEventsScript;
    #endregion

    #region VARIABLES
    private List<GameObject> _hitEnemies = new List<GameObject>();
    #region ACCESSORS

    #endregion
    #endregion

    #region CONFIGURATION
    [Header("CONFIGURATION")]
    [SerializeField] private Data_Player _playerData;
    #endregion

    private void OnEnable()
    {
        _controlsMap.Gameplay.Enable();
        _playerData.event_attackSpeedUpdated.AddListener(UpdateAttackSpeed);
        _weaponEventsScript.event_weaponHitsSomething.AddListener(WeaponHitSomething);
    }
    private void OnDisable()
    {
        _controlsMap.Gameplay.Disable();
        if (!this.gameObject.scene.isLoaded) return;//Avoid null ref
        _playerData.event_attackSpeedUpdated.RemoveListener(UpdateAttackSpeed);
        _weaponEventsScript.event_weaponHitsSomething.RemoveListener(WeaponHitSomething);
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
        if(PlayerMovementManager.Instance.CurrentMovementState != PlayerBehaviorState.DASH)
        {
            _graphAnimator.SetBool("Attacking", true);
            //Prevent moving while attacking
            PlayerMovementManager.Instance.StopReadMovementDirection();
            PlayerMovementManager.Instance.CurrentMovementState = PlayerBehaviorState.ATTACK;
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
    public void FinishAttack()
    {
        StopAttacking();
        AttackedFinished();
    }

    //Clear _hitEnnemies list at the start of a new attack, so we can hit everybody
    public void StartNewAttack()
    {
        _hitEnemies.Clear();
    }

    private void AttackedFinished()
    {
        PlayerMovementManager.Instance.CurrentMovementState = PlayerBehaviorState.IDLE;
        //If attack input still pressed we start attacking right away (remove this or add delay to modify gameplay)
        if (_controlsMap.Gameplay.Attack.IsPressed())
        {
            StartAttacking();
        }
        //If movement input is pressed we directly start using it
        else if (_controlsMap.Gameplay.Movement.IsPressed())
        {
            PlayerMovementManager.Instance.ReadMovementDirection(PlayerMovementManager.Instance.MovementDirection);
        }
    }

    private void UpdateAttackSpeed(float value)
    {
        _graphAnimator.SetFloat("AttackSpeed", value);
    }

    private void WeaponHitSomething(Collider other)
    {
        if (!_hitEnemies.Contains(other.gameObject))
        {
            if (other.CompareTag("Enemy"))
            {
                //Once a ennemy is hit we add it to the list so we avoid hitting him twice with one attack
                _hitEnemies.Add(other.gameObject);
                try
                {
                    other.gameObject.GetComponent<BasicEnemy_BT>().TakeDamage(_playerData.AttackDamage);
                }
                catch (System.Exception)
                {
                    Debug.LogError("Player weapon hit an enemy which doesn't have BasicEnemy script : " + other.gameObject.name);
                }
                return;
            }
            Debug.LogError("Player weapon hit something not handled : " + other.gameObject.name);
        }
    }
}
