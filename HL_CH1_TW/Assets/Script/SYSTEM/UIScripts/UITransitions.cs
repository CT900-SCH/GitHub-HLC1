using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITransitions : MonoBehaviour
{
    public GameUIID NextUI;
    public float Delay = 3;

    bool _transitioning = false;
    void GoNextUI()
    {
        if (_transitioning)
        {
            return;
        }
        UIManager.Instance.OpenReplace(NextUI); // TODO replace this with OpenReplace
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(GoNextUI), Delay);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            GoNextUI();
        }
    }
}
