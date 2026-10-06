using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIButtonOpenPauseUI
{
    public Button button;
    public SoundID sound;
    public UIPauseID uiToOpen;
    public bool useReplace;
}

public class UIPauseMenu : MonoBehaviour
{
    public List<UIButtonOpenPauseUI> pauseUIButtons;

    private void Start()
    {
        foreach (UIButtonOpenPauseUI pair in pauseUIButtons)
        {
            if (pair.button == null)
                continue;

            SoundID sfx = pair.sound;
            UIPauseID uiID = pair.uiToOpen;
            bool replace = pair.useReplace;

            pair.button.onClick.AddListener(() =>
            {
                AudioManager.Instance.SFXSound(sfx);

                if (replace)
                    UIManager.Instance.OpenReplace(uiID);
                else
                    UIManager.Instance.Open(uiID);
            });
        }
    }
}
