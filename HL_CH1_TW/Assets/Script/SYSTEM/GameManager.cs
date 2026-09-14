using UnityEngine;

public class GameManager : SimpleSingleton<GameManager>
{
    [Header("Player Data: ")]
    [SerializeField]
    private Player player01;
    public Player CurrentPlayer { private set; get; }

    [Header("Player's Current Stats: ")]
    public float currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIManager.Instance.Open(GameUIID.Logo);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spawnPlayerOnce(Vector3 PlayersNewSpawnPosition)
    {
        // Spawn the player once when entering a Level.

        Vector3 playerPos = PlayersNewSpawnPosition;

        var obj = Instantiate(player01, playerPos, Quaternion.identity);
        if (obj.TryGetComponent(out Player player))
        {
            CurrentPlayer = player;
        }
    }
}
