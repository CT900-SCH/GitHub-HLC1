using UnityEngine;

public class ResourceItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerLevelUpStats.Instance.Gold += 1;
            Destroy(gameObject); // Destroy the pickup
        }
    }
}
