using UnityEngine;
using Cinemachine;

public class WeaponRecoil : MonoBehaviour
{
    [HideInInspector] public CharacterAiming aiming;
    [HideInInspector] public CinemachineImpulseSource cameraShake;
    [HideInInspector] public Animator rigController;


    public float verticalRecoil;
    public float horizontalRecoil;
    public float duration;
    public float recoilModifier = 1.0f;

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
            aiming.yAxis.Value -= (verticalRecoil * Time.deltaTime) / duration * recoilModifier;
            aiming.xAxis.Value += (hRecoil * Time.deltaTime) / duration * recoilModifier;
            time -= Time.deltaTime;
        }
    }
}
