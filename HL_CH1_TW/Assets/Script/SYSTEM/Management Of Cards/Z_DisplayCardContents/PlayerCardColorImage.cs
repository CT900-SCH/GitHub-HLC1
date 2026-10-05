using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(Image))]
public class PlayerCardColorImage : MonoBehaviour
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
    private PlayerCardView playerCardView;

    private PlayerCardData previousCardData;
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
        if (playerCardView == null)
        {
            FindReferences();
        }

        PlayerCardData cardData = GetCardData();

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

        if (playerCardView == null)
        {
            playerCardView =
                GetComponentInParent<PlayerCardView>(true);
        }
    }

    private PlayerCardData GetCardData()
    {
        if (playerCardView == null)
            return null;

        return playerCardView.CardData;
    }

    public void RefreshImage()
    {
        FindReferences();

        PlayerCardData cardData = GetCardData();

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