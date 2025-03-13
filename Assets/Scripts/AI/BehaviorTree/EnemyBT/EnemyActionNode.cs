using UnityEngine;
using UnityEngine.AI;

public class EnemyActionNode : Node
{
    protected EnemyContext context;

    public EnemyActionNode(EnemyContext context)
    {
        this.context = context;
    }
}