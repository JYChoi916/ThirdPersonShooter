using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public Transform projectileSpawn;
    public GameObject projectile;

    public void Shot()
    {
        Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
    }
}
