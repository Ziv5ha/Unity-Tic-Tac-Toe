using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SoundsBtnsScript : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Image MusicBtn;
    [SerializeField] private Sprite MusicOn;
    [SerializeField] private Sprite MusicOff;
    [SerializeField] private Image SoundEffectsBtn;
    [SerializeField] private Sprite SoundEffectsOn;
    [SerializeField] private Sprite SoundEffectsOff;

    // general confusion with the hierarchy of this class. is it telling someone else what to do? shouldn't it send events instead?
    public void ControlMusic()
    {
        if (audioManager.ControlMusic())
        {
            MusicBtn.sprite = MusicOn;
        }
        else
        {
            MusicBtn.sprite = MusicOff;
        }
    }
    public void ControlSoundEffects()
    {
        if (audioManager.ControlSoundEffects())
        {
            SoundEffectsBtn.sprite = SoundEffectsOn;
        }
        else
        {
            SoundEffectsBtn.sprite = SoundEffectsOff;
        }
    }
}
