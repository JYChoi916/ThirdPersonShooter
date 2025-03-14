using System.Collections.Generic;

public enum NodeState
{
    Success,
    Failure,
    Running,
}

public abstract class Node
{
    public NodeState state { get; protected set; }

    public Node parent;

    protected List<Node> children = new List<Node>();

    public abstract NodeState Evaluate();
}

public class SelectorNode : Node
{
    public SelectorNode(List<Node> children)
    {
        this.children = children;
        foreach(Node node in children)
        {
            node.parent = this;
        }
    }

    public override NodeState Evaluate()
    {
        foreach (Node node in children)
        {
            state = node.Evaluate();
            if (state == NodeState.Success || state == NodeState.Running)
            {
                return state;
            }
        }
        return NodeState.Failure;
    }
}

public class SequenceNode : Node 
{
    public SequenceNode(List<Node> children)
    {
        this.children = children;
        foreach(Node node in children)
        {
            node.parent = this;
        }
    }

    public override NodeState Evaluate()
    {
        foreach (Node node in children)
        {
            state = node.Evaluate();
            if (state == NodeState.Failure || state == NodeState.Running)
            {
                return state;
            }
        }
        
        return NodeState.Success;
    }
}