using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITutorialScreen : MonoBehaviour
{
    public GameObject[] tutorialPages; // Drag your tutorial page panels here in Inspector
    public Button nextButton;
    public Button prevButton;
    public Button resumeButton;

    private int currentPage = 0;

    void Start()
    {
        PauseGame();
        ShowPage(currentPage);

        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PreviousPage);
        resumeButton.onClick.AddListener(ResumeGame);
    }

    void ShowPage(int index)
    {
        // Hide all pages
        for (int i = 0; i < tutorialPages.Length; i++)
        {
            tutorialPages[i].SetActive(i == index);
        }

        // Show or hide buttons
        prevButton.gameObject.SetActive(index > 0);
        nextButton.gameObject.SetActive(index < tutorialPages.Length - 1);
        resumeButton.gameObject.SetActive(index == tutorialPages.Length - 1);
    }

    public void NextPage()
    {
        if (currentPage < tutorialPages.Length - 1)
        {
            currentPage++;
            ShowPage(currentPage);
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage(currentPage);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        UIManager.Instance.CloseAll();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
}
