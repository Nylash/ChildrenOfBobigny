namespace BehaviorTree
{
    public class Sequence : Node
    {
        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;

            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        continue;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }
            
            state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }
    }
}