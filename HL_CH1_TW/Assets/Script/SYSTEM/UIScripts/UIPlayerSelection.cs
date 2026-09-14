using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterCardUI
{
    public string NameOfClass = "Put Class Name Here";
    public TMP_Text nameText;
    public TMP_Text hpText;
    public TMP_Text speedText;
    public TMP_Text descriptionText;
}

[System.Serializable]
public class WeaponSet
{
    public List<Weapon> weapons;
}


public class UIPlayerSelection : MonoBehaviour
{
    [Header("Character Select")]
    public List<CharacterCardUI> characterCards; // Assign 3 elements in the inspector

    ///test
    public Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PauseGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        UIManager.Instance.CloseAll();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;

        var classList = CharacterLoader.Instance.myClassList;

        for (int i = 0; i < characterCards.Count; i++)
        {
            if (i < classList.classes.Count)
            {
                var data = classList.classes[i];
                var ui = characterCards[i];

                ui.nameText.text = data.className;
                ui.hpText.text = data.MaxHP.ToString();
                ui.speedText.text = data.moveSpeed.ToString();
                ui.descriptionText.text = data.description;
            }
        }
    }

    public void ChosenWarriorClass() => ChooseClass(0);
    public void ChosenRangerClass() => ChooseClass(1);
    public void ChosenMageClass() => ChooseClass(2);

    private void ChooseClass(int index)
    {
        // Assign the stats based on respective class
        var characterData = CharacterLoader.Instance.myClassList.classes[index];

        ///Test
        Player.Instance.ID = characterData.ID;
        // Update Player's New class
        Player.Instance.MaxHP = characterData.MaxHP;
        Player.Instance.HP = Player.Instance.MaxHP;
        Player.Instance.moveSpeed = characterData.moveSpeed;

        Time.timeScale = 1f;

        WeaponController weaponCtrl = Player.Instance.GetComponentInChildren<WeaponController>();
        if (weaponCtrl != null)
        {
            var primaryWeaponData = WeaponLoader.Instance.myWeaponList.weapons[characterData.starterWeaponPrimary];
            var secondaryWeaponData = WeaponLoader.Instance.myWeaponList.weapons[characterData.starterWeaponSecondary];
            weaponCtrl.AddWeapon(primaryWeaponData);
            weaponCtrl.AddWeapon(secondaryWeaponData);
        }
        else
            Debug.LogWarning("WeaponController not found on Player!");

        UIManager.Instance.CloseAll();

        // Do something with index, like pass it to GameManager or store selected class
    }
}
