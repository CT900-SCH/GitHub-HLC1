using UnityEngine;
using System;



public enum CharacterPosition
{
    Left,
    Right,
    Center,
}

[Serializable]
public struct DialogueLine
{
    public string speakerName;

    [TextArea(2, 5)]
    public string line;

    public AudioClip voiceClip;


    [Header("Manual Sprite Assignment")]
    public Sprite leftSprite;
    public Sprite rightSprite;
    public Sprite centerSprite;


    [Header("Image Opacity Per Slide")]
    [Range(0f, 1f)]
    public float leftImageOpacity;

    [Range(0f, 1f)]
    public float rightImageOpacity;

    [Range(0f, 1f)]
    public float centerImageOpacity;
}
