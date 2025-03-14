using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public int hp;

    bool isDead;
    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }

    public void Damage(int damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            isDead = true;
        }
    }
}
