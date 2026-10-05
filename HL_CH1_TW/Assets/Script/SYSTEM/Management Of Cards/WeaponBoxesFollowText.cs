using TMPro;
using UnityEngine;

[ExecuteAlways]
public class WeaponBoxesFollowText : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text mainWeaponText;
    [SerializeField] private RectTransform weaponClassBoxes;

    [Header("Spacing")]
    [SerializeField] private float spacing = 8f;

    private void LateUpdate()
    {
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        if (mainWeaponText == null || weaponClassBoxes == null)
            return;

        mainWeaponText.ForceMeshUpdate();

        RectTransform row = transform as RectTransform;
        RectTransform textRect =
            mainWeaponText.transform as RectTransform;

        if (row == null || textRect == null)
            return;

        // Find where the text begins inside Weapon Display Row.
        Bounds textBounds =
            RectTransformUtility.CalculateRelativeRectTransformBounds(
                row,
                textRect
            );

        // Find the current left edge of the complete box group.
        Bounds boxBounds =
            RectTransformUtility.CalculateRelativeRectTransformBounds(
                row,
                weaponClassBoxes
            );

        // Actual width required by the current weapon name.
        float textWidth = mainWeaponText.preferredWidth;

        // Desired left position for the weapon boxes.
        float targetLeft =
            textBounds.min.x + textWidth + spacing;

        float movement =
            targetLeft - boxBounds.min.x;

        if (Mathf.Abs(movement) > 0.01f)
        {
            weaponClassBoxes.anchoredPosition +=
                new Vector2(movement, 0f);
        }
    }
}