using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public bool CanPlayMusic = true;
    public bool CanPlaySoundEffects = true;
    public Sound[] XSounds;
    public Sound[] OSounds;
    public Sound[] WinSounds;
    public Sound[] TieSounds;

    void Awake()
    {


        foreach (Sound xs in XSounds)
        {
            xs.source = gameObject.AddComponent<AudioSource>();
            xs.source.clip = xs.clip;
            xs.source.volume = xs.volume;
            xs.source.pitch = xs.pitch;
        }
        foreach (Sound os in OSounds)
        {
            os.source = gameObject.AddComponent<AudioSource>();
            os.source.clip = os.clip;
            os.source.volume = os.volume;
            os.source.pitch = os.pitch;
        }
        foreach (Sound ws in WinSounds)
        {
            ws.source = gameObject.AddComponent<AudioSource>();
            ws.source.clip = ws.clip;
            ws.source.volume = ws.volume;
            ws.source.pitch = ws.pitch;
        }
        foreach (Sound ts in TieSounds)
        {
            ts.source = gameObject.AddComponent<AudioSource>();
            ts.source.clip = ts.clip;
            ts.source.volume = ts.volume;
            ts.source.pitch = ts.pitch;
        }

    }

    public void PlayXOSound(bool XTurn)
    {
        if (CanPlaySoundEffects)
        {
            if (XTurn)
            {
                PlaySound(XSounds);
            }
            else
            {
                PlaySound(OSounds);
            }
        }
    }
    private void PlaySound(Sound[] soundsArr)
    {
        int i = Random.Range(0, soundsArr.GetLength(0));
        // soundsArr[i].source.volume = soundsArr[i].volume * MasterVolume;
        soundsArr[i].source.Play();
    }
    public void PlayWinSound()
    {
        if (CanPlayMusic)
        {
            PlaySound(WinSounds);
        }
    }
    public void PlayTieSound()
    {
        if (CanPlayMusic)
        {
            PlaySound(TieSounds);
        }
    }

    public bool ControlMusic()
    {
        CanPlayMusic = !CanPlayMusic;
        Debug.Log("Music: " + CanPlayMusic);
        return CanPlayMusic;
    }
    public bool ControlSoundEffects()
    {
        CanPlaySoundEffects = !CanPlaySoundEffects;
        Debug.Log("SoundEffects: " + CanPlaySoundEffects);
        return CanPlaySoundEffects;
    }
}
