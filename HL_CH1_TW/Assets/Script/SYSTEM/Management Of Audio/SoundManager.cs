using UnityEngine;
using System;


public enum ChapterType
{
    SideQuestsTheYoungEmperor
}

public enum SoundType
{
    EmperorHongBaJie_Dialogues,
    Chainnino_Dialogues,
    Morty
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.audioSource.PlayOneShot(randomClip, volume);
    }

    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);
        for(int i = 0; i < soundList.Length; i++)
            soundList[i].name = names[i];
    }
}

[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds { get => sounds; }
    [HideInInspector] public string name;
    public string chapter;
    //public DialogueLine[] dialogueLine;
    [SerializeField] private AudioClip[] sounds;
}

//[Serializable]
//public struct DialogueLine
//{
//    public string speakerName;

//    [TextArea(2, 5)]
//    public string line;

//    public AudioClip voiceClip;
//}

