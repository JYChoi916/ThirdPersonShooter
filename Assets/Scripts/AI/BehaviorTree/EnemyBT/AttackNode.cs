using UnityEngine;

public class AttackNode : EnemyActionNode
{
    private float elapsedTime = 0f;

    public AttackNode(EnemyContext context) : base(context) { }


    public override NodeState Evaluate()
    {
        context.agent.isStopped = true;
        context.animator.SetBool("IsAttack", true);
        context.rigController.SetBool("IsAttack", true);

        Vector3 direction = context.player.transform.position - context.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        context.transform.rotation = Quaternion.Slerp(context.transform.rotation, targetRotation, Time.deltaTime * context.agent.angularSpeed);

        elapsedTime += Time.deltaTime;

        if (elapsedTime > context.attackDelay)
        {
            // 적 탄 발사
            context.rigController.Play("Weapon_Recoil");

            // timer reset
            elapsedTime = 0f;
            return NodeState.Success;            
        }

        return NodeState.Running;
    }
}