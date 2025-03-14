public class ChaseNode :EnemyActionNode
{
    public ChaseNode(EnemyContext context) : base(context) { }

    public override NodeState Evaluate()
    {
        context.agent.isStopped = false;
        context.agent.speed = 4.0f;
        context.animator.SetBool("IsAttack", false);
        context.agent.destination = context.player.transform.position;
        context.rigController.SetBool("IsDetected", true);
        context.rigController.SetBool("IsAttack", false);
        context.animator.SetFloat("Speed", context.agent.speed);

        return NodeState.Running;
    }
}