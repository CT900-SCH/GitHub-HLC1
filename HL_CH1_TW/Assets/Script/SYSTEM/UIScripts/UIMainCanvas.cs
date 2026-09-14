using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainCanvas : MonoBehaviour
{
    public static UIMainCanvas instance;

    private void Awake()
    {
        //instance = this; // Inserting this into the Static Pigeon hole.


        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void OnDestroy()
    {
        instance = null;
    }
}
