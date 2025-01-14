using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 입력 값을 받아
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        // 애니메이션의 블렌딩 파라미터로 전달
        animator.SetFloat("InputX", inputX);
        animator.SetFloat("InputY", inputY);
    }
}
