using UnityEditor.U2D.Animation;
using UnityEngine;

public class WeaponItem : MonoBehaviour
{
    [SerializeField] private WeaponController weaponController;   // Assign this in Inspector
    [SerializeField] private int weaponID;
    [SerializeField] private Weapon weaponToGive;


    private void Awake()
    {
        weaponToGive = WeaponLoader.Instance.myWeaponList.weapons[weaponID];
    }

    private void Update()
    {
        if (weaponController == null)
            weaponController = FindObjectOfType<Player>().GetComponentInChildren<WeaponController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (weaponController != null)
            {
                bool added = weaponController.AddWeapon(weaponToGive);
                if (added)
                {
                    Debug.Log($"Player picked up: {weaponToGive.weaponName}");
                    Destroy(gameObject); // Destroy the pickup
                }
                else
                {
                    Debug.Log("Player already has this weapon.");
                    Destroy(gameObject); // Destroy the pickup
                }
            }
        }
    }
}
