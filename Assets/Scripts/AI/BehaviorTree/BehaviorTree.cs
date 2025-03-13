using UnityEngine;
public abstract class BehaviorTree : MonoBehaviour
{
    protected Node root = null;

    protected void Update()
    {
        if (root != null)
            root.Evaluate();
    }
}