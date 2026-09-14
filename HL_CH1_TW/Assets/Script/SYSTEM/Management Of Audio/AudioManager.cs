using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : SimpleSingleton<AudioManager>
{
    public AudioSource BGMsource;
    public AudioMixerGroup BGMchannel;
    public AudioSource UISource;
    public AudioMixerGroup UIChannel;
    public SFXData[] Sounds;

    Dictionary<SoundID, SFXData> _sound = new Dictionary<SoundID, SFXData>();

    protected override void Awake()
    {
        base.Awake();

        foreach (var sfx in Sounds)
        {
            _sound[sfx.ID] = sfx;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SFXSound(SoundID id, Vector3? position = null, float volume = 1f)
    {
        if (_sound.TryGetValue(id, out SFXData data))
        {
            // New Version.
            AudioSource SoundSource = Instantiate(UISource, transform.position, Quaternion.identity);
            SoundSource.outputAudioMixerGroup = UIChannel;
            int randomness = Random.Range(0, data.Clips.Length);
            AudioClip clipness = data.Clips[randomness];
            if (position != null && position.HasValue)
            {
                AudioSource.PlayClipAtPoint(clipness, position.Value);
            }
            else // Basically going to here.
            {
                SoundSource.PlayOneShot(clipness);
                Destroy(SoundSource.gameObject, clipness.length + .1f);
            }
        }
    }

    public void BGMSound(SoundID id, Vector3? position = null, float volume = 1f)
    {
        if (_sound.TryGetValue(id, out SFXData data))
        {
            // New Version.
            AudioSource SoundSource = Instantiate(BGMsource, transform.position, Quaternion.identity);
            SoundSource.outputAudioMixerGroup = BGMchannel;
            int randomness = Random.Range(0, data.Clips.Length);
            AudioClip clipness = data.Clips[randomness];
            if (position != null && position.HasValue)
            {
                AudioSource.PlayClipAtPoint(clipness, position.Value);
            }
            else // Basically going to here.
            {
                SoundSource.PlayOneShot(clipness);
                Destroy(SoundSource.gameObject, clipness.length + .1f);
            }
        }
    }
}

public enum SoundID
{
    ButtonClick,
    Confirm,
    Cancel,
    VictorySound,
    DefeatSound,
}

[System.Serializable]
public class SFXData
{
    public SoundID ID;
    public AudioClip[] Clips;
}
