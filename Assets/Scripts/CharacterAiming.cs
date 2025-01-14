using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAiming : MonoBehaviour
{
    public float turnSpeed = 15f;
    public float aimDuration = 0.3f;
    public Rig aimRigLayer;
    public GameObject laserDotObject;

    Camera mainCamera;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 카메라 트랜스폼의 Y값을 가지고
        float yawControl = mainCamera.transform.eulerAngles.y;

        // 카메라가 보는 방향으로 캐릭터를 회전 시킴
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawControl, 0), turnSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            aimRigLayer.weight += Time.deltaTime / aimDuration;
            laserDotObject.SetActive(true);
        }
        else
        {
            aimRigLayer.weight -= Time.deltaTime / aimDuration;
            laserDotObject.SetActive(false);
        }
    }
}
