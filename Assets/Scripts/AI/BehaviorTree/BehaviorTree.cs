using UnityEngine;

public abstract class BehaviorTree : MonoBehaviour
{
    protected Node rootNode;

    protected void Update()
    {
        if (rootNode != null)
        {
            rootNode.Evaluate();
        }
    }
}