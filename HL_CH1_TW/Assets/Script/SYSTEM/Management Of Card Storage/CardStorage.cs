using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DeckSlotData
{
    [SerializeField]
    private List<PlayerCardData> playerCards = new List<PlayerCardData>();

    [SerializeField]
    private List<HeroCardData> heroCards = new List<HeroCardData>();

    [SerializeField]
    private List<UnitCardData> unitCards = new List<UnitCardData>();

    public IReadOnlyList<PlayerCardData> PlayerCards => playerCards;
    public IReadOnlyList<HeroCardData> HeroCards => heroCards;
    public IReadOnlyList<UnitCardData> UnitCards => unitCards;

    public int CardCount =>
        playerCards.Count + heroCards.Count + unitCards.Count;

    public int GetCardAmount(PlayerCardData card) =>
        CountCopies(playerCards, card);

    public int GetCardAmount(HeroCardData card) =>
        CountCopies(heroCards, card);

    public int GetCardAmount(UnitCardData card) =>
        CountCopies(unitCards, card);

    internal void AddCard(PlayerCardData card) => playerCards.Add(card);
    internal void AddCard(HeroCardData card) => heroCards.Add(card);
    internal void AddCard(UnitCardData card) => unitCards.Add(card);

    internal bool RemoveCard(PlayerCardData card) =>
        playerCards.Remove(card);

    internal bool RemoveCard(HeroCardData card) =>
        heroCards.Remove(card);

    internal bool RemoveCard(UnitCardData card) =>
        unitCards.Remove(card);

    internal void Clear()
    {
        playerCards.Clear();
        heroCards.Clear();
        unitCards.Clear();
    }

    private int CountCopies<T>(List<T> cards, T card)
        where T : ScriptableObject
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
}

[DisallowMultipleComponent]
public class CardStorage : MonoBehaviour
{
    public const int MaximumCapacity = 50;
    public const int DeckSlotAmount = 5;
    public const int DeckCardCapacity = 40;
    public const int MaximumDeckDuplicates = 4;

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

    [Header("Five Deck Slots")]

    [SerializeField]
    private List<DeckSlotData> deckSlots =
        new List<DeckSlotData>();

    public event Action OnStorageChanged;
    public event Action OnDecksChanged;

    public IReadOnlyList<PlayerCardData> PlayerCards =>
        playerCards;

    public IReadOnlyList<HeroCardData> HeroCards =>
        heroCards;

    public IReadOnlyList<UnitCardData> UnitCards =>
        unitCards;

    public IReadOnlyList<DeckSlotData> DeckSlots =>
        deckSlots;

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

    private void Awake()
    {
        EnsureDeckSlots();
    }

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

    // -------------------------
    // DECK SLOT INFORMATION
    // Slot numbers are 1 to 5.
    // -------------------------

    public DeckSlotData GetDeckSlot(int slotNumber)
    {
        EnsureDeckSlots();

        if (slotNumber < 1 || slotNumber > DeckSlotAmount)
        {
            Debug.LogWarning(
                "Deck Slot must be between 1 and " +
                DeckSlotAmount + "."
            );

            return null;
        }

        return deckSlots[slotNumber - 1];
    }

    public int GetDeckCardCount(int slotNumber)
    {
        DeckSlotData slot = GetDeckSlot(slotNumber);
        return slot == null ? 0 : slot.CardCount;
    }

    public bool IsDeckComplete(int slotNumber)
    {
        return GetDeckCardCount(slotNumber) == DeckCardCapacity;
    }

    public int GetDeckCardAmount(int slotNumber, PlayerCardData card)
    {
        DeckSlotData slot = GetDeckSlot(slotNumber);
        return slot == null ? 0 : slot.GetCardAmount(card);
    }

    public int GetDeckCardAmount(int slotNumber, HeroCardData card)
    {
        DeckSlotData slot = GetDeckSlot(slotNumber);
        return slot == null ? 0 : slot.GetCardAmount(card);
    }

    public int GetDeckCardAmount(int slotNumber, UnitCardData card)
    {
        DeckSlotData slot = GetDeckSlot(slotNumber);
        return slot == null ? 0 : slot.GetCardAmount(card);
    }

    // -------------------------
    // ADD CARDS TO A DECK
    // -------------------------

    public bool TryAddToDeck(
        int slotNumber,
        PlayerCardData card,
        out string reason
    )
    {
        DeckSlotData slot = GetDeckSlot(slotNumber);

        if (!CanAddToDeck(
                slot,
                card,
                GetCardAmount(card),
                slot == null ? 0 : slot.GetCardAmount(card),
                out reason))
        {
            return false;
        }

        slot.AddCard(card);
        DeckWasChanged();
        return true;
    }

