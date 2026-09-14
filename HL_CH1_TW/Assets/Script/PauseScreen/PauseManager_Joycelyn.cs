using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager_Joycelyn : MonoBehaviour
{
    public Image blackOverlay;

    public GameObject pauseScreen;
    public Animator anim;
    bool isPaused = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //(prob dun need when merge with the gamemanager script)
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        Pause();
    }

    public void Pause()
    {
        // Block pause input if PlayerSelection is open
        // Don't pause while this UI is active
        // if (UIManager.Instance.IsUIOpen(GameUIID.PlayerSelection)) return;

        // Allow pause if Escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = true;
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void Resume()
    {
        anim.Play("PausePanel_Outro");
    }

    public void ResumeAnimationEnd()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
    }


    public void RestartAnimation()
    {
        anim.Play("PausePanel_Transition");
    }
    public void MenuAimation()
    {
        anim.Play("PausePanel_Transition2");
    }

    public void RestartEnd()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ResumeAnimationEnd();
    }

    public void MenuEnd()
    {
        SceneManager.LoadScene(0); //(this loads the 0 buildindex from the build profiles)
        ResumeAnimationEnd();
    }
}
