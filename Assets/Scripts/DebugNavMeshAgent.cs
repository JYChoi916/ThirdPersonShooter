using UnityEngine;
using UnityEngine.AI;

public class DebugNavMeshAgent : MonoBehaviour
{
    public bool showVelocity;
    public bool showDesiredVelocity;
    public bool showPath;

    NavMeshAgent agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            if (showVelocity)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, transform.position + agent.velocity);
            }

            if (showDesiredVelocity)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, transform.position + agent.desiredVelocity);
            }

            if (showPath)
            {
                Gizmos.color = Color.black;
                NavMeshPath path = agent.path;
                Vector3 prevConner = transform.position;
                prevConner.y = 0.5f;
                foreach (var conner in path.corners)
                {
                    Vector3 nextConner = conner;
                    nextConner.y = 0.5f;
                    Gizmos.DrawLine(prevConner, nextConner);
                    Gizmos.DrawSphere(nextConner, 0.1f);
                    prevConner = nextConner;
                }
            }
        }
    }
}
