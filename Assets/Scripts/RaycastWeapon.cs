using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    class Bullet
    {
        public float time;
        public Vector3 initPosition;
        public Vector3 initVelocity;
        public TrailRenderer tracer;
        public int bounce;
        public float impulse;
    }

    public ActiveWeapon.WeaponSlot weaponSlot;
    public bool isFiring = false;
    public int fireRate = 25;
    public float bulletSpeed = 1000.0f;
    public float bulletDrop = 0.0f;
    public float maxBulletLifeTime = 3.0f;
    public int maxBounce = 2;
    public float bulletImpulse = 20f;

    public ParticleSystem[] muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer tracerEffect;
    public string weaponName;

    public Transform raycastOrigin;
    public Transform raycastDestination;
    public WeaponRecoil recoil;
    public GameObject magazine;

    public AudioClip fireClip;
    public AudioClip reloadClip;

    public int ammoCount;
    public int clipSize;

    Ray ray;
    RaycastHit hitInfo;
    float accumulatedTime;
    List<Bullet> bullets = new List<Bullet>();

    void Awake()
    {
        recoil = GetComponent<WeaponRecoil>();
    }

    Vector3 GetPosition(Bullet bullet)
    {
        // p + v * t + 0.5 * g * t * t
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initPosition) + (bullet.initVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initPosition = position;
        bullet.initVelocity = velocity;
        bullet.time = 0f;
        bullet.bounce = maxBounce;
        bullet.impulse = bulletImpulse;
        bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        return bullet;
    }

    public void StartFiring()
    {
        isFiring = true;
        FireBullet();
        accumulatedTime = -(1.0f / fireRate);
    }

    public void UpdateFiring(float deltaTime)
    {
        accumulatedTime += deltaTime;
        float fireInterval = 1.0f / fireRate;
        while(accumulatedTime >= 0.0f)
        {
            FireBullet();
            accumulatedTime -= fireInterval;
        }
    }
    
    public void UpdateBullets(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestroyBullets();
    }

    void SimulateBullets(float deltaTime)
    {
        for(int i = 0; i< bullets.Count; ++i)
        {
            Bullet bullet = bullets[i];
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        }
    }

    void DestroyBullets()
    {
        bullets.RemoveAll(bullet => bullet.time > maxBulletLifeTime);
    }

    void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        ray.origin = start;
        ray.direction = direction;

        if (Physics.Raycast(ray, out hitInfo, distance))
        {
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);

            bullet.time = maxBulletLifeTime;
            end = hitInfo.point;

            // µµ≈∫
            if (bullet.bounce > 0)
            {
                bullet.time = 0;
                bullet.initPosition = hitInfo.point;
                bullet.initVelocity = Vector3.Reflect(bullet.initVelocity, hitInfo.normal);
                bullet.bounce--;
            }

            // √Êµπ ¿”∆ﬁΩ∫
            var rb2d = hitInfo.collider.GetComponent<Rigidbody>();
            if (rb2d != null)
            {
                rb2d.AddForceAtPosition(ray.direction * bullet.impulse, hitInfo.point, ForceMode.Impulse);
                bullet.bounce = 0;
            }
        }

        if (bullet.tracer)
        {
            bullet.tracer.transform.position = end;
        }
    }

    private void FireBullet()
    {
        if (ammoCount <= 0)
        {
            return;
        }
        ammoCount--;

        AudioSource source = GetComponent<AudioSource>();
        source.PlayOneShot(fireClip);
        foreach (var particle in muzzleFlash)
        {
            particle.Emit(1);
        }

        Vector3 velocity = (raycastDestination.position - raycastOrigin.position).normalized * bulletSpeed;
        Bullet bullet = CreateBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);

        recoil.GenerateRecoil(weaponName);
    }

    public void StopFiring()
    {
        isFiring = false;
    }

    public void OpenMagazine(bool open)
    {
        Animation animation = GetComponent<Animation>();
        if (animation)
        {
            animation["OpenMagazine"].speed = open ? 1 : -1;
            animation.Play("OpenMagazine");
        }
    }

    public void PlayReloadSound()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.PlayOneShot(reloadClip);
    }
}
