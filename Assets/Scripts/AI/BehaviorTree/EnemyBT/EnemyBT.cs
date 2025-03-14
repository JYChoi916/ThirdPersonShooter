using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class EnemyBT : BehaviorTree
{
    private EnemyContext context;

    void Start()
    {
        ConstructBehaviorTree();
    }

    void ConstructBehaviorTree()
    {
        context = GetComponent<EnemyContext>();

        // root노드 부터 시작해 leaf노드까지 생성
        rootNode = new SelectorNode(new List<Node>
        {
            // CheckDead Node (죽음 상태 체크)
            new CheckDeadNode(context),

            // 플레이어 감지 후 행동하는 Sequence Node 
            new SequenceNode(new List<Node>
            {
                // 플레이어 감지 체크 노드
                new CheckPlayerInSightNode(context),

                // 플레이어 발견 후 공격 또는 추적 선택할 Selector Node
                new SelectorNode(new List<Node>
                {
                    // 공격 Sequence Node
                    new SequenceNode(new List<Node>
                    {
                        // 공격 사거리내에 플레이어 존재 체크 노드
                        new CheckPlayerInAttackRange(context),

                        // 공격 노드
                        new AttackNode(context),
                    }),

                    // 추적 노드
                    new ChaseNode(context),
                })
            }), 

            // Patrol Node
            new PatrolNode(context),
        });
    }
}