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

    RaycastWeapon weapon;
    Animator anim;
    AnimatorOverrideController overrides;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        overrides = anim.runtimeAnimatorController as AnimatorOverrideController;

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
            handIK.weight = 1f;

            if (Input.GetButtonDown("Fire1"))
            {
                weapon.StartFiring();
            }

            if (weapon.isFiring)
            {
                weapon.UpdateFiring(Time.deltaTime);
            }

            weapon.UpdateBullets(Time.deltaTime);

            if (Input.GetButtonUp("Fire1"))
            {
                weapon.StopFiring();
            }
        }
        else
        {
            handIK.weight = 0f;
            anim.SetLayerWeight(1, 0.0f);
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

        anim.SetLayerWeight(1, 1.0f);
        overrides["Weapon_Anim_Empty"] = weapon.weaponAnimation;
    }

    [ContextMenu("Save Weapon Pose")]
    void SaveWeaponPose()
    {
        GameObjectRecorder recoder = new GameObjectRecorder(gameObject);
        recoder.BindComponentsOfType<Transform>(weaponParent.gameObject, false);
        recoder.BindComponentsOfType<Transform>(weaponLeftGrip.gameObject, false);
        recoder.BindComponentsOfType<Transform>(weaponRightGrip.gameObject, false);
        recoder.TakeSnapshot(0.0f);
        recoder.SaveToClip(weapon.weaponAnimation);
    }
}
