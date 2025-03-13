using UnityEngine;
public class CheckAttackRangeNode : EnemyActionNode
{
    public CheckAttackRangeNode(EnemyContext context) : base(context) { }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(context.transform.position, context.playerTransform.position);
        return distance <= context.attackRange ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}