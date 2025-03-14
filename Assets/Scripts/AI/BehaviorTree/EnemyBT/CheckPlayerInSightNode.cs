using UnityEngine;

public class CheckPlayerInSightNode : EnemyActionNode
{
    public CheckPlayerInSightNode(EnemyContext context) : base(context) { }

    public override NodeState Evaluate()
    {
        float distanceToPlayer = Vector3.Distance(context.player.transform.position, context.transform.position);

        if (distanceToPlayer <= context.sightRange)
        {
            return NodeState.Success;
        }
        
        return NodeState.Failure;
    }
}