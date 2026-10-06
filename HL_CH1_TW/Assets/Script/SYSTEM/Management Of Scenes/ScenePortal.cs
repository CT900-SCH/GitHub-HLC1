using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortal : MonoBehaviour
{
    [Header("Portal Destination")]
    [SerializeField] private string destinationSceneName;

    [Header("Settings")]
    [SerializeField] private bool requireInteraction;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    private bool playerInside;
    private bool isLoading;

    private void Update()
    {
        if (isLoading)
            return;

        if (requireInteraction && playerInside &&
            Input.GetKeyDown(interactionKey))
        {
            LoadDestinationScene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = true;

        if (!requireInteraction)
        {
            LoadDestinationScene();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;
    }

    private void LoadDestinationScene()
    {
        if (isLoading)
            return;

        if (string.IsNullOrWhiteSpace(destinationSceneName))
        {
            Debug.LogError(
                $"{gameObject.name} does not have a destination scene!"
            );

            return;
        }

        isLoading = true;

        Debug.Log(
            $"Entering portal. Loading {destinationSceneName}."
        );

        SceneManager.LoadScene(destinationSceneName);
    }
}