using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBT : BehaviorTree
{
    private EnemyContext context;

    private void Start()
    {
        ConstructBehaviourTree();
    }

    private void ConstructBehaviourTree()
    {
        context = GetComponent<EnemyContext>();

        root = new SelectorNode(new List<Node>
        {
            // 죽음 상태 체크
            new CheckDeadNode(context),
            
            // 공격 시퀀스
            new SequenceNode(new List<Node> 
            {
                new CheckAttackRangeNode(context),
                new AttackNode(context)
            }),
            
            // 추적
            new ChaseNode(context),
            
            // 정찰
            new PatrolNode(context)
        });
    }
}