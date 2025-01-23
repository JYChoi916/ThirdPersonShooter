using System.Collections;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float lifeTime;
    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DestroyAfterSecond());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // Player 데미지;

            // 모든 코루틴을 멈추고
            StopAllCoroutines();

            // 자기자신을 파괴
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyAfterSecond()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
