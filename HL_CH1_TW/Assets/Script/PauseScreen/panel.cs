using UnityEngine;
using UnityEngine.UI;

public class panel : MonoBehaviour
{
    public PauseManager_Joycelyn pauseScreen;

    public void HideScreen()
    {
        pauseScreen.ResumeAnimationEnd();
    }
    public void RestartTransit()
    {
        pauseScreen.RestartEnd();
    }
    public void MenuTransit()
    {
        pauseScreen.MenuEnd();
    }
}
