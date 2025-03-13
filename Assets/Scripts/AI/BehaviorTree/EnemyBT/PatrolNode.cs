using UnityEngine;

public class PatrolNode : EnemyActionNode
{
    int currentPoint = 0;
    public PatrolNode(EnemyContext context) : base(context) { }

    public override NodeState Evaluate()
    {
        // 플레이어가 시야 내에 있으면 실패
        float distToPlayer = Vector3.Distance(context.transform.position, context.playerTransform.position);
        if (distToPlayer <= context.sightRange)
            return NodeState.FAILURE;

        // Patrol 로직
        context.agent.isStopped = false;
        context.animator.SetBool("IsAttack", false);
        context.agent.destination = context.patrolPoints[currentPoint].position;

        if (Vector3.Distance(context.transform.position, context.patrolPoints[currentPoint].position) < 0.5f)
            currentPoint = (currentPoint + 1) % context.patrolPoints.Length;

        return NodeState.RUNNING;
    }
}