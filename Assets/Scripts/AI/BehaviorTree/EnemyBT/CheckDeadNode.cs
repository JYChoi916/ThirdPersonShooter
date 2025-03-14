using UnityEngine;

public class CheckDeadNode : EnemyActionNode 
{
    public CheckDeadNode(EnemyContext context) : base(context) { }

    public override NodeState Evaluate()
    {
        if (context.IsDead)
        {
            context.animator.Play("Dying");
            context.rigController.Play("Dying");
            context.agent.isStopped = true;
            if (context.weapon)
            {
                GameObject.Destroy(context.weapon.gameObject);
                context.weapon = null;
            }

            GameObject.Destroy(context.gameObject, 5f);
            return NodeState.Success;
        }
        else
        {
            return NodeState.Failure;
        }
    }
}