    public bool TryAddToDeck(
        int slotNumber,
        HeroCardData card,
        out string reason
    )
    {
        DeckSlotData slot = GetDeckSlot(slotNumber);

        if (!CanAddToDeck(
                slot,
                card,
                GetCardAmount(card),
                slot == null ? 0 : slot.GetCardAmount(card),
                out reason))
        {
            return false;
        }

        slot.AddCard(card);
        DeckWasChanged();
        return true;
    }

    public bool TryAddToDeck(
        int slotNumber,
        UnitCardData card,
        out string reason
    )
    {
        DeckSlotData slot = GetDeckSlot(slotNumber);

        if (!CanAddToDeck(
                slot,
                card,
                GetCardAmount(card),
                slot == null ? 0 : slot.GetCardAmount(card),
                out reason))
        {
            return false;
        }

        slot.AddCard(card);
        DeckWasChanged();
        return true;
    }

    public bool CanAddToDeck(
        int slotNumber,
        PlayerCardData card,
        out string reason
    )
    {
        DeckSlotData slot = GetDeckSlot(slotNumber);

        return CanAddToDeck(
            slot,
            card,
            GetCardAmount(card),
            slot == null ? 0 : slot.GetCardAmount(card),
            out reason
        );
    }

    public bool CanAddToDeck(
        int slotNumber,
        HeroCardData card,
        out string reason
    )
    {
        DeckSlotData slot = GetDeckSlot(slotNumber);

        return CanAddToDeck(
            slot,
            card,
            GetCardAmount(card),
            slot == null ? 0 : slot.GetCardAmount(card),
            out reason
        );
    }

    public bool CanAddToDeck(
        int slotNumber,
        UnitCardData card,
        out string reason
    )
    {
        DeckSlotData slot = GetDeckSlot(slotNumber);

        return CanAddToDeck(
            slot,
            card,
            GetCardAmount(card),
            slot == null ? 0 : slot.GetCardAmount(card),
            out reason
        );
    }

    private bool CanAddToDeck<T>(
        DeckSlotData slot,
        T card,
        int ownedAmount,
        int deckAmount,
        out string reason
    ) where T : ScriptableObject
    {
        if (slot == null)
        {
            reason = "The selected Deck Slot does not exist.";
            return false;
        }

        if (card == null)
        {
            reason = "The selected Card Data is missing.";
            return false;
        }

        if (slot.CardCount >= DeckCardCapacity)
        {
            reason = "This deck already contains 40 cards.";
            return false;
        }

        if (deckAmount >= MaximumDeckDuplicates)
        {
            reason = "A deck can only carry 4 copies of the same card.";
            return false;
        }

        if (deckAmount >= ownedAmount)
        {
            reason = "You do not own another copy of this card.";
            return false;
        }

        reason = string.Empty;
        return true;
    }

    // -------------------------
    // REMOVE CARDS FROM A DECK
    // -------------------------

    public bool RemoveFromDeck(int slotNumber, PlayerCardData card)
    {
        DeckSlotData slot = GetDeckSlot(slotNumber);
        bool removed = slot != null && slot.RemoveCard(card);

        if (removed)
            DeckWasChanged();

        return removed;
    }

    public bool RemoveFromDeck(int slotNumber, HeroCardData card)
    {
        DeckSlotData slot = GetDeckSlot(slotNumber);
        bool removed = slot != null && slot.RemoveCard(card);

        if (removed)
            DeckWasChanged();

        return removed;
    }

    public bool RemoveFromDeck(int slotNumber, UnitCardData card)
    {
        DeckSlotData slot = GetDeckSlot(slotNumber);
        bool removed = slot != null && slot.RemoveCard(card);

        if (removed)
            DeckWasChanged();

        return removed;
    }

    public void ClearDeckSlot(int slotNumber)
    {
        DeckSlotData slot = GetDeckSlot(slotNumber);

        if (slot == null || slot.CardCount == 0)
            return;

        slot.Clear();
        DeckWasChanged();
    }

    private void CardWasChanged()
    {
        OnStorageChanged?.Invoke();
    }

    private void DeckWasChanged()
    {
        OnDecksChanged?.Invoke();
    }

    private void EnsureDeckSlots()
    {
        if (deckSlots == null)
            deckSlots = new List<DeckSlotData>();

        while (deckSlots.Count < DeckSlotAmount)
            deckSlots.Add(new DeckSlotData());
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

        EnsureDeckSlots();
    }
}
