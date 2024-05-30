using BehaviorTree;
using UnityEngine;

public class CheckTargetInRange : Node
{
    public override NodeState Evaluate()
    {
        return NodeState.FAILURE;
    }
}
