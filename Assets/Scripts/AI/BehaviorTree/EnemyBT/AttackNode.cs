using UnityEngine;
using UnityEngine.AI;

public class AttackNode : EnemyActionNode
{
    float lastAttackTime = 0f;

    public AttackNode(EnemyContext context) : base(context) { }

    public override NodeState Evaluate()
    {
        // 공격 범위를 벗어났는지 체크
        float distance = Vector3.Distance(context.transform.position, context.playerTransform.position);
        if (distance > context.agent.stoppingDistance)
            return NodeState.FAILURE;

        // 적 방향으로 회전
        Vector3 direction = context.playerTransform.position - context.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        context.transform.rotation = Quaternion.Slerp(
            context.transform.rotation,
            lookRotation,
            Time.deltaTime * context.agent.angularSpeed
        );

        // 공격 실행
        context.agent.isStopped = true;
        context.animator.SetBool("IsAttack", true);

        if (Time.time - lastAttackTime >= context.attackDelay)
        {
            context.weapon.Shot();
            context.animator.Play("Weapon_Recoil", 0, 0f);
            lastAttackTime = Time.time;
        }

        return NodeState.RUNNING;
    }
}