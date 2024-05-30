using BehaviorTree;
using UnityEngine;

public class TaskAttackTarget : Node
{
    public override NodeState Evaluate()
    {
        return NodeState.FAILURE;
    }
}
