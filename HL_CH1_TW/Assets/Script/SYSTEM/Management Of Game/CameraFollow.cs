using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float lerpSpeed;

    private Vector3 offset;

    private Vector3 targetPos;

    private void Start()
    {
        findPlayerTarget(GameManager.Instance.CurrentPlayer.transform);
        offset = transform.position - target.position;
    }

    public void findPlayerTarget(Transform playerLocation)
    {
        target = playerLocation;
    }

    private void Update()
    {
        if (target == null) return;

        //targetPos = transform.position = target.position + offset;
        //transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
        transform.position = target.position + offset;
    }
}