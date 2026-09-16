using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryTextController : MonoBehaviour
{
    [System.Serializable]
    public class StoryPage
    {
        [Header("Page Illustration")]
        public Sprite image;

        [Header("Page Writing")]
        [TextArea(4, 12)]
        public string text;
    }

    [Header("UI References")]
    [SerializeField] private Image storyImage;
    [SerializeField] private TMP_Text storyText;
    [SerializeField] private Button continueButton;

    [Header("Story Pages")]
    [SerializeField] private List<StoryPage> storyPages = new();

    [Header("Text Animation")]
    [SerializeField] private float charactersPerSecond = 30f;

    [Header("After Final Page")]
    [SerializeField] private bool loadNextScene;
    [SerializeField] private string nextSceneName;
    [SerializeField] private UnityEvent onStoryFinished;

    private int currentPage;
    private Coroutine typingCoroutine;
    private bool isTyping;
    private bool storyFinished;

    private void Start()
    {
        continueButton.onClick.AddListener(ContinueStory);

        if (storyImage != null)
        {
            storyImage.preserveAspect = true;
        }

        if (storyPages.Count > 0)
        {
            ShowPage(0);
        }
        else
        {
            storyText.text = "";

            if (storyImage != null)
            {
                storyImage.enabled = false;
            }
        }
    }

    private void OnDestroy()
    {
        if (continueButton != null)
        {
            continueButton.onClick.RemoveListener(ContinueStory);
        }
    }

    public void ContinueStory()
    {
        if (storyFinished)
            return;
        AudioManager.Instance.SFXSound(SoundID.PageTurn);

        // First click immediately completes the current writing.
        if (isTyping)
        {
            FinishTypingImmediately();
            return;
        }

        // The next click moves to the following story page.
        if (currentPage < storyPages.Count - 1)
        {
            currentPage++;
            ShowPage(currentPage);
            return;
        }

        FinishStory();
    }

    private void ShowPage(int pageIndex)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        StoryPage page = storyPages[pageIndex];

        // Change the illustration.
        if (storyImage != null)
        {
            storyImage.sprite = page.image;
            storyImage.enabled = page.image != null;
        }

        // Begin displaying the new writing.
        typingCoroutine = StartCoroutine(TypeText(page.text));
    }

    private IEnumerator TypeText(string newText)
    {
        isTyping = true;

        storyText.text = newText;
        storyText.maxVisibleCharacters = 0;
        storyText.ForceMeshUpdate();

        int totalCharacters = storyText.textInfo.characterCount;
        float delay = 1f / Mathf.Max(1f, charactersPerSecond);

        for (int visibleCharacters = 0;
             visibleCharacters <= totalCharacters;
             visibleCharacters++)
        {
            storyText.maxVisibleCharacters = visibleCharacters;
            yield return new WaitForSecondsRealtime(delay);
        }

        storyText.maxVisibleCharacters = int.MaxValue;
        isTyping = false;
        typingCoroutine = null;
    }

    private void FinishTypingImmediately()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        storyText.maxVisibleCharacters = int.MaxValue;
        isTyping = false;
    }

    private void FinishStory()
    {
        storyFinished = true;

        onStoryFinished?.Invoke();

        if (loadNextScene && !string.IsNullOrWhiteSpace(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}