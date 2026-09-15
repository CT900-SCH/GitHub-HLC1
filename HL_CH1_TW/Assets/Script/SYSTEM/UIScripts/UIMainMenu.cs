using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIButtonOpenMainUI
{
    public Button button;
    public SoundID sound;
    public UIMainID uiToOpen;
    public bool useReplace;
}

public class UIMainMenu : MonoBehaviour
{
    public List<UIButtonOpenMainUI> mainUIButtons;

    private void Start()
    {
        foreach (UIButtonOpenMainUI pair in mainUIButtons)
        {
            if (pair.button == null)
                continue;

            SoundID sfx = pair.sound;
            UIMainID uiID = pair.uiToOpen;
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
