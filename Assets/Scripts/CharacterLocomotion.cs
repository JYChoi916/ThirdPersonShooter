using System;
using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    public Animator rigController;
    public float jumpHeight;
    public float gravity;
    public float stepDown;
    public float airControl;
    public float moveSpeed;
    public float pushPower = 2.0F;

    Animator animator;
    CharacterController characterController;
    CharacterAiming characterAiming;
    ActiveWeapon activeWeapon;
    ReloadWeapon reloadWeapon;
    Vector3 rootMotion;
    Vector3 velocity;

    bool isJumping;

    Vector2 input;
    int isSprintingParam = Animator.StringToHash("IsSprinting");

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        characterAiming = GetComponent<CharacterAiming>();
        activeWeapon = GetComponent<ActiveWeapon>();
        reloadWeapon = GetComponent<ReloadWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        // 입력 처리
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        // 입력 전달
        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);

        UpdateIsSprinting();

        if (Input.GetButton("Jump"))
        {
            Jump();
        }
    }

    bool IsSprinting()
    {
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        bool isFiring = activeWeapon.IsFiring();
        bool isReloading = reloadWeapon.isReloading;
        bool isChanging = activeWeapon.isChanging;
        bool isAiming = characterAiming.isAiming;
        return isSprinting && !isFiring && !isReloading && !isChanging && !isAiming;
    }

    private void UpdateIsSprinting()
    {
        bool isSprinting = IsSprinting();
        animator.SetBool(isSprintingParam, isSprinting);
        rigController.SetBool(isSprintingParam, isSprinting);
        rigController.SetInteger("WeaponIndex", activeWeapon.GetActiveWeaponIndex());
    }

    private void OnAnimatorMove()
    {
        rootMotion += animator.deltaPosition;
    }

    void FixedUpdate()
    {
        if (isJumping) {
            UpdateInAir();
        }
        else
        {              
            UpdateOnGround();
        }
    }

    private void UpdateOnGround()
    {
        Vector3 stepFowardAmount = rootMotion * moveSpeed;
        Vector3 stepDownAmount = Vector3.down * stepDown;

        characterController.Move(stepFowardAmount + stepDownAmount);
        rootMotion = Vector3.zero;

        if (characterController.isGrounded == false)
        {
            SetInAir(0);
        }
    }

    void UpdateInAir()
    {
        velocity.y -= gravity * Time.fixedDeltaTime;
        Vector3 displacement = velocity * Time.fixedDeltaTime;
        displacement += CalculateAirControl(Time.fixedDeltaTime);
        characterController.Move(displacement);
        isJumping = !characterController.isGrounded;
        rootMotion = Vector3.zero;
        animator.SetBool("IsJumping", isJumping);
    }

    Vector3 CalculateAirControl(float deltaTime)
    {
        return ((transform.forward * input.y) + (transform.right * input.x)) * (airControl / 100) * moveSpeed * deltaTime;
    }

    void Jump()
    {
        if (isJumping == false)
        {
            float jumpVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
            SetInAir(jumpVelocity);
        }
    }

    private void SetInAir(float jumpVelocity)
    {
        isJumping = true;
        velocity = animator.velocity;
        velocity.y = jumpVelocity;
        animator.SetBool("IsJumping", true);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
            return;

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3f)
            return;

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.linearVelocity = pushDir * pushPower;
    }
}
