using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EnemyContext : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Transform playerTransform;
    public EnemyWeapon weapon;
    public Animator rigController;
    public Transform[] patrolPoints;
    
    public float sightRange;
    public float attackRange;
    public float attackDelay;

    public int hp;

    bool isDead;
    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }

    public void Damage(int damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            isDead = true;
        }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        weapon = GetComponent<EnemyWeapon>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        GetComponent<RigBuilder>().Build();
    }
}