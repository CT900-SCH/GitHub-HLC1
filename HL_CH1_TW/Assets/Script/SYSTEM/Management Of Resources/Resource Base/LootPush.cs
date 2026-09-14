using UnityEngine;
using System.Collections;

public class LootPush : MonoBehaviour
{
    [Header("Random Force Settings")]
    public float minForce = 3f;
    public float maxForce = 7f;

    [Header("Auto Stop Settings")]
    public float stopDelay = 0.1f; // time in seconds before stopping
    public float dampTime = 0.3f;  // optional: ease into stop

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (rb != null)
        {
            // Random direction
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            // Random force within range
            float randomForce = Random.Range(minForce, maxForce);
            // Apply impulse
            rb.AddForce(randomDir * randomForce, ForceMode2D.Impulse);

            StartCoroutine(StopLootAfterDelay());
        }
    }

    private IEnumerator StopLootAfterDelay()
    {
        yield return new WaitForSeconds(stopDelay);

        if (rb != null)
        {
            Vector2 initialVelocity = rb.linearVelocity;

            float elapsed = 0f;
            while (elapsed < dampTime)
            {
                rb.linearVelocity = Vector2.Lerp(initialVelocity, Vector2.zero, elapsed / dampTime);
                elapsed += Time.deltaTime;
                yield return null;
            }

            rb.linearVelocity = Vector2.zero;
        }
    }
}