using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class InventoryCardListController : MonoBehaviour
{
    private const string CardContentPath =
        "CardStorage/Text_CardContent";

    [Header("Scroll View")]

    [Tooltip(
        "Assign CardListScrollView/Viewport/Content here."
    )]
    [SerializeField]
    private Transform contentParent;

    private CardStorage cardStorage;

    private InventoryCardListItem cardContentPrefab;

    private void OnEnable()
    {
        FindCardStorage();
        LoadCardContentPrefab();

        if (cardStorage != null)
        {
            cardStorage.OnStorageChanged +=
                RefreshInventoryList;
        }

        RefreshInventoryList();
    }

    private void OnDisable()
    {
        if (cardStorage != null)
        {
            cardStorage.OnStorageChanged -=
                RefreshInventoryList;
        }
    }

    private void FindCardStorage()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError(
                "InventoryCardListController cannot find GameManager."
            );

            return;
        }

        cardStorage =
            GameManager.Instance.GetComponent<CardStorage>();

        if (cardStorage == null)
        {
            Debug.LogError(
                "CardStorage is missing from GameManager."
            );
        }
    }

    private void LoadCardContentPrefab()
    {
        cardContentPrefab =
            Resources.Load<InventoryCardListItem>(
                CardContentPath
            );

        if (cardContentPrefab == null)
        {
            Debug.LogError(
                "Could not find Text_CardContent prefab at:\n" +
                "Assets/Resources/CardStorage/" +
                "Text_CardContent.prefab"
            );
        }
    }

    [ContextMenu("Refresh Inventory List")]
    public void RefreshInventoryList()
    {
        if (contentParent == null)
        {
            Debug.LogError(
                "Content Parent has not been assigned."
            );

            return;
        }

        ClearCurrentRows();

        if (cardStorage == null ||
            cardContentPrefab == null)
        {
            return;
        }

        CreatePlayerCardRows();
        CreateHeroCardRows();
        CreateUnitCardRows();
    }

    private void CreatePlayerCardRows()
    {
        HashSet<PlayerCardData> displayedCards =
            new HashSet<PlayerCardData>();

        foreach (
            PlayerCardData card
            in cardStorage.PlayerCards
        )
        {
            if (card == null ||
                !displayedCards.Add(card))
            {
                continue;
            }

            int inventoryAmount =
                cardStorage.GetCardAmount(card);

            CreateRow(
                GetPlayerCardName(card),
                inventoryAmount,
                0
            );
        }
    }

    private void CreateHeroCardRows()
    {
        HashSet<HeroCardData> displayedCards =
            new HashSet<HeroCardData>();

        foreach (
            HeroCardData card
            in cardStorage.HeroCards
        )
        {
            if (card == null ||
                !displayedCards.Add(card))
            {
                continue;
            }

            int inventoryAmount =
                cardStorage.GetCardAmount(card);

            CreateRow(
                GetHeroCardName(card),
                inventoryAmount,
                0
            );
        }
    }

    private void CreateUnitCardRows()
    {
        HashSet<UnitCardData> displayedCards =
            new HashSet<UnitCardData>();

        foreach (
            UnitCardData card
            in cardStorage.UnitCards
        )
        {
            if (card == null ||
                !displayedCards.Add(card))
            {
                continue;
            }

            int inventoryAmount =
                cardStorage.GetCardAmount(card);

            CreateRow(
                GetUnitCardName(card),
                inventoryAmount,
                0
            );
        }
    }

    private void CreateRow(
        string cardName,
        int inventoryAmount,
        int deckAmount
    )
    {
        InventoryCardListItem newRow =
            Instantiate(
                cardContentPrefab,
                contentParent
            );

        newRow.SetContent(
            cardName,
            inventoryAmount,
            deckAmount
        );
    }

    private string GetPlayerCardName(
        PlayerCardData card
    )
    {
        if (!string.IsNullOrWhiteSpace(card.title))
        {
            return card.title;
        }

        return Nicify(
            card.characterName.ToString()
        );
    }

    private string GetHeroCardName(
        HeroCardData card
    )
    {
        if (!string.IsNullOrWhiteSpace(card.title))
        {
            return card.title;
        }

        return Nicify(
            card.characterName.ToString()
        );
    }

    private string GetUnitCardName(
        UnitCardData card
    )
    {
        if (!string.IsNullOrWhiteSpace(card.title))
        {
            return card.title;
        }

        return Nicify(
            card.characterClass.ToString()
        ) + " Unit";
    }

    private void ClearCurrentRows()
    {
        for (
            int i = contentParent.childCount - 1;
            i >= 0;
            i--
        )
        {
            Destroy(
                contentParent.GetChild(i).gameObject
            );
        }
    }

    private string Nicify(
        string originalText
    )
    {
        if (string.IsNullOrWhiteSpace(originalText))
        {
            return "Unnamed Card";
        }

        return Regex.Replace(
            originalText,
            "([a-z])([A-Z])",
            "$1 $2"
        );
    }
}