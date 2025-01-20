using UnityEngine;

public class ReloadWeapon : MonoBehaviour
{
    public Animator rigController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            rigController.SetTrigger("Reload_Weapon");
        }
    }
}
