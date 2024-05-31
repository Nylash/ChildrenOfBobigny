using BehaviourTree;
using System.Collections.Generic;
using UnityEngine;

public class TaskAttackTarget : Node
{
    private WeaponEvents _weaponEvents;
    private Animator _animator;
    private EnemyAnimationEvents _animationEvents;

    private List<GameObject> _hitUnits = new List<GameObject>();
    private Data_BasicEnemy _enemyData;

    public TaskAttackTarget(Data_BasicEnemy enemyData, WeaponEvents weaponEvents, Animator animator, EnemyAnimationEvents animationEvents) : base()
    {
        _enemyData = enemyData;
        _weaponEvents = weaponEvents;
        _animator = animator;
        _animationEvents = animationEvents;

        _weaponEvents.event_weaponHitsSomething.AddListener(WeaponHitSomething);
    }

    public override NodeState Evaluate()
    {
        //Launch an attack if it's not already playing
        if (!_animationEvents.IsAttacking)
        {
            _animator.SetTrigger("Attack");
            _animationEvents.IsAttacking = true;
            _hitUnits.Clear();
        }

        state = NodeState.RUNNING;
        return state;
    }

    void WeaponHitSomething(Collider other)
    {
        if (!_hitUnits.Contains(other.gameObject))
        {
            if (other.CompareTag("Player"))
            {
                //Once player is hit we add it to the list so we avoid hitting him twice with one attack
                _hitUnits.Add(other.gameObject);
                try
                {
                    PlayerHealthManager.Instance.TakeDamage(_enemyData.AttackDamage);
                }
                catch (System.Exception)
                {
                    Debug.LogError("Enemy weapon hit Player which doesn't have PlayerHealthManager script : " + other.gameObject.name);
                }
                return;
            }
            Debug.LogError("Enemy weapon hit something not handled : " + other.gameObject.name);
        }
    }
}
