using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocomotion : MonoBehaviour
{
    public enum EnemyState
    {
        Stopped,        // 멈춰 있을 때 (Test용)
        Patrol,         // 플레이어를 발견하지 못했을 때
        Trace,
        Attack,
        Die,
    }

    public EnemyState state;

    public float sightRange;
    public float attackRange;
    public float attackDelay = 1f;

    public Animator rigController;
    public EnemyWeapon weapon;
    public WeaponAnimationEvents animationEvents;

    Vector3 startingPoint;
    Quaternion startingRotation;

    GameObject player;
    NavMeshAgent agent;
    Animator animator;

    Coroutine attackCoroutine = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        state = EnemyState.Stopped;
        startingPoint = transform.position;
        startingRotation = transform.rotation;
        animationEvents.weaponAnimationEvent.AddListener(OnAnimationEvent);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();

        switch(state)
        {
            case EnemyState.Stopped:
                Stay();
                break;
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Trace:
                TracePlayer();
                break;
            case EnemyState.Attack:
                StartAttack();
                break;
        }
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    void UpdateState()
    {
        // 플레이어가 시야 안에 들어왔다면
        if (CheckPlayerInSight())
        {
            // 공격 사거리 안에 들어왔다면
            if (CheckPlayerInAttackRange())
            {
                // 공격 상태
                state = EnemyState.Attack;
            }
            else
            {
                if (attackCoroutine != null)
                {
                    StopCoroutine(attackCoroutine);
                    attackCoroutine = null;
                }

                rigController.SetBool("IsAttack", false);
                rigController.Play("Aiming");
                // 공격 사거리 밖이라면 Trace 상태로 만들어 주자
                state = EnemyState.Trace;
            }
        }
        else
        // 아니라면
        {
            // 정찰 루트가 있다면
            //if ()
            //state = EnemyState.Patrol;
            // 아니라면
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }

            rigController.SetBool("IsAttack", false);
            rigController.Play("Aiming");
            state = EnemyState.Stopped;
        }
    }

    bool CheckPlayerInSight()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= sightRange)
        {
            Vector3 toPlayer = player.transform.position - transform.position;

            // 이 캐릭터가 보고 있는 방향에 플레이가 있는지 체크 (내적연산)
            float dot = Vector3.Dot(transform.forward, toPlayer);

            // 내적의 결과가 0보다 크면 앞에 아니라면 뒤에 있으므로 해당 결과를 리턴
            return dot > 0;
        }

        return false;
    }

    bool CheckPlayerInAttackRange()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance <= attackRange;
    }

    void Stay() // 정찰 패턴이 없는 경우
    {
        agent.destination = startingPoint;
        agent.stoppingDistance = 0.1f;

        // 현재 위치가 최초 위치와 다르다면
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            agent.isStopped = false;
        }
        else
        {
            agent.isStopped = true;
            transform.rotation = Quaternion.Slerp(transform.rotation, startingRotation, Time.deltaTime * agent.angularSpeed);
        }
    }

    void Patrol()
    {
        // 정해진 루트를 반복적으로 쫓아가도록 합니다.
        agent.stoppingDistance = 0.1f;
        agent.isStopped = false;
    }

    void TracePlayer()
    {
        agent.stoppingDistance = attackRange;
        agent.isStopped = false;
        // 플레이어를 따라갑니다.
        agent.destination = player.transform.position;
    }

    void StartAttack()
    {
        // 제자리에 멈추고 공격 합니다.
        agent.isStopped = true;
        if (attackCoroutine == null)
        {
            rigController.SetBool("IsAttack", true);
            attackCoroutine = StartCoroutine(Attack());
        }

        Vector3 direction = player.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * agent.angularSpeed);
    }

    IEnumerator Attack()
    {
        do
        {
            if (state != EnemyState.Attack)
                break;

            yield return new WaitForSeconds(0.05f);
        }
        while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);


        while (state == EnemyState.Attack) 
        {
            if (state != EnemyState.Attack)
                break;

            // 적 탄 발사
            EnemyFire();

            yield return new WaitForSeconds(attackDelay);
        }

        attackCoroutine = null;
    }

    void EnemyFire()
    {
        // 반동 애니메이션
        rigController.Play("Weapon_Recoil");
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
