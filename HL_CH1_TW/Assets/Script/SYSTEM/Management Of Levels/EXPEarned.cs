using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class EXPEarned : MonoBehaviour
{
    public static EXPEarned instance;

    [Header("EXP Points Earned: ")]
    public int EXPPoints;

    private void Awake()
    {
        instance = this; // Inserting this into the Static Pigeon hole.
    }

    private void OnDestroy()
    {
        instance = null;
    }

    private void Start()
    {
        EXPPoints = 0;
    }
}
