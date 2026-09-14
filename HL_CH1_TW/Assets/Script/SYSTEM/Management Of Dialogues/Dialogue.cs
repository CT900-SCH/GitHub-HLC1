using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using UnityEngine.Playables;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textcomponent;
    [TextArea(2, 5)] public string[] lines;
    public SoundType soundtype;
    public float textspeed;
    public bool dialoguefinished = false;
    public GameObject healthbar;
    public Animator animator;
    public float disableDelay = 0.2f; // Public timer to control the delay before disabling movement and animator
    private float disableTimer = 0.2f; // Internal timer to track the delay before disabling movement
    private int index;
    public PlayableDirector Timeline;

    void Start()
    {
        
        Timeline.playableGraph.GetRootPlayable(0).SetSpeed(0); // Pause the Timeline
        textcomponent.text = string.Empty;
        
        StartCoroutine(StartDialogue()); // Use a coroutine to introduce an initial delay
    }

    void Update()
    {
        if (disableTimer > 0f)
        {
            disableTimer -= Time.deltaTime; // Reduce the timer by the elapsed time
        }

        animator.Play("Idle", 0, 0.5f);
        // Check for left-click input to advance the dialogue
        if (Input.GetMouseButtonDown(0))
        {
            if (textcomponent.text == lines[index]) // If the current line is fully typed
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textcomponent.text = lines[index]; // Instantly display the full text
            }
        }
    }

    IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(0.5f); // Delay before first dialogue appears
        index = 0;
        SoundManager.PlaySound(soundtype); // Start pratting.....
        StartCoroutine(TypeLine());
    }

    void DisplayNextLine()
    {
        StopAllCoroutines(); // Ensure coroutine restarts properly
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        textcomponent.text = ""; // Ensure text is empty before typing
        yield return new WaitForSeconds(textspeed); // Ensure consistent delay before first letter

        foreach (char c in lines[index])
        {
            textcomponent.text += c;
            yield return new WaitForSeconds(textspeed); // Type each character with delay
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            SoundManager.PlaySound(soundtype); // Next pratting.....
            DisplayNextLine();
        }
        else
        {
            // Dialogue finished
            dialoguefinished = true;
            Destroy(gameObject);
            // Hide dialogue box

            // Check if the Timeline exists before resuming
            if (Timeline != null)
            {
                Timeline.playableGraph.GetRootPlayable(0).SetSpeed(1); // Resume the Timeline
            }

            // Re-enable player animator
            animator.enabled = true;

            // Show health bar
            healthbar.SetActive(true);
        }
    }
}
