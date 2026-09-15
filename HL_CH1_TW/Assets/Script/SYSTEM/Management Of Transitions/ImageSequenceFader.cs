using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ImageSequenceFader : MonoBehaviour
{
    [Header("Images (play from top to bottom)")]
    [SerializeField] private List<Image> images = new List<Image>();

    [Header("Timing")]
    [Min(0f)][SerializeField] private float fadeInDuration = 1f;
    [Min(0f)][SerializeField] private float displayDuration = 2f;
    [Min(0f)][SerializeField] private float fadeOutDuration = 1f;

    [Header("Next Scene")]
    [SerializeField] private string nextSceneName;

    [Header("Options")]
    [Tooltip("True: all images fade together. False: images fade one by one.")]
    [SerializeField] private bool simultaneous = true;
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private bool useUnscaledTime = true;

    private Coroutine sequenceCoroutine;

    private void Awake()
    {
        foreach (Image image in images)
        {
            if (image == null)
                continue;

            SetAlpha(image, 0f);
            image.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        if (playOnStart)
            PlaySequence();
    }

    public void PlaySequence()
    {
        if (sequenceCoroutine != null)
            StopCoroutine(sequenceCoroutine);

        HideAllImages();
        sequenceCoroutine = StartCoroutine(PlaySequenceRoutine());
    }

    private IEnumerator PlaySequenceRoutine()
    {
        if (simultaneous)
            yield return PlayAllImagesSimultaneously();
        else
            yield return PlayImagesOneByOne();

        sequenceCoroutine = null;
        LoadNextScene();
    }

    private IEnumerator PlayAllImagesSimultaneously()
    {
        foreach (Image image in images)
        {
            if (image == null)
                continue;

            image.gameObject.SetActive(true);
            SetAlpha(image, 0f);
        }

        yield return FadeAllImages(0f, 1f, fadeInDuration);
        yield return WaitForDuration(displayDuration);
        yield return FadeAllImages(1f, 0f, fadeOutDuration);

        HideAllImages();
    }

    private IEnumerator PlayImagesOneByOne()
    {
        foreach (Image image in images)
        {
            if (image == null)
                continue;

            image.gameObject.SetActive(true);
            SetAlpha(image, 0f);

            yield return FadeImage(image, 0f, 1f, fadeInDuration);
            yield return WaitForDuration(displayDuration);
            yield return FadeImage(image, 1f, 0f, fadeOutDuration);

            image.gameObject.SetActive(false);
        }
    }

    private IEnumerator FadeAllImages(float startAlpha, float endAlpha, float duration)
    {
        if (duration <= 0f)
        {
            SetAllImagesAlpha(endAlpha);
            yield break;
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += GetDeltaTime();
            float progress = Mathf.Clamp01(elapsed / duration);
            SetAllImagesAlpha(Mathf.Lerp(startAlpha, endAlpha, progress));
            yield return null;
        }

        SetAllImagesAlpha(endAlpha);
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrWhiteSpace(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("ImageSequenceFader: Next Scene Name is empty.", this);
        }
    }

    private void HideAllImages()
    {
        foreach (Image image in images)
        {
            if (image == null)
                continue;

            SetAlpha(image, 0f);
            image.gameObject.SetActive(false);
        }
    }

    private void SetAllImagesAlpha(float alpha)
    {
        foreach (Image image in images)
        {
            if (image != null)
                SetAlpha(image, alpha);
        }
    }

    private IEnumerator FadeImage(Image image, float startAlpha, float endAlpha, float duration)
    {
        if (duration <= 0f)
        {
            SetAlpha(image, endAlpha);
            yield break;
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += GetDeltaTime();
            float progress = Mathf.Clamp01(elapsed / duration);
            SetAlpha(image, Mathf.Lerp(startAlpha, endAlpha, progress));
            yield return null;
        }

        SetAlpha(image, endAlpha);
    }

    private IEnumerator WaitForDuration(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += GetDeltaTime();
            yield return null;
        }
    }

    private float GetDeltaTime()
    {
        return useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
    }

    private static void SetAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}
