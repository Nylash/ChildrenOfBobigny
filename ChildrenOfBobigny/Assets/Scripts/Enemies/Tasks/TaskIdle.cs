using BehaviorTree;
using System.Collections.Generic;
using UnityEngine;

public class TaskIdle : Node
{
    public override NodeState Evaluate()
    {
        state = NodeState.SUCCESS;
        return state;
    }
}
