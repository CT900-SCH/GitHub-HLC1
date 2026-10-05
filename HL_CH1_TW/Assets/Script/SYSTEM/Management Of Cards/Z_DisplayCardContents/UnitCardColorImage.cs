using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(Image))]
public class UnitCardColorImage : MonoBehaviour
{
    [Header("Five Color Versions")]

    [SerializeField]
    private Sprite redSprite;

    [SerializeField]
    private Sprite blueSprite;

    [SerializeField]
    private Sprite greenSprite;

    [SerializeField]
    private Sprite yellowSprite;

    [SerializeField]
    private Sprite purpleSprite;

    private Image targetImage;
    private UnitCardView unitCardView;

    private UnitCardData previousCardData;
    private CardColor previousColor;

    private void OnEnable()
    {
        FindReferences();
        RefreshImage();
    }

    private void OnValidate()
    {
        FindReferences();
        RefreshImage();
    }

    private void OnTransformParentChanged()
    {
        FindReferences();
        RefreshImage();
    }

    private void Update()
    {
        if (unitCardView == null)
        {
            FindReferences();
        }

        UnitCardData data = GetCardData();

        if (data == null)
            return;

        if (previousCardData != data ||
            previousColor != data.cardColor)
        {
            RefreshImage();
        }
    }

    private void FindReferences()
    {
        if (targetImage == null)
        {
            targetImage = GetComponent<Image>();
        }

        if (unitCardView == null)
        {
            unitCardView =
                GetComponentInParent<UnitCardView>(true);
        }
    }

    private UnitCardData GetCardData()
    {
        if (unitCardView == null)
            return null;

        return unitCardView.CardData;
    }

    [ContextMenu("Refresh Color Image")]
    public void RefreshImage()
    {
        FindReferences();

        UnitCardData data = GetCardData();

        if (targetImage == null || data == null)
            return;

        Sprite selectedSprite = null;

        switch (data.cardColor)
        {
            case CardColor.Red:
                selectedSprite = redSprite;
                break;

            case CardColor.Blue:
                selectedSprite = blueSprite;
                break;

            case CardColor.Green:
                selectedSprite = greenSprite;
                break;

            case CardColor.Yellow:
                selectedSprite = yellowSprite;
                break;

            case CardColor.Purple:
                selectedSprite = purpleSprite;
                break;
        }

        if (selectedSprite != null)
        {
            targetImage.sprite = selectedSprite;
        }

        previousCardData = data;
        previousColor = data.cardColor;
    }
}