using UnityEngine;
using UnityEngine.AI;

public class CheckDeadNode : EnemyActionNode
{
    public CheckDeadNode(EnemyContext context) : base(context) { }

    public override NodeState Evaluate()
    {
        if (context.IsDead)
        {
            context.animator.Play("Dying");
            if (context.weapon) GameObject.Destroy(context.weapon.gameObject);
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}