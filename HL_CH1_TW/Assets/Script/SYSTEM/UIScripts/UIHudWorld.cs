using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIButtonSoundOpenPanel
{
    public Button button;
    public SoundID sound;
    public GameUIID uiToOpen;
    public bool useReplace; // true = OpenReplace, false = Open
}

[System.Serializable]
public class UIButtonSoundClosePanel
{
    public Button button;
    public SoundID sound;
    public bool useCloseAll; // true = CloseAll, false = Close
}

public class UIHudWorld : MonoBehaviour
{
    public List<UIButtonSoundOpenPanel> customButtonSounds;

    // Start is called before the first frame update
    void Start()
    {
        foreach (UIButtonSoundOpenPanel pair in customButtonSounds)
        {
            if (pair.button != null)
            {
                SoundID sfx = pair.sound;
                GameUIID uiID = pair.uiToOpen;
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
}
