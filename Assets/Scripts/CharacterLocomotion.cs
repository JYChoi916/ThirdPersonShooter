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
        // �Է� ���� �޾�
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        // �ִϸ��̼��� ���� �Ķ���ͷ� ����
        animator.SetFloat("InputX", inputX);
        animator.SetFloat("InputY", inputY);
    }
}
