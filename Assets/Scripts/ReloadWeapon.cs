using UnityEngine;

public class ReloadWeapon : MonoBehaviour
{
    public Animator rigController;
    public WeaponAnimationEvents animationEvents;
    public ActiveWeapon activeWeapon;
    public Transform leftHand;
    public AmmoWidget ammoWidget;

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
        if (weapon)
        {
            if (Input.GetKeyDown(KeyCode.R) || weapon.ammoCount <= 0)
            {
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
            case "DetachMagzineFromWeapon":
                DetachMagzineFromWeapon();
                break;
            case "DropMagazine":
                DropMagazine();
                break;
            case "CreateNewMagazine":
                CreateNewMagazine();
                break;
            case "AttachMagazineToWeapon":
                AttachMagazineToWeapon();
                break;
        }
    }

    void DetachMagzineFromWeapon()
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

    void CreateNewMagazine()
    {
        magazineHand.SetActive(true);
    }

    void AttachMagazineToWeapon()
    {
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();
        weapon.magazine.SetActive(true);
        Destroy(magazineHand);
        rigController.ResetTrigger("Reload_Weapon");
        weapon.ammoCount = weapon.clipSize;
        ammoWidget.Refresh(weapon.ammoCount, weapon.clipSize);
    }
}
