using BehaviourTree;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minion_BT : BasicEnemy_BT
{
    [Header("COMPONENTS")]
    [SerializeField] private TriggerDetection _rangeTrigger;
    [SerializeField] private WeaponEvents _weaponEvents;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            //Attack sequence
            new Sequence(new List<Node>
            {
                new CheckTargetInRange(_rangeTrigger),
                new TaskAttackTarget(_enemyData, _weaponEvents, GetComponent<Animator>(), GetComponent<EnemyAnimationEvents>())
            }),
            //Movement sequence
            new Sequence(new List<Node>
            {
                new CheckHasTarget(),
                new TaskReachTarget(GetComponent<NavMeshAgent>()),
            }),
            new TaskIdle()
        });

        return root;
    }
}
