using UnityEngine;
using System.Collections.Generic;



[System.Serializable]
public class Skill
{
    public string skillName;
    public string effectType;
    public int value;
    public string description;
}

[System.Serializable]
public class SkillList
{
    public List<Skill> skills;
}

public class SkillLoader : MonoBehaviour
{
    public SkillList mySkillList = new SkillList();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("skills");
        if (jsonFile != null)
        {
            mySkillList = JsonUtility.FromJson<SkillList>(jsonFile.text);
        }
        else
        {
            Debug.LogError("Could not find player.json in Resources folder.");
        }
    }
}

public enum AllEffectType
{
    IncreaseMaxHP,
    IncreaseMoveSpeed,
    IncreaseWeaponDamagePercent,
}
