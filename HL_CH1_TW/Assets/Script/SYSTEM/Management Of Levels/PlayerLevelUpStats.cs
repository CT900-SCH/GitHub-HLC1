using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelUpStats : MonoBehaviour
{
    public static PlayerLevelUpStats Instance;


    // Levels
    public int Level = 1;
    public float experience { get; private set; }
    public static float expNeeded;
    public static float previousExperience;

    // Gold
    public int Gold = 0;

    // Kills
    public int Kills = 0;

    // Spent
    public int Spent = 0;

    // Items
    public int Items = 0;


    private void Start()
    {
        Level = 1;
        experience = 0;
        Gold = 0;
        Kills = 0;
        Spent = 0;
        Items = 0;
    }

    private void Awake()
    {
        Instance = this; // Inserting this into the Static Pigeon hole.
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public static int ExpNeedToLvlUp(int currentLevel)
    {
        if (currentLevel == 0)
            return 0;
        return (currentLevel * currentLevel + currentLevel) * 5;
    }

    public void SetExperience(float expOrbsEarned)
    {
        experience += expOrbsEarned;
        Debug.Log(expOrbsEarned + " Exps Aquired.");

        expNeeded = ExpNeedToLvlUp(Level);
        previousExperience = ExpNeedToLvlUp(Level - 1);

        if(experience >= expNeeded)
        {
            LevelUp();
            expNeeded = ExpNeedToLvlUp(Level);
            previousExperience = ExpNeedToLvlUp(Level - 1);
        }
    }

    public void LevelUp()
    {
        Level++;
        LevelUpReward();
    }

    public void LevelUpReward()
    {
        Time.timeScale = 0f;
        // UIManager.Instance.OpenReplace(GameUIID.LevelUpSelection);
    }
}
