using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIButtonOpenDeckBuilderUI
{
    public Button button;
    public SoundID sound;
    public UIDeckBuilderID uiToOpen;
    public bool useReplace;

    [Range(1, 10)]
    public int deckSlotNumber = 1;
}

public class UIDeckBuilder : MonoBehaviour
{
    [SerializeField]
    private List<UIButtonOpenDeckBuilderUI> deckBuilderUIButtons = new();

    private void Start()
    {
        foreach (UIButtonOpenDeckBuilderUI pair in deckBuilderUIButtons)
        {
            if (pair.button == null)
                continue;

            SoundID sfx = pair.sound;
            UIDeckBuilderID uiID = pair.uiToOpen;
            bool replace = pair.useReplace;
            int slotNumber = pair.deckSlotNumber;

            pair.button.onClick.AddListener(() =>
            {
                AudioManager.Instance.SFXSound(sfx);

                GameObject openedUI;

                if (replace)
                    openedUI = UIManager.Instance.OpenReplace(uiID);
                else
                    openedUI = UIManager.Instance.Open(uiID);

                if (openedUI != null &&
                    openedUI.TryGetComponent(out DeckBuilderUI deckBuilderUI))
                {
                    deckBuilderUI.SetDeckSlot(slotNumber);
                }
            });
        }
    }
}
