using BehaviorTree;

public class CheckTargetInRange : Node
{
    private TriggerDetection _triggerDetection;

    public CheckTargetInRange(TriggerDetection triggerDetection) : base()
    {
        _triggerDetection = triggerDetection;
    }

    public override NodeState Evaluate()
    {
        if(Root.GetData("Target") != null && _triggerDetection.IsTriggered)
        {
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}
