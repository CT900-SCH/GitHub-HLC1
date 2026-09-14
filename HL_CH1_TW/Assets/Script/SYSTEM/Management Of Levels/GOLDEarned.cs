using UnityEngine;

public class GOLDEarned : MonoBehaviour
{
    public static GOLDEarned instance;

    [Header("GOLD Points Earned: ")]
    public int GOLDPoints;

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
        GOLDPoints = 0;
    }
}
