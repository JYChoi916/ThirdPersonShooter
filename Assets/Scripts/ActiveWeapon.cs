using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Collections;
using Unity.Cinemachine;


public class ActiveWeapon : MonoBehaviour
{
    public enum WeaponSlot
    {
        Primary,
        Secondary
    }

    public Transform crosshairTarget;
    public Rig handIK;
    public Transform weaponLeftGrip;
    public Transform weaponRightGrip;
    public Transform[] weaponSlots;

    public Animator rigController;

    public CinemachineOrbitalFollow playerCamera;


    RaycastWeapon[] equipped_weapons = new RaycastWeapon[2];
    int activeWeaponIndex;

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

    RaycastWeapon GetWeapon(int index)
    {
        if (index < 0 || index >= equipped_weapons.Length)
            return null;

        return equipped_weapons[index];
    }

    // Update is called once per frame
    void Update()
    {
        var weapon = GetWeapon(activeWeaponIndex);

        if (weapon)
        {
            if (Input.GetButtonDown("Fire1") && isHolstered == false)
            {
                weapon.StartFiring();
            }

            if (weapon.isFiring && isHolstered == false)
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
                ToggleActiveWeapon();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetActiveWeapon(WeaponSlot.Primary);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetActiveWeapon(WeaponSlot.Secondary);
        }
    }

    public void Equip(RaycastWeapon newWeapon, Vector3 localPosition)
    {
        var currentWeapon = GetWeapon(activeWeaponIndex);
        if (currentWeapon != null && currentWeapon.weaponName == newWeapon.weaponName && isHolstered == false)
        {
            return;
        }

        int weaponSlotIndex = (int)newWeapon.weaponSlot;
        var weapon = GetWeapon(weaponSlotIndex);

        if (weapon)
        {
            Destroy(weapon.gameObject);
        }

        weapon = newWeapon;
        weapon.raycastDestination = crosshairTarget;
        weapon.transform.parent = weaponSlots[weaponSlotIndex];
        weapon.transform.localPosition = localPosition;
        weapon.transform.localRotation = Quaternion.identity;
        weapon.recoil.playerCamera = playerCamera;
        weapon.recoil.rigController = rigController;
        equipped_weapons[weaponSlotIndex] = weapon;

        SetActiveWeapon(weapon.weaponSlot);
    }

    void ToggleActiveWeapon()
    {
        if (isHolstered)
        {
            StartCoroutine(ActivateWeapon(activeWeaponIndex));
        }
        else
        {
            StartCoroutine(HolsterWeapon(activeWeaponIndex));
        }
    }

    void SetActiveWeapon(WeaponSlot weaponSlot)
    {
        int holsterIndex = activeWeaponIndex;
        int activeIndex = (int)weaponSlot;
            
        if (holsterIndex == activeIndex)
        {
            holsterIndex = -1;
        }

        StartCoroutine(SwitchWeapon(holsterIndex, activeIndex));
    }

    IEnumerator SwitchWeapon(int holsterIndex, int activeIndex)
    {
        yield return StartCoroutine(HolsterWeapon(holsterIndex));
        yield return StartCoroutine(ActivateWeapon(activeIndex));
        activeWeaponIndex = activeIndex;
    }

    IEnumerator HolsterWeapon(int index)
    {
        isHolstered = true;
        var weapon = GetWeapon(index);
        if (weapon)
        {
            rigController.SetBool("Holster_Weapon", true);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
    }

    IEnumerator ActivateWeapon(int index)
    {
        var weapon = GetWeapon(index);
        if (weapon)
        {
            equipAnimationStateName = "Equip_" + weapon.weaponName;
            rigController.SetBool("Holster_Weapon", false);
            rigController.Play(equipAnimationStateName);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
            isHolstered = false;
        }
    }
}
