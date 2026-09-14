using UnityEngine;
using UnityEngine.UI;

public class UIPlaySfx : MonoBehaviour
{
    public Button targetButton;          // Drag your UI Button here
    public AudioSource audioSource;      // Drag your AudioSource here
    public AudioClip clickSound;         // Drag your confirm sound here

    void Start()
    {
        targetButton.onClick.AddListener(PlayClickSound);
    }

    void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip not assigned.");
        }
    }
}
