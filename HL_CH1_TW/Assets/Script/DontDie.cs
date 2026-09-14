using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDie : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Debug.Log(this.name + " is now immortal.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
