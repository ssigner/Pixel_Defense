using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AudioManager : DIMono
{

    [Serializable]
    public class AudioData
    {
        public AudioClip clip;
        public string name;
    }

    [Serializable]
    public class BGMData
    {
        public AssetReferenceT<AudioClip> bgmClip;
        public string name;
    }

    AudioSource bgmSource;
    public List<BGMData> bgmList;
    public List<AudioData> sfxList;
    public Dictionary<string, float> sfxTimeRecord;
    public const float sfxPlayTerm = 0.1f;
    public int maxAudioSourceCnt;

    public float bgmVolume;
    public float sfxVolume;


    List<AudioSource> audioSources;

    [Inject]
    SettingData settingData;
    protected override void Init()
    {
        base.Init();
        SetVolume(settingData);
    }

    public string GetAttackSFXName(Tower.TowerClass towerClass)
    {
        switch (towerClass)
        {
            case Tower.TowerClass.human:
                return "HumanAttack";
            case Tower.TowerClass.spirit:
                return "SpiritAttack";
            case Tower.TowerClass.not_human:
                return "NotHumanAttack";
        }
        return "";
    }

    public void SetVolume(SettingData settingData)
    {
        bgmVolume = settingData.bgmVolume;
        sfxVolume = settingData.sfxVolume;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if(DIContainer.Global.Has(typeof(AudioManager), "") == false)
        {
            DIContainer.Global.Regist(this);
        }
 
        bgmSource = new GameObject("BGM_Source").AddComponent<AudioSource>();
        bgmSource.transform.SetParent(transform);

        sfxTimeRecord = new Dictionary<string, float>();
        audioSources = new List<AudioSource>();

        int i;
        for(i=0;i< maxAudioSourceCnt; i++)
        {
            var newObj=new GameObject("AudioSource"+i);
            newObj.transform.parent = transform;
            var audioSource=newObj.AddComponent<AudioSource>();

            audioSources.Add(audioSource);
        }
    }

    int idx = 0;
    AudioSource GetAudio()
    {
        var audio = audioSources[idx];


        idx++;
        idx %= audioSources.Count;
        return audio;
    }

    public void PlayBGM(string name)
    {
        var bgm = bgmList.Find(I => I.name == name);
        if (bgm == null) return;

        if(bgmSource.clip != null)
        {
            var previousClip = bgmSource.clip;
            bgmSource.clip = null;
            Addressables.Release(previousClip);
        }

        var bgmClip = Addressables.LoadAssetAsync<AudioClip>(bgm.bgmClip).WaitForCompletion();
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.volume = bgmVolume;
        bgmSource.Play();
    }



    public void Play(string name)
    {
        if (sfxTimeRecord.ContainsKey(name))
        {
            if (Time.time - sfxTimeRecord[name] <= sfxPlayTerm) return;
        }

        
        var sfx = sfxList.Find(I => I.name == name);
        var currentTime = Time.time;
        sfxTimeRecord[name]= currentTime;

        var audioSource= GetAudio();
        var clip = sfx.clip;
        audioSource.volume = sfxVolume;
        audioSource.PlayOneShot(clip);
    }

    public void SetSFXVolume(float v)
    {
        sfxVolume = v;
    }

    public void SetBGMVolume(float v)
    {
        bgmVolume = v;
        bgmSource.volume = v;
    }
    //이전에 재생된 audio가 일정 시간 내에 똑같은 audio가 재생되려고 한다면
}
