using UnityEngine;

public class PatrolNode : EnemyActionNode
{
    public PatrolNode(EnemyContext context) : base(context) { }

    private int currentPatrolPointIndex = 0;

    public override NodeState Evaluate()
    {
        context.agent.speed = 1.5f;
        context.agent.isStopped = false;
        context.animator.SetBool("IsAttack", false);
        context.rigController.SetBool("IsDetected", false);
        context.rigController.SetBool("IsAttack", false);
        context.animator.SetFloat("Speed", context.agent.speed);
        
        if (context.agent.remainingDistance <= context.agent.stoppingDistance)
        {
            currentPatrolPointIndex = (currentPatrolPointIndex + 1) % context.patrolPoints.Length;
        }

        context.agent.destination = context.patrolPoints[currentPatrolPointIndex].position;

        return NodeState.Running;
    }
}