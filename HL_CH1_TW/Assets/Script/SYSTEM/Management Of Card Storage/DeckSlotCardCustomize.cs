using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class DeckSlotCardCustomize : MonoBehaviour
{
    [Header("Card List")]

    [Tooltip("Drag CardListScrollView/Viewport/Content here.")]
    [SerializeField]
    private RectTransform contentParent;

    private DeckSlotCardEntryUI cardEntryPrefab;

    [Header("Deck Information")]

    [Tooltip("Optional text that displays 0 / 40, 1 / 40, and so on.")]
    [SerializeField]
    private TMP_Text deckAmountText;

    [SerializeField]
    private Button saveDeckButton;

    private const string CardEntryResourcePath =
        "CardStorage/Text_CardContent_Equip";

    private readonly List<DeckSlotCardEntryUI> spawnedEntries =
        new List<DeckSlotCardEntryUI>();

    private CardStorage cardStorage;
    private DeckBuilderUI deckBuilderUI;
    private int selectedSlotNumber = 1;
    private bool subscribed;

    private void Start()
    {
        deckBuilderUI = GetComponentInParent<DeckBuilderUI>();

        if (deckBuilderUI != null &&
            deckBuilderUI.SelectedDeckSlot > 0)
        {
            selectedSlotNumber = deckBuilderUI.SelectedDeckSlot;
        }

        BindCardStorage();

        if (saveDeckButton != null)
        {
            saveDeckButton.onClick.RemoveListener(SaveDeck);
            saveDeckButton.onClick.AddListener(SaveDeck);
        }

        RebuildCardList();
    }

    private void OnDestroy()
    {
        UnsubscribeFromStorage();

        if (saveDeckButton != null)
            saveDeckButton.onClick.RemoveListener(SaveDeck);
    }

    private bool LoadCardEntryPrefab()
    {
        // Use the Inspector assignment if one already exists.
        if (cardEntryPrefab != null)
            return true;

        GameObject prefab = Resources.Load<GameObject>(
            CardEntryResourcePath
        );

        if (prefab == null)
        {
            Debug.LogError(
                "Cannot find the card entry prefab at " +
                "Assets/Resources/CardStorage/" +
                "Text_CardContent_Equip.prefab.",
                this
            );

            return false;
        }

        cardEntryPrefab =
            prefab.GetComponent<DeckSlotCardEntryUI>();

        if (cardEntryPrefab == null)
        {
            Debug.LogError(
                "Text_CardContent_Equip needs the " +
                "DeckSlotCardEntryUI component on its root GameObject. " +
                "Open the prefab and attach that component.",
                prefab
            );

            return false;
        }

        return true;
    }

    public void SelectDeckSlot(int slotNumber)
    {
        selectedSlotNumber = Mathf.Clamp(
            slotNumber,
            1,
            CardStorage.DeckSlotAmount
        );

        RefreshAllEntries();
    }

    [ContextMenu("Rebuild Card List")]
    public void RebuildCardList()
    {
        if (!Application.isPlaying)
            return;

        BindCardStorage();

        if (cardStorage == null || contentParent == null)
        {
            Debug.LogWarning(
                "DeckSlotCardCustomize is missing " +
                "CardStorage or Content Parent.",
                this
            );

            return;
        }

        if (!LoadCardEntryPrefab())
            return;

        ClearSpawnedEntries();

        HashSet<PlayerCardData> shownPlayerCards =
            new HashSet<PlayerCardData>();

        foreach (PlayerCardData card in cardStorage.PlayerCards)
        {
            if (card != null && shownPlayerCards.Add(card))
                CreateEntry(card);
        }

        HashSet<HeroCardData> shownHeroCards =
            new HashSet<HeroCardData>();

        foreach (HeroCardData card in cardStorage.HeroCards)
        {
            if (card != null && shownHeroCards.Add(card))
                CreateEntry(card);
        }

        HashSet<UnitCardData> shownUnitCards =
            new HashSet<UnitCardData>();

        foreach (UnitCardData card in cardStorage.UnitCards)
        {
            if (card != null && shownUnitCards.Add(card))
                CreateEntry(card);
        }

        RefreshDeckInformation();
    }

    private void CreateEntry(PlayerCardData card)
    {
        DeckSlotCardEntryUI entry = Instantiate(
            cardEntryPrefab,
            contentParent
        );

        entry.gameObject.SetActive(true);

        entry.SetUp(
            cardStorage,
            selectedSlotNumber,
            card,
            RefreshAllEntries
        );

        spawnedEntries.Add(entry);
    }

    private void CreateEntry(HeroCardData card)
    {
        DeckSlotCardEntryUI entry = Instantiate(
            cardEntryPrefab,
            contentParent
        );

        entry.gameObject.SetActive(true);

        entry.SetUp(
            cardStorage,
            selectedSlotNumber,
            card,
            RefreshAllEntries
        );

        spawnedEntries.Add(entry);
    }

    private void CreateEntry(UnitCardData card)
    {
        DeckSlotCardEntryUI entry = Instantiate(
            cardEntryPrefab,
            contentParent
        );

        entry.gameObject.SetActive(true);

        entry.SetUp(
            cardStorage,
            selectedSlotNumber,
            card,
            RefreshAllEntries
        );

        spawnedEntries.Add(entry);
    }

    private void RefreshAllEntries()
    {
        foreach (DeckSlotCardEntryUI entry in spawnedEntries)
        {
            if (entry != null)
            {
                entry.SetDeckSlot(selectedSlotNumber);
                entry.RefreshDisplay();
            }
        }

        RefreshDeckInformation();
    }

    private void RefreshDeckInformation()
    {
        if (cardStorage == null)
            return;

        int cardAmount =
            cardStorage.GetDeckCardCount(selectedSlotNumber);

        if (deckAmountText != null)
        {
            deckAmountText.text =
                cardAmount + " / " + CardStorage.DeckCardCapacity;
        }

        if (saveDeckButton != null)
        {
            saveDeckButton.interactable =
                cardStorage.IsDeckComplete(selectedSlotNumber);
        }
    }

    public void SaveDeck()
    {
        if (cardStorage == null)
            return;

        int cardAmount =
            cardStorage.GetDeckCardCount(selectedSlotNumber);

        if (!cardStorage.IsDeckComplete(selectedSlotNumber))
        {
            Debug.LogWarning(
                "Deck Slot " + selectedSlotNumber +
                " cannot be completed yet. It currently has " +
                cardAmount + "/" + CardStorage.DeckCardCapacity +
                " cards."
            );

            return;
        }

        Debug.Log(
            "Deck Slot " + selectedSlotNumber +
            " is complete with " +
            CardStorage.DeckCardCapacity + " cards."
        );
    }

    private void BindCardStorage()
    {
        if (cardStorage == null && GameManager.Instance != null)
        {
            cardStorage =
                GameManager.Instance.GetComponent<CardStorage>();
        }

        if (cardStorage == null || subscribed)
            return;

        cardStorage.OnStorageChanged += RebuildCardList;
        cardStorage.OnDecksChanged += RefreshAllEntries;
        subscribed = true;
    }

    private void UnsubscribeFromStorage()
    {
        if (cardStorage == null || !subscribed)
            return;

        cardStorage.OnStorageChanged -= RebuildCardList;
        cardStorage.OnDecksChanged -= RefreshAllEntries;
        subscribed = false;
    }

    private void ClearSpawnedEntries()
    {
        foreach (DeckSlotCardEntryUI entry in spawnedEntries)
        {
            if (entry != null)
            {
                entry.gameObject.SetActive(false);
                Destroy(entry.gameObject);
            }
        }

        spawnedEntries.Clear();
    }
}