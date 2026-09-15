using UnityEngine;

public class MainUI : MonoBehaviour
{
    public UIMainID ID;

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