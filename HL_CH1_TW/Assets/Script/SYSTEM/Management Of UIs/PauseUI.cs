using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public UIPauseID ID;

    public bool showCursor = true;
    public CursorLockMode cursorLockMode = CursorLockMode.None;

    private void OnEnable()
    {
        Cursor.visible = showCursor;
        Cursor.lockState = cursorLockMode;
    }

    public void ClosePanel()
    {
        UIManager.Instance.Close(gameObject);
    }
}