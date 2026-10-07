using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplayModeToggle : MonoBehaviour
{
    [Header("Inventory Modes")]

    [SerializeField]
    private GameObject listMode;

    [SerializeField]
    private GameObject iconMode;

    [Header("Mode Text")]

    [SerializeField]
    private TMP_Text modeText;

    [Header("Starting Mode")]

    [SerializeField]
    private bool startInIconMode = false;

    private Button toggleButton;
    private bool isIconMode;

    private void Awake()
    {
        toggleButton = GetComponent<Button>();

        if (toggleButton == null)
        {
            Debug.LogError(
                "InventoryDisplayModeToggle requires a Button component."
            );

            return;
        }

        toggleButton.onClick.AddListener(
            ToggleDisplayMode
        );

        SetDisplayMode(startInIconMode);
    }

    private void OnDestroy()
    {
        if (toggleButton != null)
        {
            toggleButton.onClick.RemoveListener(
                ToggleDisplayMode
            );
        }
    }

    public void ToggleDisplayMode()
    {
        SetDisplayMode(!isIconMode);
    }

    public void ShowListMode()
    {
        SetDisplayMode(false);
    }

    public void ShowIconMode()
    {
        SetDisplayMode(true);
    }

    private void SetDisplayMode(bool showIconMode)
    {
        isIconMode = showIconMode;

        if (listMode != null)
        {
            listMode.SetActive(!isIconMode);
        }

        if (iconMode != null)
        {
            iconMode.SetActive(isIconMode);
        }

        UpdateModeText();
    }

    private void UpdateModeText()
    {
        if (modeText == null)
        {
            return;
        }

        modeText.text = isIconMode
            ? "ICON"
            : "LIST";
    }
}