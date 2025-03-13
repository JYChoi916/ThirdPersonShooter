using System.Collections.Generic;

public enum NodeState
{
    SUCCESS,
    FAILURE,
    RUNNING
}

public abstract class Node
{
    protected NodeState state;
    public Node parent;
    protected List<Node> children = new List<Node>();
    
    public virtual NodeState Evaluate()
    {
        return state;
    }
}
public class SelectorNode : Node
{
    public SelectorNode(List<Node> children)
    {
        this.children = children;
        foreach (Node child in children)
        {
            child.parent = this;
        }
    }

    public override NodeState Evaluate()
    {
        foreach (Node node in children)
        {
            state = node.Evaluate();
            if (state == NodeState.SUCCESS || state == NodeState.RUNNING)
                return state;
        }
        return NodeState.FAILURE;
    }
}

public class SequenceNode : Node
{
    public SequenceNode(List<Node> children)
    {
        this.children = children;
        foreach (Node child in children)
        {
            child.parent = this;
        }
    }

    public override NodeState Evaluate()
    {
        foreach (Node node in children)
        {
            state = node.Evaluate();
            if (state == NodeState.FAILURE || state == NodeState.RUNNING)
                return state;
        }
        return NodeState.SUCCESS;
    }
}