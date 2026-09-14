using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHPDisplay : MonoBehaviour
{
    // Image For HealthBar.
    [SerializeField] private Image hpImage;
    [SerializeField] private Image hpEffectImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CurrentPlayer != null)
        {
            hpImage.fillAmount = Player.Instance.HP / Player.Instance.MaxHP;

            if (hpEffectImage.fillAmount > hpImage.fillAmount)
            {
                hpEffectImage.fillAmount -= 0.005f; // Delay Effect
            }
            else
            {
                hpEffectImage.fillAmount = hpImage.fillAmount; // STOP Continue Decreasing
            }
        }
    }
}
