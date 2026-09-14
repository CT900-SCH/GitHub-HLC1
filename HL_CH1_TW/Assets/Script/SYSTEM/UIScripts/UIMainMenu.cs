using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    public Button btnStart;
    public Button btnSettings;
    public Button btnQuit;

    // Start is called before the first frame update
    void Start()
    {
        btnStart.onClick.AddListener(OnPlayClick);
        btnSettings.onClick.AddListener(OnSettingsClick);
        btnQuit.onClick.AddListener(OnQuitClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnSettingsClick()
    {
        AudioManager.Instance.SFXSound(SoundID.ButtonClick);
        UIManager.Instance.Open(GameUIID.Settings);
    }

    private void OnPlayClick()
    {
        AudioManager.Instance.SFXSound(SoundID.Confirm);
        UIManager.Instance.OpenReplace(GameUIID.Title);
    }

    private void OnQuitClick()
    {
        AudioManager.Instance.SFXSound(SoundID.Cancel);
        UIManager.Instance.OpenReplace(GameUIID.Title);
    }
}
