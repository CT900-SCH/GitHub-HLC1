using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RandomCardRewardButton : MonoBehaviour
{
    [Header("Button")]

    [SerializeField]
    private Button rewardButton;

    [Header("Possible Card Rewards")]

    [Tooltip("Drag PlayerCardData, HeroCardData, or UnitCardData assets here.")]
    [SerializeField]
    private List<ScriptableObject> possibleCards =
        new List<ScriptableObject>();

    [Header("Button Settings")]

    [Tooltip("If enabled, this button can only successfully give one card.")]
    [SerializeField]
    private bool oneUseOnly;

    [SerializeField]
    private bool showDebugMessages = true;

    private bool hasBeenUsed;

    private void Reset()
    {
        rewardButton =
            GetComponent<Button>();
    }

    private void Awake()
    {
        if (rewardButton == null)
        {
            rewardButton =
                GetComponent<Button>();
        }
    }

    private void OnEnable()
    {
        if (rewardButton == null)
            return;

        rewardButton.onClick.RemoveListener(
            GiveRandomCard
        );

        rewardButton.onClick.AddListener(
            GiveRandomCard
        );
    }

    private void OnDisable()
    {
        if (rewardButton == null)
            return;

        rewardButton.onClick.RemoveListener(
            GiveRandomCard
        );
    }

    public void GiveRandomCard()
    {
        if (oneUseOnly && hasBeenUsed)
        {
            ShowMessage(
                "This card reward button has already been used."
            );

            return;
        }

        if (GameManager.Instance == null)
        {
            Debug.LogError(
                "RandomCardRewardButton could not find GameManager."
            );

            return;
        }

        CardStorage storage =
            GameManager.Instance.GetComponent<CardStorage>();

        if (storage == null)
        {
            Debug.LogError(
                "CardStorage is missing from GameManager."
            );

            return;
        }

        if (storage.IsFull)
        {
            Debug.LogWarning(
                "The Card Storage is full. No card was added."
            );

            return;
        }

        List<ScriptableObject> validCards =
            GetValidCards();

        if (validCards.Count == 0)
        {
            Debug.LogWarning(
                "The random card reward list contains no valid cards."
            );

            return;
        }

        /*
         * Try cards in a random order.
         *
         * If one randomly selected card cannot be added
         * because duplicates are disabled, the script tries
         * another card from the assigned list.
         */
        while (validCards.Count > 0)
        {
            int randomIndex =
                Random.Range(
                    0,
                    validCards.Count
                );

            ScriptableObject selectedCard =
                validCards[randomIndex];

            validCards.RemoveAt(randomIndex);

            bool cardWasAdded =
                TryAddCard(
                    storage,
                    selectedCard
                );

            if (!cardWasAdded)
                continue;

            hasBeenUsed = true;

            ShowMessage(
                "Random card added: " +
                selectedCard.name +
                " | Storage: " +
                storage.CurrentAmount +
                "/" +
                storage.Capacity
            );

            if (oneUseOnly)
            {
                rewardButton.interactable = false;
            }

            return;
        }

        Debug.LogWarning(
            "None of the assigned cards could be added. " +
            "They may already be collected while duplicate cards are disabled."
        );
    }

    private List<ScriptableObject> GetValidCards()
    {
        List<ScriptableObject> validCards =
            new List<ScriptableObject>();

        foreach (
            ScriptableObject card
            in possibleCards
        )
        {
            if (card == null)
                continue;

            if (IsSupportedCardType(card))
            {
                validCards.Add(card);
            }
            else
            {
                Debug.LogWarning(
                    card.name +
                    " was ignored because it is not a " +
                    "PlayerCardData, HeroCardData, or UnitCardData."
                );
            }
        }

        return validCards;
    }

    private bool IsSupportedCardType(
        ScriptableObject card
    )
    {
        return card is PlayerCardData ||
               card is HeroCardData ||
               card is UnitCardData;
    }

    private bool TryAddCard(
        CardStorage storage,
        ScriptableObject selectedCard
    )
    {
        if (selectedCard is PlayerCardData playerCard)
        {
            return storage.AddCard(
                playerCard
            );
        }

        if (selectedCard is HeroCardData heroCard)
        {
            return storage.AddCard(
                heroCard
            );
        }

        if (selectedCard is UnitCardData unitCard)
        {
            return storage.AddCard(
                unitCard
            );
        }

        return false;
    }

    private void ShowMessage(
        string message
    )
    {
        if (showDebugMessages)
        {
            Debug.Log(message);
        }
    }
}