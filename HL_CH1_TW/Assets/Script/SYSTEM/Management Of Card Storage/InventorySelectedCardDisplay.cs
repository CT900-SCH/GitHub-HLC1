using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySelectedCardDisplay : MonoBehaviour
{
    [Header("Selected Card Artwork")]

    [SerializeField]
    private Image artworkImage;

    [Header("Selected Card Statistics")]

    [SerializeField]
    private TMP_Text sizeValueText;

    [SerializeField]
    private TMP_Text moveValueText;

    [SerializeField]
    private TMP_Text attackValueText;

    [SerializeField]
    private TMP_Text defenseValueText;

    public void DisplayUnitCard(UnitCardData cardData)
    {
        if (cardData == null)
        {
            ClearDisplay();
            return;
        }

        if (artworkImage != null)
        {
            artworkImage.sprite =
                cardData.illustrationArtwork;

            artworkImage.enabled =
                cardData.illustrationArtwork != null;
        }

        SetText(
            sizeValueText,
            cardData.size
        );

        SetText(
            moveValueText,
            cardData.moveSpeed
        );

        SetText(
            attackValueText,
            cardData.attack
        );

        SetText(
            defenseValueText,
            cardData.defense
        );
    }

    public void ClearDisplay()
    {
        if (artworkImage != null)
        {
            artworkImage.sprite = null;
            artworkImage.enabled = false;
        }

        SetText(sizeValueText, 0);
        SetText(moveValueText, 0);
        SetText(attackValueText, 0);
        SetText(defenseValueText, 0);
    }

    private void SetText(
        TMP_Text targetText,
        int value
    )
    {
        if (targetText != null)
        {
            targetText.text =
                value.ToString();
        }
    }
}