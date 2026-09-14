using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerEXPDisplay : MonoBehaviour
{
    // Image For EXPBar
    [SerializeField] private Image expImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Fill Exp Bar Image with Exp
        //Reset the FillBar
        expImage.fillAmount = (PlayerLevelUpStats.Instance.experience - PlayerLevelUpStats.previousExperience) / (PlayerLevelUpStats.expNeeded - PlayerLevelUpStats.previousExperience);

        //Reset the FillBarc
        if (expImage.fillAmount == 1)
        {
            expImage.fillAmount = 0;
        }
    }
}
