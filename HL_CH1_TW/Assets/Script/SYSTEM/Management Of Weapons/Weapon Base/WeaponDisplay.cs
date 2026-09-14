using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDisplay : MonoBehaviour
{
    public WeaponController ws;
    public int listData = 0;

    public Image im;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ws = FindObjectOfType<WeaponController>();
        im = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ws.weaponDataList.Count <= listData) return;

        if (ws.weaponDataList[listData] != null)
        {
            im.color = Color.white;
            Color newColor = im.color;         // Get the current color
            newColor.a = 1f;                   // Set alpha to 1 (Fully Non-transparent)
            im.color = newColor;               // Apply the updated color
            im.sprite = ws.weaponDataList[listData].loadedSprite;
        }
        else
        {
            im.color = Color.black;
            Color newColor = im.color;         // Get the current color
            newColor.a = 0f;                   // Set alpha to 0 (Fully transparent)
            im.color = newColor;               // Apply the updated color
            im.sprite = null;
        }
    }
}
