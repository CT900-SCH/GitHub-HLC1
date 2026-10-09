using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class UnitCardView : MonoBehaviour
{
    [Header("Unit Card Data")]

    [SerializeField]
    private UnitCardData cardData;

    public UnitCardData CardData => cardData;

    [Header("Card Images")]

    [SerializeField]
    private Image artworkImage;

    [SerializeField]
    private Image illustrationImage;

    [SerializeField]
    private Image roleIconImage;

    [SerializeField]
    private Image classIconImage;

    [SerializeField]
    private Image expansionIconImage;

    [Header("Weapon Level Boxes")]

    [Tooltip("The white Weapon Class Level box images.")]
    [SerializeField]
    private Image[] weaponLevelBoxImages;

    [Tooltip("The weapon icons placed over the white boxes.")]
    [SerializeField]
    private Image[] weaponClassIconImages;

    [Header("Card Information Text")]

    [SerializeField]
    private TMP_Text illustratorNameText;

    [SerializeField]
    private TMP_Text cardIDText;

    [SerializeField]
    private TMP_Text cardTypeText;

    [SerializeField]
    private TMP_Text costText;

    [Header("Character Identity Text")]

    [SerializeField]
    private TMP_Text titleText;

    // Units deliberately have no Character Name.

    [SerializeField]
    private TMP_Text raceText;

    [SerializeField]
    private TMP_Text roleText;

    [SerializeField]
    private TMP_Text classText;

    [Header("Statistics Text")]

    [SerializeField]
    private TMP_Text sizeText;

    [SerializeField]
    private TMP_Text moveText;

    [SerializeField]
    private TMP_Text attackText;

    [SerializeField]
    private TMP_Text defenseText;

    [Header("Weapon Text")]

    [SerializeField]
    private TMP_Text mainWeaponText;

    [Header("Card Writing Text")]

    [SerializeField]
    private TMP_Text descriptionText;

    [SerializeField]
    private TMP_Text flavorText;

    private void OnEnable()
    {
        RefreshCard();
    }

    private void OnValidate()
    {
        RefreshCard();
    }

    [ContextMenu("Refresh Card")]
    public void RefreshCard()
    {
        if (cardData == null)
            return;

        cardData.EnsureWeaponArrays();

        RefreshImages();
        RefreshCardInformation();
        RefreshCharacterIdentity();
        RefreshStatistics();
        RefreshWeaponInformation();
        RefreshWriting();
    }

    private void RefreshImages()
    {
        SetImage(artworkImage, cardData.cardArtwork);

        SetImage(illustrationImage, cardData.illustrationArtwork);

        if (illustrationImage != null) illustrationImage.preserveAspect = true;

        SetImage(roleIconImage, cardData.roleIcon);

        SetImage(classIconImage, cardData.classIcon);

        SetImage(expansionIconImage, cardData.expansionIcon);
    }

    private void RefreshCardInformation()
    {
        SetText(
            illustratorNameText,
            "Illust. " + cardData.illustratorName
        );

        SetText(
            cardIDText,
            cardData.cardID
        );

        SetText(
            cardTypeText,
            "UNIT"
        );

        SetText(
            costText,
            cardData.cardCost.ToString()
        );
    }

    private void RefreshCharacterIdentity()
    {
        SetText(
            titleText,
            (cardData.title ?? string.Empty)
                .ToUpperInvariant()
        );

        SetText(
            raceText,
            Nicify(cardData.race.ToString())
        );

        SetText(
            roleText,
            Nicify(cardData.role.ToString())
                .ToUpperInvariant()
        );

        SetText(
            classText,
            Nicify(cardData.characterClass.ToString())
                .ToUpperInvariant()
        );
    }

    private void RefreshStatistics()
    {
        SetText(
            sizeText,
            cardData.size.ToString()
        );

        SetText(
            moveText,
            cardData.moveSpeed.ToString()
        );

        SetText(
            attackText,
            cardData.attack.ToString()
        );

        SetText(
            defenseText,
            cardData.defense.ToString()
        );
    }

    private void RefreshWeaponInformation()
    {
        SetText(
            mainWeaponText,
            Nicify(cardData.weaponClass.ToString())
                .ToUpperInvariant()
        );

        RefreshWeaponLevelBoxes();
        RefreshWeaponClassIcons();
    }

    private void RefreshWeaponLevelBoxes()
    {
        if (weaponLevelBoxImages == null)
            return;

        for (int i = 0; i < weaponLevelBoxImages.Length; i++)
        {
            Sprite boxSprite = null;

            if (cardData.weaponClassLevelBoxSprites != null &&
                i < cardData.weaponClassLevelBoxSprites.Length)
            {
                boxSprite =
                    cardData.weaponClassLevelBoxSprites[i];
            }

            SetImage(
                weaponLevelBoxImages[i],
                boxSprite
            );
        }
    }

    private void RefreshWeaponClassIcons()
    {
        if (weaponClassIconImages == null)
            return;

        int iconAmount =
            (int)cardData.weaponClassIconAmount;

        for (int i = 0; i < weaponClassIconImages.Length; i++)
        {
            Sprite weaponSprite = null;

            if (i < iconAmount &&
                cardData.weaponClassIconSprites != null &&
                i < cardData.weaponClassIconSprites.Length)
            {
                weaponSprite =
                    cardData.weaponClassIconSprites[i];
            }

            SetImage(
                weaponClassIconImages[i],
                weaponSprite
            );
        }
    }

    private void RefreshWriting()
    {
        SetText(
            descriptionText,
            cardData.description
        );

        SetText(
            flavorText,
            cardData.flavorText
        );
    }

    private void SetText(
        TMP_Text targetText,
        string value
    )
    {
        if (targetText == null)
            return;

        targetText.text =
            string.IsNullOrEmpty(value)
                ? string.Empty
                : value;
    }

    private void SetImage(
        Image targetImage,
        Sprite sprite
    )
    {
        if (targetImage == null)
            return;

        targetImage.sprite = sprite;
        targetImage.enabled = sprite != null;
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

    public void SetCard(UnitCardData newCardData)
    {
        cardData = newCardData;
        RefreshCard();
    }
}