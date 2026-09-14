using UnityEngine;

public class KILLSEarned : MonoBehaviour
{
    public static KILLSEarned instance;

    [Header("KILL Points Earned: ")]
    public int KILLPoints;

    private void Awake()
    {
        instance = this; // Inserting this into the Static Pigeon hole.
    }

    private void OnDestroy()
    {
        instance = null;
    }

    private void Start()
    {
        KILLPoints = 0;
    }
}
