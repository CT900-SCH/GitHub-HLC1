using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HeroCardData))]
public class HeroCardDataEditor : Editor
{
    private const string IconLibraryPath =
        "CardIcons/HeroCardIconLibrary";

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawHeader("Card Information");

        DrawProperty("cardID");
        DrawProperty("illustratorName");
        DrawProperty("cardColor");
        DrawProperty("role");

        DrawHeader("Card Images");

        DrawProperty("cardArtwork");
        DrawProperty("expansionIcon");

        DrawHeader("Battlefield Information");

        DrawProperty("cardCost");
        DrawProperty("size");
        DrawProperty("moveSpeed");
        DrawProperty("attack");
        DrawProperty("defense");

        DrawHeader("Character Identity");

        DrawProperty("title");
        DrawProperty("race");
        DrawProperty("characterName");
        DrawProperty("characterClass");

        DrawHeader("Weapon Information");

        DrawProperty("weaponClass");
        DrawProperty("weaponClassLevel");
        DrawProperty("weaponClassIconAmount");

        serializedObject.ApplyModifiedProperties();

        HeroCardData card =
            (HeroCardData)target;

        card.EnsureWeaponArrays();

        DrawWeaponIconSelectors(card);

        HeroCardIconLibrary library =
            Resources.Load<HeroCardIconLibrary>(
                IconLibraryPath
            );

        if (library == null)
        {
            EditorGUILayout.Space(8);

            EditorGUILayout.HelpBox(
                "HeroCardIconLibrary was not found.\n\n" +
                "Create or move it to:\n" +
                "Assets/Resources/CardIcons/" +
                "HeroCardIconLibrary.asset",
                MessageType.Error
            );
        }
        else
        {
            ApplyAutomaticIcons(card, library);
            DrawAutomaticPreview(card, library);
        }

        serializedObject.Update();

        DrawHeader("Card Writing");

