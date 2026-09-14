using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JSONReader : SimpleSingleton<JSONReader>
{

    public TextAsset Enemy;
    [System.Serializable]
    public class EnemyClass
    {
        public string name;
        public int health;
        public int damageDealt;
        public int movementSpeed;
        public float firerate;
        public int weaponSwingSpeed;
        public string enemyPattern;
        public float bulletSize;
        public float bulletSpeed;
        public float bulletTime;

    }

    [System.Serializable]
    public class EnemyClassList
    {
        public EnemyClass[] enemyClasses;
    }

    public static EnemyClassList enemyClassList { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created\

    private void Awake()
    {
        base.Awake();
        enemyClassList = JsonUtility.FromJson<EnemyClassList>(Enemy.text);     
    }

    public EnemyClass GetEnemyDataByName(string enemyName)
    {
        foreach (EnemyClass enemy in enemyClassList.enemyClasses)
        {
            if (enemy.name == enemyName)
            {
                return enemy;
            }
        }
        return null;    

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
