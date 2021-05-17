using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : UnitySingleton<SoundManager>
{
    #region Fields

    private AudioSource Background;
    private AudioSource Effect;
    private float volume_weight = 1;
    #endregion
    
    #region CallBacks

    public override void OnCreated()
    {
        DontDestroyOnLoad(gameObject);
        Background = transform.Find("Background").GetComponent<AudioSource>();
        Effect = transform.Find("Effect").GetComponent<AudioSource>();
    }

    public override void OnInitiate()
    {
        
    }

    #endregion

    #region Functions
    
    public void PlaySound(SOUNDTYPE p_type, int key)
    {
        AudioClip clip = SoundData.Instance.GetAudioClip(key);
        float volume = SoundData.Instance.GetTableData(key).Volume;

        AudioSource playSource = null;
        if (p_type == SOUNDTYPE.BACKGROUND)
        {
            playSource = Background;
            playSource.clip = clip;
            playSource.volume = volume * volume_weight;
            playSource.Play();
        }
        else if(p_type == SOUNDTYPE.EFFECT)
        {
            playSource = Effect;
            playSource.volume = volume * volume_weight;
            playSource.PlayOneShot(clip);
        }

    }

    public void StopSound(SOUNDTYPE p_type)
    {
        AudioSource playSource = null;
        if (p_type == SOUNDTYPE.BACKGROUND)
            playSource = Background;
        else if (p_type == SOUNDTYPE.EFFECT)
            playSource = Effect;
        
        playSource.Stop();
    }

    public void ControlVolume(SOUNDTYPE p_type, float p_volume)
    {
        AudioSource playSource = null;
        volume_weight = p_volume;
        if (p_type == SOUNDTYPE.BACKGROUND)
            playSource = Background;
        else if (p_type == SOUNDTYPE.EFFECT)
            playSource = Effect;

        playSource.volume *= volume_weight;
    }
    
    #endregion
}
