using TMPro;
using UnityEngine;

public class DeckBuilderUI : MonoBehaviour
{
    public UIDeckBuilderID ID;

    [Header("Deck Slot")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private string titlePrefix = "Slot ";

    public bool showCursor = true;
    public CursorLockMode cursorLockMode = CursorLockMode.None;

    public int SelectedDeckSlot { get; private set; }

    private void OnEnable()
    {
        Cursor.visible = showCursor;
        Cursor.lockState = cursorLockMode;
    }

    public void SetDeckSlot(int slotNumber)
    {
        SelectedDeckSlot = Mathf.Clamp(slotNumber, 1, 5);

        if (titleText != null)
        {
            titleText.text = titlePrefix + SelectedDeckSlot;
        }
        else
        {
            Debug.LogWarning(
                "DeckBuilderUI needs the Text_Title object assigned to Title Text.",
                this
            );
        }
    }

    public void ClosePanel()
    {
        UIManager.Instance.Close(gameObject);
    }
}
