using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Collections;
using Cinemachine;


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

    CharacterAiming aiming;

    public AmmoWidget ammoWidget;

    RaycastWeapon[] equipped_weapons = new RaycastWeapon[2];
    int activeWeaponIndex;

    public bool isHolstered = false;
    public bool isChanging = false;
    string equipAnimationStateName;
    ReloadWeapon reloadWeapon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RaycastWeapon existingWeapon = GetComponentInChildren<RaycastWeapon>();
        if (existingWeapon)
        {
            Equip(existingWeapon, existingWeapon.transform.localPosition);
        }

        reloadWeapon = GetComponent<ReloadWeapon>();
        aiming = GetComponent<CharacterAiming>();
    }

    public bool IsFiring()
    {
        var activeWeapon = GetActiveWeapon();

        if (activeWeapon == null)
        {
            return false;
        }

        return activeWeapon.isFiring;
    }

    RaycastWeapon GetWeapon(int index)
    {
        if (index < 0 || index >= equipped_weapons.Length)
            return null;

        return equipped_weapons[index];
    }

    public RaycastWeapon GetActiveWeapon()
    {
        return GetWeapon(activeWeaponIndex);
    }

    public int GetActiveWeaponIndex()
    {
        return activeWeaponIndex;
    }

    // Update is called once per frame
    void Update()
    {
        var weapon = GetWeapon(activeWeaponIndex);

        // To do :리로드 중에는 아래 로직을 모두 하지 말아야 한다.
        if (weapon)
        {
            if (reloadWeapon.isReloading == false)
            {
                if (Input.GetButtonDown("Fire1") && isHolstered == false)
                {
                    weapon.StartFiring();
                }

                if (weapon.isFiring && isHolstered == false)
                {
                    weapon.UpdateFiring(Time.deltaTime);
                }
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

    public void Equip(RaycastWeapon weaponPrefab, Vector3 localPosition)
    {
        var currentWeapon = GetWeapon(activeWeaponIndex);
        if (currentWeapon != null && currentWeapon.weaponName == weaponPrefab.weaponName && isHolstered == false)
        {
            return;
        }

        RaycastWeapon newWeapon = Instantiate(weaponPrefab);

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
        weapon.recoil.aiming = aiming;
        weapon.recoil.rigController = rigController;
        equipped_weapons[weaponSlotIndex] = weapon;

        handIK.weight = 1.0f;

        SetActiveWeapon(weapon.weaponSlot);

        ammoWidget.Refresh(weapon.ammoCount, weapon.clipSize);
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
        var weapon = GetActiveWeapon();
    }

    IEnumerator HolsterWeapon(int index)
    {
        isHolstered = true;
        isChanging = true;
        var weapon = GetWeapon(index);
        if (weapon)
        {
            rigController.SetBool("Holster_Weapon", true);
            do
            {
                yield return new WaitForSeconds(0.05f);
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
        isChanging = false;
    }

    IEnumerator ActivateWeapon(int index)
    {
        isChanging = true;
        var weapon = GetWeapon(index);
        if (weapon)
        {
            equipAnimationStateName = "Equip_" + weapon.weaponName;
            rigController.SetBool("Holster_Weapon", false);
            rigController.Play(equipAnimationStateName);
            do
            {
                yield return new WaitForSeconds(0.05f);
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
            isHolstered = false;
        }
        isChanging = false;
    }
}
