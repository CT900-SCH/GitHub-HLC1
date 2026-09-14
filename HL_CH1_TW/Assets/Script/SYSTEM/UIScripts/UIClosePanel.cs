using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIClosePanel : MonoBehaviour
{
    public List<UIButtonSoundClosePanel> closeButtonSounds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (UIButtonSoundClosePanel pair in closeButtonSounds)
        {
            if (pair.button != null)
            {
                SoundID sfx = pair.sound;
                bool replace = pair.useCloseAll;

                pair.button.onClick.AddListener(() =>
                {
                    AudioManager.Instance.SFXSound(sfx);

                    if (replace)
                        UIManager.Instance.CloseAll();
                    else
                        UIManager.Instance.Close(this.gameObject);
                });
            }
        }
    }
}
