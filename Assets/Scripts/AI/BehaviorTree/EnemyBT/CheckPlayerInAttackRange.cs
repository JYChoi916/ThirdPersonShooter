using UnityEngine;

public class CheckPlayerInAttackRange : EnemyActionNode
{
    public CheckPlayerInAttackRange(EnemyContext context) : base(context) { }

    public override NodeState Evaluate()
    {
        float distanceToPlayer = Vector3.Distance(context.player.transform.position, context.transform.position);

        if (distanceToPlayer <= context.attackRange)
        {
            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}