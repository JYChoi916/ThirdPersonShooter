public class EnemyActionNode : Node
{
    protected EnemyContext context;

    public EnemyActionNode(EnemyContext context)
    {
        this.context = context;
    }

    public override NodeState Evaluate()
    {
        return state;
    }
}