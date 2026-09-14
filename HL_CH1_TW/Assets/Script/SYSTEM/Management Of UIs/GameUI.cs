using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public GameUIID ID;
    public bool ShowCursor;
    public CursorLockMode CursorLockMode = CursorLockMode.None;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = ShowCursor;
        Cursor.lockState = CursorLockMode;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
