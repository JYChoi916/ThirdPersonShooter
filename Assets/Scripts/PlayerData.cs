using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int maxHP;

    bool isDead;
    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }

    public int currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    public void Damage(int damage)
    {
        currentHP -= damage;
        if (currentHP < 0)
        {
            isDead = true;
        }
    }

    public void Heal(int healValue)
    {
        currentHP += healValue;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }
}
