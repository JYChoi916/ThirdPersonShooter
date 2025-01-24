using System.Collections;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    // 발사체의 지속시간
    public float lifeTime;

    // 발사체의 이동속도
    public float speed;

    public GameObject impactEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 시작하는 순간 lifeTime 이후 삭제하도록 실행
        StartCoroutine(DestroyAfterSecond());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        // 임팩트 이펙트 발생
        Instantiate(impactEffect, transform.position, transform.rotation);

        if (other.tag == "Player")
        {
            // Player 데미지;

        }

        // 모든 코루틴을 멈추고
        StopAllCoroutines();

        // 자기자신을 파괴
        Destroy(gameObject);
    }

    // lifeTime 이후 자기 자신을 파괴
    IEnumerator DestroyAfterSecond()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
