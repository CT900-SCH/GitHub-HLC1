using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerCardData))]
public class PlayerCardDataEditor : Editor
{
    private const string IconLibraryPath =
        "CardIcons/PlayerCardIconLibrary";

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

        DrawHeader("Character Statistics");

        DrawProperty("carry");
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

        PlayerCardData card =
            (PlayerCardData)target;

        card.EnsureWeaponArrays();

        DrawWeaponIconSelectors(card);

        PlayerCardIconLibrary library =
            Resources.Load<PlayerCardIconLibrary>(
                IconLibraryPath
            );

        if (library == null)
        {
            EditorGUILayout.Space(8);

            EditorGUILayout.HelpBox(
                "PlayerCardIconLibrary was not found.\n\n" +
                "Create or move it to:\n" +
                "Assets/Resources/CardIcons/" +
                "PlayerCardIconLibrary.asset",
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
        PlayerCardData card
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
            PlayerWeaponClass currentSelection =
                card.selectedWeaponBoxIcons[i];

            EditorGUI.BeginChangeCheck();

            PlayerWeaponClass newSelection =
                (PlayerWeaponClass)
                EditorGUILayout.EnumPopup(
                    "Box " + (i + 1) + " Icon",
                    currentSelection
                );

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(
                    card,
                    "Change Weapon Box Icon"
                );

                card.selectedWeaponBoxIcons[i] =
                    newSelection;

                EditorUtility.SetDirty(card);
            }
        }
    }

    private void ApplyAutomaticIcons(
        PlayerCardData card,
        PlayerCardIconLibrary library
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
                PlayerWeaponClass selectedIcon =
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

    private void DrawAutomaticPreview(PlayerCardData card, PlayerCardIconLibrary library)
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

        DrawWeaponBoxes(card, true);
    }

    private void DrawWeaponBoxes(
        PlayerCardData card,
        bool drawWeaponIcons
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

            GUI.Box(boxRect, GUIContent.none);

            Sprite boxSprite =
                card.weaponClassLevelBoxSprites[i];

            if (boxSprite != null)
            {
                DrawSprite(boxRect, boxSprite);
            }

            if (drawWeaponIcons &&
                card.weaponClassIconSprites != null &&
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