        DrawProperty("description");
        DrawProperty("flavorText");

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawWeaponIconSelectors(
        HeroCardData card
    )
    {
        int totalBoxes =
            (int)card.weaponClassLevel;

        int iconBoxes = Mathf.Clamp(
            (int)card.weaponClassIconAmount,
            0,
            totalBoxes
        );

        EditorGUILayout.Space(8);

        EditorGUILayout.LabelField(
            "Weapon Box Icon Selections",
            EditorStyles.boldLabel
        );

        if (iconBoxes == 0)
        {
            EditorGUILayout.HelpBox(
                "Weapon Class Icon Amount is None. " +
                "No weapon boxes will contain icons.",
                MessageType.Info
            );

            return;
        }

        for (int i = 0; i < iconBoxes; i++)
        {
            HeroWeaponClass currentSelection =
                card.selectedWeaponBoxIcons[i];

            EditorGUI.BeginChangeCheck();

            HeroWeaponClass newSelection =
                (HeroWeaponClass)
                EditorGUILayout.EnumPopup(
                    "Box " + (i + 1) + " Icon",
                    currentSelection
                );

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(
                    card,
                    "Change Hero Weapon Box Icon"
                );

                card.selectedWeaponBoxIcons[i] =
                    newSelection;

                EditorUtility.SetDirty(card);
            }
        }
    }

    private void ApplyAutomaticIcons(
        HeroCardData card,
        HeroCardIconLibrary library
    )
    {
        card.EnsureWeaponArrays();

        bool changed = false;

        Sprite requiredRoleIcon =
            library.GetRoleIcon(card.role);

        Sprite requiredClassIcon =
            library.GetClassIcon(
                card.characterClass
            );

        if (card.roleIcon != requiredRoleIcon)
        {
            card.roleIcon = requiredRoleIcon;
            changed = true;
        }

        if (card.classIcon != requiredClassIcon)
        {
            card.classIcon = requiredClassIcon;
            changed = true;
        }

        int totalBoxes =
            (int)card.weaponClassLevel;

        int iconBoxes = Mathf.Clamp(
            (int)card.weaponClassIconAmount,
            0,
            totalBoxes
        );

        for (int i = 0; i < totalBoxes; i++)
        {
            if (card.weaponClassLevelBoxSprites[i] !=
                library.whiteLevelBox)
            {
                card.weaponClassLevelBoxSprites[i] =
                    library.whiteLevelBox;

                changed = true;
            }

            Sprite requiredWeaponIcon = null;

            if (i < iconBoxes)
            {
                HeroWeaponClass selectedIcon =
                    card.selectedWeaponBoxIcons[i];

                requiredWeaponIcon =
                    library.GetWeaponIcon(
                        selectedIcon
                    );
            }

            if (card.weaponClassIconSprites[i] !=
                requiredWeaponIcon)
            {
                card.weaponClassIconSprites[i] =
                    requiredWeaponIcon;

                changed = true;
            }
        }

        if (changed)
        {
            EditorUtility.SetDirty(card);
        }
    }

    private void DrawAutomaticPreview(
        HeroCardData card,
        HeroCardIconLibrary library
    )
    {
        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField(
            "Selected Icon Preview",
            EditorStyles.boldLabel
        );

        EditorGUILayout.BeginHorizontal();

        DrawLabeledPreview(
            card.role.ToString(),
            card.roleIcon
        );

        DrawLabeledPreview(
            card.characterClass.ToString(),
            card.classIcon
        );

        DrawLabeledPreview(
            card.weaponClass.ToString(),
            library.GetWeaponIcon(
                card.weaponClass
            )
        );

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(8);

        EditorGUILayout.LabelField(
            "Weapon Boxes With Selected Icons",
            EditorStyles.miniBoldLabel
        );

        DrawWeaponBoxes(card);
    }

    private void DrawWeaponBoxes(
        HeroCardData card
    )
    {
        if (card.weaponClassLevelBoxSprites == null)
            return;

        EditorGUILayout.BeginHorizontal();

        for (
            int i = 0;
            i < card.weaponClassLevelBoxSprites.Length;
            i++
        )
        {
            Rect boxRect = GUILayoutUtility.GetRect(
                55,
                55,
                GUILayout.Width(55),
                GUILayout.Height(55)
            );

            GUI.Box(
                boxRect,
                GUIContent.none
            );

            Sprite boxSprite =
                card.weaponClassLevelBoxSprites[i];

            if (boxSprite != null)
            {
                DrawSprite(
                    boxRect,
                    boxSprite
                );
            }

            if (card.weaponClassIconSprites != null &&
                i < card.weaponClassIconSprites.Length)
            {
                Sprite weaponIcon =
                    card.weaponClassIconSprites[i];

                if (weaponIcon != null)
                {
                    Rect iconRect = new Rect(
                        boxRect.x + 8,
                        boxRect.y + 8,
                        boxRect.width - 16,
                        boxRect.height - 16
                    );

                    DrawSprite(
                        iconRect,
                        weaponIcon
                    );
                }
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawLabeledPreview(
        string label,
        Sprite sprite
    )
    {
        EditorGUILayout.BeginVertical(
            GUILayout.Width(90)
        );

        EditorGUILayout.LabelField(
            label,
            EditorStyles.centeredGreyMiniLabel,
            GUILayout.Width(85)
        );

        Rect previewRect =
            GUILayoutUtility.GetRect(
                75,
                75,
                GUILayout.Width(75),
                GUILayout.Height(75)
            );

        GUI.Box(
            previewRect,
            GUIContent.none
        );

        if (sprite != null)
        {
            DrawSprite(
                previewRect,
                sprite
            );
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawSprite(
        Rect rect,
        Sprite sprite
    )
    {
        if (sprite == null ||
            sprite.texture == null)
        {
            return;
        }

        Texture2D texture =
            sprite.texture;

        Rect spriteRect =
            sprite.textureRect;

        Rect textureCoordinates =
            new Rect(
                spriteRect.x / texture.width,
                spriteRect.y / texture.height,
                spriteRect.width / texture.width,
                spriteRect.height / texture.height
            );

        GUI.DrawTextureWithTexCoords(
            rect,
            texture,
            textureCoordinates,
            true
        );
    }

    private void DrawHeader(string text)
    {
        EditorGUILayout.Space(8);

        EditorGUILayout.LabelField(
            text,
            EditorStyles.boldLabel
        );
    }

    private void DrawProperty(
        string propertyName
    )
    {
        SerializedProperty property =
            serializedObject.FindProperty(
                propertyName
            );

        if (property != null)
        {
            EditorGUILayout.PropertyField(
                property,
                true
            );
        }
    }
}