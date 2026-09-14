using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStub : MonoBehaviour
{
    public GameObject GameManagerPrefab;   
    
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance == null)
        {
            Instantiate(GameManagerPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
