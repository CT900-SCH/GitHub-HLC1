using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIOpenScene : MonoBehaviour
{
    public Button btnPlay;
    public string putSceneName;

    // Start is called before the first frame update
    void Start()
    {
        btnPlay.onClick.AddListener(OnPlayClick);
    }

    private void OnPlayClick()
    {
        UIManager.Instance.CloseAll();
        AudioManager.Instance.SFXSound(SoundID.Confirm);
        
        if(putSceneName != null)
        {
            SceneManager.LoadScene(putSceneName);
        }
    }
}
