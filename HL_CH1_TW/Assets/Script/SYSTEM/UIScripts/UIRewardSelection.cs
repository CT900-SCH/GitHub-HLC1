using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;


public enum RewardType
{
    Weapon,
    Skill
}

[System.Serializable]
public class RewardCardUI
{
    public string NameOfClass = "Put Reward Name Here";
    public Image spriteHolder; // UI Image to show weapon sprite
    public TMP_Text nameText;
    public TMP_Text rarityText;
    public TMP_Text typeText;
    public TMP_Text descriptionText;

    public void Setup(Weapon weapon)
    {
        NameOfClass = weapon.weaponName;
        nameText.text = weapon.weaponName;
        typeText.text = weapon.weaponType.ToString();
        descriptionText.text = weapon.description;
        spriteHolder.sprite = Resources.Load<Sprite>(weapon.spritePath);
        rarityText.text = weapon.rarity.ToString();

        switch (weapon.rarity)
        {
            case WeaponRarity.Common:
                rarityText.color = Color.black;
                break;
            case WeaponRarity.Rare:
                rarityText.color = Color.blue;
                break;
            case WeaponRarity.Epic:
                rarityText.color = new Color(0.6f, 0.2f, 1f); // Purple
                break;
            case WeaponRarity.Legendary:
                rarityText.color = Color.yellow;
                break;
            case WeaponRarity.Mystical:
                rarityText.color = Color.cyan;
                break;
        }
    }
}

public class UIRewardSelection : MonoBehaviour
{
    public WeaponController weaponController;   // Assign this in Inspector
    public List<RewardCardUI> rewardCards;      // List of UI slots (3 only)

    public Weapon Reward1;
    public Weapon Reward2;
    public Weapon Reward3;

    [Header("Option 4")]
    public TMP_Text howMuchGoldOffered;
    public int goldValue = 10;
    private WeaponScriptableObject ConvertToScriptableObject(Weapon weapon)
    {
        var so = ScriptableObject.CreateInstance<WeaponScriptableObject>();

        // Load assets from Resources
        Sprite loadedSprite = Resources.Load<Sprite>(weapon.spritePath);
        GameObject loadedPrefab = Resources.Load<GameObject>(weapon.prefabPath);

        if (loadedSprite == null && loadedPrefab == null)
            Debug.LogError($"Prefab not found at: Resources/{weapon.prefabPath}");

        so.Initialize(
            weapon.weaponName,
            weapon.weaponType,
            loadedSprite,
            loadedPrefab,
            weapon.damage,
            weapon.speed,
            weapon.cooldownDuration,
            weapon.pierce,
            weapon.description
        );

        return so;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PauseGame();
        howMuchGoldOffered.text = $"{goldValue}";
        GetWeapons();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetGoldResource()
    {
        Time.timeScale = 1f;
        UIManager.Instance.CloseAll();
        PlayerLevelUpStats.Instance.Gold += goldValue;
    }

    private void UpdateRewardUI()
    {
        if (rewardCards.Count < 3) return;

        rewardCards[0].Setup(Reward1);
        rewardCards[1].Setup(Reward2);
        rewardCards[2].Setup(Reward3);
    }

    public void GetWeapons()
    {
        var allWeapons = WeaponLoader.Instance.myWeaponList.weapons;

        if (allWeapons == null || allWeapons.Count < 3)
        {
            Debug.LogWarning("Not enough weapons in weapon list.");
            return;
        }

        List<Weapon> randomWeapons = new List<Weapon>();

        while (randomWeapons.Count < 3)
        {
            var candidate = allWeapons[Random.Range(0, allWeapons.Count)];
            if (!randomWeapons.Exists(w => w.weaponName == candidate.weaponName))
            {
                randomWeapons.Add(candidate);
            }
        }

        Reward1 = randomWeapons[0];
        Reward2 = randomWeapons[1];
        Reward3 = randomWeapons[2];

        UpdateRewardUI();
    }

    private void GiveWeapon(Weapon selectedWeapon)
    {
        if (weaponController == null)
            weaponController = FindObjectOfType<Player>().GetComponentInChildren<WeaponController>();


        if (selectedWeapon != null && weaponController != null)
        {
            weaponController.AddWeapon(selectedWeapon); // Add to player's inventory        
            Debug.Log($"Weapon '{selectedWeapon.weaponName}' added to weaponDataList.");
        }
        else
        {
            Debug.LogWarning("Failed to give weapon. Check for null references.");
        }

        Time.timeScale = 1f;
        UIManager.Instance.CloseAll();
    }

    public void Selection1() => GiveWeapon(Reward1);
    public void Selection2() => GiveWeapon(Reward2);
    public void Selection3() => GiveWeapon(Reward3);

    public void PlayerRecievesChosenWeapon(int index)
    {
        Weapon selected = index switch
        {
            0 => Reward1,
            1 => Reward2,
            2 => Reward3,
            _ => null
        };

        if (selected != null)
            GiveWeapon(selected);
    }

    public void PauseGame()
    {
        weaponController = FindObjectOfType<Player>().GetComponentInChildren<WeaponController>();
        Time.timeScale = 0f;
    }
}
