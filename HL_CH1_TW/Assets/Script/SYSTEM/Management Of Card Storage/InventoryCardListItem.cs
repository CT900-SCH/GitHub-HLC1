using TMPro;
using UnityEngine;

public class InventoryCardListItem : MonoBehaviour
{
    [Header("Card Information")]

    [SerializeField]
    private TMP_Text cardNameText;

    [SerializeField]
    private TMP_Text inventoryAmountText;

    [SerializeField]
    private TMP_Text deckAmountText;

    public void SetContent(
        string cardName,
        int inventoryAmount,
        int deckAmount
    )
    {
        if (cardNameText != null)
        {
            cardNameText.text =
                string.IsNullOrWhiteSpace(cardName)
                    ? "Unnamed Card"
                    : cardName;
        }

        if (inventoryAmountText != null)
        {
            inventoryAmountText.text =
                inventoryAmount.ToString();
        }

        if (deckAmountText != null)
        {
            deckAmountText.text =
                deckAmount.ToString();
        }
    }

    private void Reset()
    {
        cardNameText =
            GetComponent<TMP_Text>();
    }
}