using UnityEngine;

public class ReloadWeapon : MonoBehaviour
{
    public Animator rigController;
    public WeaponAnimationEvents animationEvents;
    public ActiveWeapon activeWeapon;
    public Transform leftHand;
    public Transform revolverMagTransform;
    public AmmoWidget ammoWidget;
    public bool isReloading;

    GameObject magazineHand;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animationEvents.weaponAnimationEvent.AddListener(OnAnimationEvent);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();
        if (weapon && activeWeapon.isHolstered == false)
        {
            if (Input.GetKeyDown(KeyCode.R) || weapon.ammoCount <= 0)
            {
                isReloading = true;
                rigController.SetTrigger("Reload_Weapon");
            }

            if (weapon.isFiring)
            {
                ammoWidget.Refresh(weapon.ammoCount, weapon.clipSize);
            }
        }
    }

    void OnAnimationEvent(string eventName)
    {
        switch(eventName)
        {
            case "PlayReloadSound":
                PlayReloadSound();
                break;
            case "OpenMagazine":
                OpenMagazine();
                break;
            case "DetachMagzineFromWeapon":
                DetachMagzineFromRifle();
                break;
            case "DropMagazine":
                DropMagazine();
                break;
            case "DropMagazineFromPistol":
                DropMagazineFromPistol();
                break;
            case "CreateNewMagazine":
                CreateNewMagazine();
                break;
            case "AttachMagazineToWeapon":
                AttachMagazineToWeapon();
                break;
            case "CloseMagazine":
                CloseMagazine();
                break;
        }
    }

    void OpenMagazine()
    {
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();
        weapon.OpenMagazine(true);
    }

    void DetachMagzineFromRifle()
    {
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();
        magazineHand = Instantiate(weapon.magazine, leftHand, true);
        weapon.magazine.SetActive(false);
    }

    void DropMagazine()
    {
        GameObject droppedMagazine = Instantiate(magazineHand, magazineHand.transform.position, magazineHand.transform.rotation);
        droppedMagazine.AddComponent<Rigidbody>();
        droppedMagazine.AddComponent<BoxCollider>();
        magazineHand.SetActive(false);
    }

    void DropMagazineFromPistol()
    {
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();
        GameObject droppedMagazine = Instantiate(weapon.magazine, weapon.magazine.transform.position, weapon.magazine.transform.rotation);
        droppedMagazine.AddComponent<Rigidbody>();
        droppedMagazine.AddComponent<BoxCollider>();
        weapon.magazine.SetActive(false);
    }

    void CreateNewMagazine()
    {
        if (magazineHand != null)
        {
            magazineHand.SetActive(true);
        }
        else
        {
            RaycastWeapon weapon = activeWeapon.GetActiveWeapon();
            magazineHand = Instantiate(weapon.magazine, revolverMagTransform);
            magazineHand.transform.localPosition = Vector3.zero;
            magazineHand.transform.localRotation = Quaternion.identity;
            magazineHand.SetActive(true);
        }
    }

    void AttachMagazineToWeapon()
    {
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();
        weapon.magazine.SetActive(true);
        Destroy(magazineHand);
        rigController.ResetTrigger("Reload_Weapon");
        weapon.ammoCount = weapon.clipSize;
        ammoWidget.Refresh(weapon.ammoCount, weapon.clipSize);
        isReloading = false;
    }

    void CloseMagazine()
    {
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();
        weapon.OpenMagazine(false);
    }

    void PlayReloadSound()
    {
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();
        weapon.PlayReloadSound();
    }
}
