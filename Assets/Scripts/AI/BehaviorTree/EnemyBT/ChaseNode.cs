using UnityEngine;
using UnityEngine.AI;

public class ChaseNode : EnemyActionNode
{
    public ChaseNode(EnemyContext context) : base(context) { }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(context.transform.position, context.playerTransform.position);
        
        // 공격 범위 안에 들어오면 Chase 중단
        if (distance <= context.attackRange)
            return NodeState.FAILURE;

        // Chase 실행
        context.agent.isStopped = false;
        context.animator.SetBool("IsAttack", false);
        context.agent.destination = context.playerTransform.position;

        return NodeState.RUNNING;
    }
}