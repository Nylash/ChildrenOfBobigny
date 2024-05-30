using BehaviorTree;
using UnityEngine;

public class CheckHasTarget : Node
{
    public override NodeState Evaluate()
    {
        if(Root.GetData("Target") != null)
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
