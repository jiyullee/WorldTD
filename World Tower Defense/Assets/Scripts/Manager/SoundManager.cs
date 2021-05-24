using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
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
        Background.loop = true;
    }

    public override void OnInitiate()
    {
        //로비 배경 음악 재생
        PlaySound(SOUNDTYPE.BACKGROUND, 0);
    }

    #endregion

    #region Functions
    
    public void PlaySound(SOUNDTYPE p_type, int key)
    {
        AudioClip clip = SoundData.Instance.GetAudioClip(key);
        AudioSource playSource = null;
        if (p_type == SOUNDTYPE.BACKGROUND)
        {
            playSource = Background;
            playSource.clip = clip;
            playSource.Play();
        }
        else if(p_type == SOUNDTYPE.EFFECT)
        {
            playSource = Effect;
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
        if (p_type == SOUNDTYPE.BACKGROUND)
            playSource = Background;
        else if (p_type == SOUNDTYPE.EFFECT)
            playSource = Effect;

        playSource.volume = p_volume;
    }
    
    #endregion
}
