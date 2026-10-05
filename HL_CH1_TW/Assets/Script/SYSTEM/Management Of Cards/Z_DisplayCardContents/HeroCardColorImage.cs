using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(Image))]
public class HeroCardColorImage : MonoBehaviour
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
    private HeroCardView heroCardView;

    private HeroCardData previousCardData;
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
        if (heroCardView == null)
        {
            FindReferences();
        }

        HeroCardData cardData = GetCardData();

        if (cardData == null)
            return;

        if (previousCardData != cardData ||
            previousColor != cardData.cardColor)
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

        if (heroCardView == null)
        {
            heroCardView =
                GetComponentInParent<HeroCardView>(true);
        }
    }

    private HeroCardData GetCardData()
    {
        if (heroCardView == null)
            return null;

        return heroCardView.CardData;
    }

    [ContextMenu("Refresh Color Image")]
    public void RefreshImage()
    {
        FindReferences();

        HeroCardData cardData = GetCardData();

        if (targetImage == null || cardData == null)
            return;

        Sprite selectedSprite = null;

        switch (cardData.cardColor)
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

        previousCardData = cardData;
        previousColor = cardData.cardColor;
    }
}