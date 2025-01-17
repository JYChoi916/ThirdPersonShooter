using UnityEngine;
using Unity.Cinemachine;

public class WeaponRecoil : MonoBehaviour
{
    [HideInInspector] public CinemachineOrbitalFollow playerCamera;
    [HideInInspector] public CinemachineImpulseSource cameraShake;
    [HideInInspector] public Animator rigController;


    public float verticalRecoil;
    public float horizontalRecoil;
    public float duration;

    float time;

    void Awake()
    {
        cameraShake = GetComponent<CinemachineImpulseSource>();
    }

    public void GenerateRecoil(string weaponName)
    {
        time = duration;

        cameraShake.GenerateImpulse(Camera.main.transform.forward);
        rigController.Play("Weapon_Recoil_" + weaponName, 1, 0.0f);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            float hRecoil = Random.Range(-horizontalRecoil, horizontalRecoil);
            playerCamera.VerticalAxis.Value -= (verticalRecoil * Time.deltaTime) / duration;
            playerCamera.HorizontalAxis.Value += (hRecoil * Time.deltaTime) / duration;
            time -= Time.deltaTime;
        }
    }
}
