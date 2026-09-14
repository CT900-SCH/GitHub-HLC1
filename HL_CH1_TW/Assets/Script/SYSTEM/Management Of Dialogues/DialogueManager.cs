using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [Header("Character Portrait Position")]
    public Image leftImage;
    public Image rightImage;
    public Image centerImage;

    [Range(0f, 1f)]
    public float inactiveOpacity = 0.3f;

    [Header("UI References")]
    public TMP_Text speakerText;
    public TMP_Text dialogueText;
    public GameObject dialoguePanel;

    public AudioSource audioSource;

    public DialogueSet dialogueSet;

    private int currentLineIndex = 0;
    private bool isDialoguePlaying = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialoguePanel.SetActive(false);

        // Start the Dialogue Automatically.
        StartDialogue();
    }
    public void StartDialogue()
    {
        if (dialogueSet == null || dialogueSet.lines.Length == 0)
        {
            Debug.LogWarning("No dialogue set assigned.");
            
            return;
        }

        currentLineIndex = 0;
        dialoguePanel.SetActive(true);
        isDialoguePlaying = true;


        ShowNextLine();
    }

    public void ShowNextLine()
    {
        void SetCharacterPortraits(DialogueLine line)
        {
            // Handle LEFT Slot
            if(line.leftSprite != null)
            {
                leftImage.sprite = line.leftSprite;
                leftImage.gameObject.SetActive(true);
                SetImageAlpha(leftImage, line.leftImageOpacity);
            }
            else
                leftImage.gameObject.SetActive(false);

            // Handle RIGHT Slot
            if (line.leftSprite != null)
            {
                rightImage.sprite = line.rightSprite;
                rightImage.gameObject.SetActive(true);
                SetImageAlpha(rightImage, line.rightImageOpacity);
            }
            else
                rightImage.gameObject.SetActive(false);

            // Handle Center Slot
            if (line.leftSprite != null)
            {
                centerImage.sprite = line.centerSprite;
                centerImage.gameObject.SetActive(true);
                SetImageAlpha(centerImage, line.centerImageOpacity);
            }
            else
                centerImage.gameObject.SetActive(false);
        }

        void SetImageAlpha(Image image, float alpha)
        {
            var color = image.color;
            color.a = alpha;
            image.color = color;
        }

        if (!isDialoguePlaying) return;

        if(currentLineIndex >= dialogueSet.lines.Length)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = dialogueSet.lines[currentLineIndex];

        speakerText.text = line.speakerName;
        dialogueText.text = line.line;

        SetCharacterPortraits(line);

        if (line.voiceClip != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(line.voiceClip);
        }
        currentLineIndex++;
    }

    public void ShowPrevious()
    {

    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isDialoguePlaying = false;

        // If a scene name is specified, load it
        if (!string.IsNullOrEmpty(dialogueSet.sceneName))
            SceneManager.LoadScene(dialogueSet.sceneName);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDialoguePlaying && Input.GetKeyDown(KeyCode.Space))
        {
            ShowNextLine();
        }
    }
}
