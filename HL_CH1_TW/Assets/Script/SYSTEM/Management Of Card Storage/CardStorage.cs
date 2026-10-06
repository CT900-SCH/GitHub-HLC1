using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class CardStorage : MonoBehaviour
{
    public const int MaximumCapacity = 50;

    [Header("Storage Information")]

    [SerializeField]
    private int maximumCapacity = MaximumCapacity;

    [Header("Duplicate Rules")]

    [SerializeField]
    private bool allowDuplicatePlayerCards = false;

    [SerializeField]
    private bool allowDuplicateHeroCards = false;

    [SerializeField]
    private bool allowDuplicateUnitCards = true;

    [Header("Collected Player Cards")]

    [SerializeField]
    private List<PlayerCardData> playerCards =
        new List<PlayerCardData>();

    [Header("Collected Hero Cards")]

    [SerializeField]
    private List<HeroCardData> heroCards =
        new List<HeroCardData>();

    [Header("Collected Unit Cards")]

    [SerializeField]
    private List<UnitCardData> unitCards =
        new List<UnitCardData>();

    public event Action OnStorageChanged;

    public IReadOnlyList<PlayerCardData> PlayerCards =>
        playerCards;

    public IReadOnlyList<HeroCardData> HeroCards =>
        heroCards;

    public IReadOnlyList<UnitCardData> UnitCards =>
        unitCards;

    public int CurrentAmount =>
        playerCards.Count +
        heroCards.Count +
        unitCards.Count;

    public int RemainingSpace =>
        Mathf.Max(
            0,
            maximumCapacity - CurrentAmount
        );

    public int Capacity =>
        maximumCapacity;

    public bool IsFull =>
        CurrentAmount >= maximumCapacity;

    // -------------------------
    // ADD PLAYER CARD
    // -------------------------

    public bool AddCard(
        PlayerCardData card
    )
    {
        if (!CanAddCard(card))
            return false;

        playerCards.Add(card);

        CardWasChanged();

        Debug.Log(
            "Added Player Card: " +
            card.name +
            " | Storage: " +
            CurrentAmount +
            "/" +
            maximumCapacity
        );

        return true;
    }

    // -------------------------
    // ADD HERO CARD
    // -------------------------

    public bool AddCard(
        HeroCardData card
    )
    {
        if (!CanAddCard(card))
            return false;

        heroCards.Add(card);

        CardWasChanged();

        Debug.Log(
            "Added Hero Card: " +
            card.name +
            " | Storage: " +
            CurrentAmount +
            "/" +
            maximumCapacity
        );

        return true;
    }

    // -------------------------
    // ADD UNIT CARD
    // -------------------------

    public bool AddCard(
        UnitCardData card
    )
    {
        if (!CanAddCard(card))
            return false;

        unitCards.Add(card);

        CardWasChanged();

        Debug.Log(
            "Added Unit Card: " +
            card.name +
            " | Storage: " +
            CurrentAmount +
            "/" +
            maximumCapacity
        );

        return true;
    }

    // -------------------------
    // CHECK PLAYER CARD
    // -------------------------

    public bool CanAddCard(
        PlayerCardData card
    )
    {
        if (card == null)
        {
            Debug.LogWarning(
                "Cannot add Player Card because it is null."
            );

            return false;
        }

        if (IsFull)
        {
            StorageFullWarning();
            return false;
        }

        if (!allowDuplicatePlayerCards &&
            playerCards.Contains(card))
        {
            Debug.LogWarning(
                "Player Card already exists in storage: " +
                card.name
            );

            return false;
        }

        return true;
    }

    // -------------------------
    // CHECK HERO CARD
    // -------------------------

    public bool CanAddCard(
        HeroCardData card
    )
    {
        if (card == null)
        {
            Debug.LogWarning(
                "Cannot add Hero Card because it is null."
            );

            return false;
        }

        if (IsFull)
        {
            StorageFullWarning();
            return false;
        }

        if (!allowDuplicateHeroCards &&
            heroCards.Contains(card))
        {
            Debug.LogWarning(
                "Hero Card already exists in storage: " +
                card.name
            );

            return false;
        }

        return true;
    }

    // -------------------------
    // CHECK UNIT CARD
    // -------------------------

    public bool CanAddCard(
        UnitCardData card
    )
    {
        if (card == null)
        {
            Debug.LogWarning(
                "Cannot add Unit Card because it is null."
            );

            return false;
        }

        if (IsFull)
        {
            StorageFullWarning();
            return false;
        }

        if (!allowDuplicateUnitCards &&
            unitCards.Contains(card))
        {
            Debug.LogWarning(
                "Unit Card already exists in storage: " +
                card.name
            );

            return false;
        }

        return true;
    }

    // -------------------------
    // REMOVE CARDS
    // -------------------------

    public bool RemoveCard(
        PlayerCardData card
    )
    {
        if (card == null)
            return false;

        bool removed =
            playerCards.Remove(card);

        if (removed)
            CardWasChanged();

        return removed;
    }

    public bool RemoveCard(
        HeroCardData card
    )
    {
        if (card == null)
            return false;

        bool removed =
            heroCards.Remove(card);

        if (removed)
            CardWasChanged();

        return removed;
    }

    public bool RemoveCard(
        UnitCardData card
    )
    {
        if (card == null)
            return false;

        bool removed =
            unitCards.Remove(card);

        if (removed)
            CardWasChanged();

        return removed;
    }

    // -------------------------
    // CARD QUANTITIES
    // -------------------------

    public int GetCardAmount(
        PlayerCardData card
    )
    {
        return CountCardCopies(
            playerCards,
            card
        );
    }

    public int GetCardAmount(
        HeroCardData card
    )
    {
        return CountCardCopies(
            heroCards,
            card
        );
    }

    public int GetCardAmount(
        UnitCardData card
    )
    {
        return CountCardCopies(
            unitCards,
            card
        );
    }

    private int CountCardCopies<T>(
        List<T> cards,
        T card
    ) where T : ScriptableObject
    {
        if (card == null)
            return 0;

        int amount = 0;

        foreach (T storedCard in cards)
        {
            if (storedCard == card)
                amount++;
        }

        return amount;
    }

    // -------------------------
    // STORAGE UTILITIES
    // -------------------------

    public bool ContainsCard(
        PlayerCardData card
    )
    {
        return card != null &&
               playerCards.Contains(card);
    }

    public bool ContainsCard(
        HeroCardData card
    )
    {
        return card != null &&
               heroCards.Contains(card);
    }

    public bool ContainsCard(
        UnitCardData card
    )
    {
        return card != null &&
               unitCards.Contains(card);
    }

    private void CardWasChanged()
    {
        OnStorageChanged?.Invoke();
    }

    private void StorageFullWarning()
    {
        Debug.LogWarning(
            "Card Storage is full. Maximum capacity: " +
            maximumCapacity
        );
    }

    private void OnValidate()
    {
        maximumCapacity = Mathf.Max(
            1,
            maximumCapacity
        );
    }
}