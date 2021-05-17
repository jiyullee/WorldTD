using System;
using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;

public class SoundData : DataSets<SoundData, SoundData.SoundDataClass>
{
    
    private Dictionary<int, AudioClip> dic_audioClip = new Dictionary<int, AudioClip>();
    public override void OnCreated()
    {
        base.OnCreated();
        fileName = ParsingDataSet.SoundData;
    }

    public override void OnInitiate()
    {
        base.OnInitiate();
        for (int i = 0; i < table.Count; i++)
        {
            dic_audioClip.Add(i, LoadAudioClip(i));
        }
    }

    public class SoundDataClass : DataClass
    {
        public string SoundType;
        public string SoundName;
        public string Path;
        public float Volume;
    }

    public AudioClip LoadAudioClip(int key)
    {
        string path = table[key].Path;
        return Resources.Load<AudioClip>(path);
    }
    
    public AudioClip GetAudioClip(int key)
    {
        return dic_audioClip[key];
    }
}
