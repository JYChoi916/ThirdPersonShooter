using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EnemyContext : MonoBehaviour 
{
    [HideInInspector] public GameObject player;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    public Animator rigController;
    public EnemyWeapon weapon;
    public WeaponAnimationEvents animationEvents;
    public MultiAimConstraint weaponAimConstraint;

    public Transform[] patrolPoints;

    public float sightRange = 25f;
    public float attackRange = 10f;
    public float attackDelay = 1f;

    public int hp;

    bool isDead;
    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }

    public void Start()
    {
        // 플레이어 찾기
        player = GameObject.Find("Player");

        // 플레이어에게 조준할 AimTarget 찾기
        Transform aimTargetTransform = Utility.RecursiveFindChild(player.transform, "Head");

        // Animator 컴포넌트 찾기
        animator = GetComponent<Animator>();

        // NavMeshAgent 컴포넌트 찾기
        agent = GetComponent<NavMeshAgent>();

        // AnimationEvent 리스너 등록
        animationEvents.weaponAnimationEvent.AddListener(OnAnimationEvent);

        // AimConstraint에 AimTarget 설정
        var data = weaponAimConstraint.data.sourceObjects;
        data.SetTransform(0, aimTargetTransform);
        weaponAimConstraint.data.sourceObjects = data;

        // RigBuilder로 Rig 생성
        GetComponent<RigBuilder>().Build();
    }

    public void Damage(int damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            isDead = true;
        }
    }

    void OnAnimationEvent(string eventName)
    {
        switch(eventName)
        {
            case "Shot":
                // 탄환 생성
                weapon.Shot();
                break;
        }
    }
}