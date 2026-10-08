using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class DeckSlotCardEntryUI : MonoBehaviour
{
    [Header("Row UI")]

    [Tooltip("Drag the root Text_CardContent_Equip text here.")]
    [SerializeField]
    private TMP_Text cardNameText;

    [Tooltip("Drag Text_DeckAmount here (left number).")]
    [SerializeField]
    private TMP_Text deckAmountText;

    [Tooltip("Drag Text_DeckStorage here (right number).")]
    [SerializeField]
    private TMP_Text deckStorageText;

    [SerializeField]
    private Button buttonLeft;

    [SerializeField]
    private Button buttonRight;

    private CardStorage cardStorage;
    private int deckSlotNumber;

    private PlayerCardData playerCard;
    private HeroCardData heroCard;
    private UnitCardData unitCard;

    private Action refreshAllEntries;

    public void SetUp(
        CardStorage storage,
        int slotNumber,
        PlayerCardData card,
        Action refreshCallback)
    {
        Prepare(storage, slotNumber, refreshCallback);
        playerCard = card;
        RefreshDisplay();
    }

    public void SetUp(
        CardStorage storage,
        int slotNumber,
        HeroCardData card,
        Action refreshCallback)
    {
        Prepare(storage, slotNumber, refreshCallback);
        heroCard = card;
        RefreshDisplay();
    }

    public void SetUp(
        CardStorage storage,
        int slotNumber,
        UnitCardData card,
        Action refreshCallback)
    {
        Prepare(storage, slotNumber, refreshCallback);
        unitCard = card;
        RefreshDisplay();
    }

    public void SetDeckSlot(int slotNumber)
    {
        deckSlotNumber = Mathf.Clamp(
            slotNumber,
            1,
            CardStorage.DeckSlotAmount
        );
    }

    public void RefreshDisplay()
    {
        if (cardStorage == null)
            return;

        int ownedAmount = GetOwnedAmount();
        int deckAmount = GetDeckAmount();

        if (cardNameText != null)
            cardNameText.text = GetCardDisplayName();

        // Left: copies in the selected deck.
        if (deckAmountText != null)
            deckAmountText.text = deckAmount.ToString();

        // Right: total owned copies.
        if (deckStorageText != null)
            deckStorageText.text = ownedAmount.ToString();

        if (buttonLeft != null)
            buttonLeft.interactable = deckAmount > 0;

        if (buttonRight != null)
            buttonRight.interactable = CanAddCardToDeck();
    }

    private void Prepare(
        CardStorage storage,
        int slotNumber,
        Action refreshCallback)
    {
        cardStorage = storage;
        SetDeckSlot(slotNumber);
        refreshAllEntries = refreshCallback;

        playerCard = null;
        heroCard = null;
        unitCard = null;

        if (buttonLeft != null)
        {
            buttonLeft.onClick.RemoveListener(RemoveOneCard);
            buttonLeft.onClick.AddListener(RemoveOneCard);
        }

        if (buttonRight != null)
        {
            buttonRight.onClick.RemoveListener(AddOneCard);
            buttonRight.onClick.AddListener(AddOneCard);
        }
    }

    private void OnDestroy()
    {
        if (buttonLeft != null)
            buttonLeft.onClick.RemoveListener(RemoveOneCard);

        if (buttonRight != null)
            buttonRight.onClick.RemoveListener(AddOneCard);
    }

    private void AddOneCard()
    {
        if (cardStorage == null)
            return;

        bool added = false;
        string reason = string.Empty;

        if (playerCard != null)
        {
            added = cardStorage.TryAddToDeck(
                deckSlotNumber, playerCard, out reason
            );
        }
        else if (heroCard != null)
        {
            added = cardStorage.TryAddToDeck(
                deckSlotNumber, heroCard, out reason
            );
        }
        else if (unitCard != null)
        {
            added = cardStorage.TryAddToDeck(
                deckSlotNumber, unitCard, out reason
            );
        }

        if (!added && !string.IsNullOrEmpty(reason))
            Debug.LogWarning(reason);

        RefreshDisplay();
        refreshAllEntries?.Invoke();
    }

    private void RemoveOneCard()
    {
        if (cardStorage == null)
            return;

        if (playerCard != null)
        {
            cardStorage.RemoveFromDeck(
                deckSlotNumber, playerCard
            );
        }
        else if (heroCard != null)
        {
            cardStorage.RemoveFromDeck(
                deckSlotNumber, heroCard
            );
        }
        else if (unitCard != null)
        {
            cardStorage.RemoveFromDeck(
                deckSlotNumber, unitCard
            );
        }

        RefreshDisplay();
        refreshAllEntries?.Invoke();
    }

    private bool CanAddCardToDeck()
    {
        if (cardStorage == null)
            return false;

        string reason;

        if (playerCard != null)
        {
            return cardStorage.CanAddToDeck(
                deckSlotNumber, playerCard, out reason
            );
        }

        if (heroCard != null)
        {
            return cardStorage.CanAddToDeck(
                deckSlotNumber, heroCard, out reason
            );
        }

        if (unitCard != null)
        {
            return cardStorage.CanAddToDeck(
                deckSlotNumber, unitCard, out reason
            );
        }

        return false;
    }

    private int GetOwnedAmount()
    {
        if (playerCard != null)
            return cardStorage.GetCardAmount(playerCard);

        if (heroCard != null)
            return cardStorage.GetCardAmount(heroCard);

        if (unitCard != null)
            return cardStorage.GetCardAmount(unitCard);

        return 0;
    }

    private int GetDeckAmount()
    {
        if (playerCard != null)
        {
            return cardStorage.GetDeckCardAmount(
                deckSlotNumber, playerCard
            );
        }

        if (heroCard != null)
        {
            return cardStorage.GetDeckCardAmount(
                deckSlotNumber, heroCard
            );
        }

        if (unitCard != null)
        {
            return cardStorage.GetDeckCardAmount(
                deckSlotNumber, unitCard
            );
        }

        return 0;
    }

    private string GetCardDisplayName()
    {
        if (playerCard != null)
        {
            return "PLAYER - " +
                Nicify(playerCard.characterName.ToString());
        }

        if (heroCard != null)
        {
            return "HERO - " +
                Nicify(heroCard.characterName.ToString());
        }

        if (unitCard != null)
        {
            string unitName = string.IsNullOrWhiteSpace(unitCard.title)
                ? unitCard.name
                : unitCard.title;

            return "UNIT - " + unitName;
        }

        return "Unknown Card";
    }

    private string Nicify(string originalText)
    {
        if (string.IsNullOrEmpty(originalText))
            return string.Empty;

        return Regex.Replace(
            originalText,
            "([a-z])([A-Z])",
            "$1 $2"
        );
    }
}