using BehaviorTree;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minion_BT : BasicEnemy_BT
{
    [SerializeField] private NavMeshAgent _navMeshAgent;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            //Attack sequence
            new Sequence(new List<Node>
            {
                new CheckTargetInRange(),
                new TaskAttackTarget()
            }),
            //Movement sequence
            new Sequence(new List<Node>
            {
                new CheckHasTarget(),
                new TaskReachTarget(_navMeshAgent),
            }),
            new TaskIdle()
        });

        return root;
    }

    /*
    private List<GameObject> _hitUnits = new List<GameObject>();

    [SerializeField] private WeaponEvents _weaponEventsScript;
    [SerializeField] private NavMeshAgent _navMeshAgent;


    private void OnEnable()
    {
        _weaponEventsScript.event_weaponHitsSomething.AddListener(WeaponHitSomething);
    }
    private void OnDisable()
    {
        if (!this.gameObject.scene.isLoaded) return;//Avoid null ref
        _weaponEventsScript.event_weaponHitsSomething.RemoveListener(WeaponHitSomething);
    }
    
    START()
    InvokeRepeating("ResetHitUnits", 0, 3);

    //TODO Remove this and clear the list at the end of the attack animation
    private void ResetHitUnits()
    {
        _hitUnits.Clear();
    }

    private void WeaponHitSomething(Collider other)
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
    }*/
}
