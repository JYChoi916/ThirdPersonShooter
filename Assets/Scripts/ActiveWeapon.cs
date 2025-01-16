using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Animations;
using UnityEditor.Animations;


public class ActiveWeapon : MonoBehaviour
{
    public Transform crosshairTarget;
    public Rig handIK;
    public Transform weaponParent;
    public Transform weaponLeftGrip;
    public Transform weaponRightGrip;
    public Animator rigController;

    RaycastWeapon weapon;
    bool isHolstered = false;
    string equipAnimationStateName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RaycastWeapon existingWeapon = GetComponentInChildren<RaycastWeapon>();
        if (existingWeapon)
        {
            Equip(existingWeapon, existingWeapon.transform.localPosition);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (weapon)
        {
            bool canAim = rigController.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;

            if (Input.GetButtonDown("Fire1") && isHolstered == false && canAim)
            {
                weapon.StartFiring();
            }

            if (weapon.isFiring && isHolstered == false && canAim)
            {
                weapon.UpdateFiring(Time.deltaTime);
            }

            weapon.UpdateBullets(Time.deltaTime);

            if (Input.GetButtonUp("Fire1"))
            {
                weapon.StopFiring();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                isHolstered = !isHolstered;
            }

            rigController.SetBool("Holster_Weapon", isHolstered);
        }
    }

    public void Equip(RaycastWeapon newWeapon, Vector3 localPosition)
    {
        if (weapon)
        {
            Destroy(weapon.gameObject);
        }

        weapon = newWeapon;
        weapon.raycastDestination = crosshairTarget;
        weapon.transform.parent = weaponParent;
        weapon.transform.localPosition = localPosition;
        weapon.transform.localRotation = Quaternion.identity;
        isHolstered = false;
        equipAnimationStateName = "Equip_" + weapon.weaponAnimation;
        rigController.Play(equipAnimationStateName);
    }
}
