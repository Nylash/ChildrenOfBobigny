using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

public class TaskReachTarget : Node
{
    private NavMeshAgent _navMeshAgent;

    public TaskReachTarget(NavMeshAgent navMeshAgent) : base()
    {
        _navMeshAgent = navMeshAgent;
    }

    public override NodeState Evaluate()
    {
        _navMeshAgent.SetDestination((Root.GetData("Target") as Transform).position);

        state = NodeState.RUNNING;
        return state;
    }
}
