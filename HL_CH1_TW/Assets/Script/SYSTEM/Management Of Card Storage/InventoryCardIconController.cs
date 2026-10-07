using UnityEngine;
using UnityEngine.UI;

public class InventoryCardIconController : MonoBehaviour
{
    private const string UnitCardPrefabPath = "Prefabs/A_CardDisplays/CardDisplay_Prefab_Unit";

    [Header("Icon Mode")]

    [SerializeField]
    private RectTransform contentParent;

    [SerializeField]
    private GridLayoutGroup gridLayoutGroup;

    [Header("Selected Card Display")]

    [SerializeField]
    private InventorySelectedCardDisplay selectedCardDisplay;

    [Header("Original Card Size")]

    [SerializeField]
    private Vector2 originalCardSize =
        new Vector2(750f, 1050f);

    [Header("Generated Card Scale")]

    [SerializeField]
    private Vector3 iconScale =
        new Vector3(0.35f, 0.35f, 0.35f);

    private CardStorage cardStorage;
    private UnitCardView unitCardPrefab;
    private bool subscribed;

    private void OnEnable()
    {
        LoadUnitCardPrefab();
        FindCardStorage();
        PrepareGridLayout();
        SubscribeToStorage();
        RefreshCardIcons();
    }

    private void Update()
    {
        if (cardStorage != null)
        {
            return;
        }

        FindCardStorage();

        if (cardStorage != null)
        {
            SubscribeToStorage();
            RefreshCardIcons();
        }
    }

    private void OnDisable()
    {
        UnsubscribeFromStorage();
    }

    private void LoadUnitCardPrefab()
    {
        if (unitCardPrefab != null)
        {
            return;
        }

        unitCardPrefab =
            Resources.Load<UnitCardView>(
                UnitCardPrefabPath
            );

        if (unitCardPrefab == null)
        {
            Debug.LogError(
                "Cannot find CardDisplay_Prefab_Unit.\n" +
                "Expected location:\n" +
                "Assets/Resources/Prefabs/" +
                "A_CardDisplays/" +
                "CardDisplay_Prefab_Unit.prefab"
            );
        }
    }

    private void FindCardStorage()
    {
        if (GameManager.Instance == null)
        {
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

    private void PrepareGridLayout()
    {
        if (contentParent == null)
        {
            return;
        }

        if (gridLayoutGroup == null)
        {
            gridLayoutGroup =
                contentParent.GetComponent<GridLayoutGroup>();
        }

        if (gridLayoutGroup == null)
        {
            Debug.LogError(
                "Content requires a Grid Layout Group."
            );

            return;
        }

        gridLayoutGroup.cellSize =
            new Vector2(
                originalCardSize.x * iconScale.x,
                originalCardSize.y * iconScale.y
            );
    }

    private void SubscribeToStorage()
    {
        if (cardStorage == null || subscribed)
        {
            return;
        }

        cardStorage.OnStorageChanged +=
            RefreshCardIcons;

        subscribed = true;
    }

    private void UnsubscribeFromStorage()
    {
        if (cardStorage == null || !subscribed)
        {
            return;
        }

        cardStorage.OnStorageChanged -=
            RefreshCardIcons;

        subscribed = false;
    }

    [ContextMenu("Refresh Card Icons")]
    public void RefreshCardIcons()
    {
        if (contentParent == null)
        {
            Debug.LogError(
                "Content Parent has not been assigned."
            );

            return;
        }

        ClearGeneratedIcons();
        PrepareGridLayout();

        if (cardStorage == null ||
            unitCardPrefab == null)
        {
            return;
        }

        foreach (
            UnitCardData unitCard
            in cardStorage.UnitCards
        )
        {
            if (unitCard == null)
            {
                continue;
            }

            CreateUnitCardIcon(unitCard);
        }

        Canvas.ForceUpdateCanvases();

        LayoutRebuilder.ForceRebuildLayoutImmediate(
            contentParent
        );
    }

    private void CreateUnitCardIcon(
        UnitCardData unitCard
    )
    {
        GameObject slotObject =
            new GameObject(
                "UnitCardSlot",
                typeof(RectTransform)
            );

        RectTransform slotRect =
            slotObject.GetComponent<RectTransform>();

        slotRect.SetParent(
            contentParent,
            false
        );

        UnitCardView newCardIcon =
            Instantiate(
                unitCardPrefab,
                slotRect,
                false
            );

        RectTransform cardRect =
            newCardIcon.GetComponent<RectTransform>();

        cardRect.anchorMin =
            new Vector2(0.5f, 0.5f);

        cardRect.anchorMax =
            new Vector2(0.5f, 0.5f);

        cardRect.pivot =
            new Vector2(0.5f, 0.5f);

        cardRect.anchoredPosition =
            Vector2.zero;

        cardRect.localRotation =
            Quaternion.identity;

        cardRect.sizeDelta =
            originalCardSize;

        cardRect.localScale =
            iconScale;

        newCardIcon.SetCard(unitCard);

        ConnectCardButton(
            newCardIcon,
            unitCard
        );
    }

    private void ConnectCardButton(
        UnitCardView cardView,
        UnitCardData unitCard
    )
    {
        Button cardButton =
            cardView.GetComponent<Button>();

        if (cardButton == null)
        {
            Debug.LogWarning(
                cardView.name +
                " does not have a Button component."
            );

            return;
        }

        UnitCardData selectedCard =
            unitCard;

        cardButton.onClick.AddListener(
            () =>
            {
                if (selectedCardDisplay != null)
                {
                    selectedCardDisplay
                        .DisplayUnitCard(selectedCard);
                }
            }
        );
    }

    private void ClearGeneratedIcons()
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
}