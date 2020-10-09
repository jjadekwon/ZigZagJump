using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;   // Enum
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{

    static Dictionary<BgmType, AudioClip> bgmClipDic = new Dictionary<BgmType, AudioClip>();
    static Dictionary<SfxType, AudioClip> sfxClipDic = new Dictionary<SfxType, AudioClip>();

    static AudioSource bgmSource;
    static AudioSource sfxSource;

    // Scene이 로드되기 전에 한번 호출됨
    // Awake에서 null 체크 - 씬이 여러개일 경우 씬마다 존재해야 함
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        GameObject obj = new GameObject("SoundManager");
        obj.AddComponent<SoundManager>();
        DontDestroyOnLoad(obj);

        GameObject bgmObj = new GameObject("BGM");
        bgmSource = bgmObj.AddComponent<AudioSource>();
        bgmSource.playOnAwake = false;
        bgmSource.loop = true;
        bgmSource.volume = PlayerPrefs.GetFloat("BGMVolume", 1);
        bgmObj.transform.SetParent(obj.transform);

        GameObject sfxObj = new GameObject("SFX");
        sfxSource = sfxObj.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1);
        sfxObj.transform.SetParent(obj.transform);

        // Resource 하위의 Sound들 저장
        AudioClip[] bgmClips = Resources.LoadAll<AudioClip>("Sounds/BGM");
        for (int i = 0; i < bgmClips.Length; i++)
        {
            try
            {
                BgmType bgmType = (BgmType) Enum.Parse( typeof(BgmType), bgmClips[i].name);
                bgmClipDic.Add(bgmType, bgmClips[i]);
            }
            catch
            {
                Debug.LogError("Need BgmType enum :" + bgmClips[i].name);
            }
        }

        AudioClip[] sfxClips = Resources.LoadAll<AudioClip>("Sounds/SFX");
        for (int i = 0; i < sfxClips.Length; i++)
        {
            try
            {
                SfxType sfxType = (SfxType) Enum.Parse( typeof(SfxType), sfxClips[i].name);
                sfxClipDic.Add(sfxType, sfxClips[i]);
            }
            catch
            {
                Debug.LogError("Need SfxType enum :" + sfxClips[i].name);
            }
        }

        SceneManager.sceneLoaded += OnSceneLoadComplete;
    }

    static void OnSceneLoadComplete (Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Game":
            case "Title":
                PlayBGM(BgmType.Game, 0.5f);
                break;
            }
    }

    public static void PlaySFX (SfxType type)
    {
        sfxSource.PlayOneShot(sfxClipDic[type]);
    }

    public static void PlayBGM (BgmType type, float fadeTime = 0)
    {
        if (bgmSource.clip != null)
        {
            if (bgmSource.clip.name == type.ToString()) return;

            if (fadeTime > 0)
            {
                bgmSource.DOFade(0, fadeTime).OnComplete(() =>
                {
                    bgmSource.clip = bgmClipDic[type];
                    bgmSource.Play();
                    bgmSource.DOFade(PlayerPrefs.GetFloat("BGMVolume", 1), fadeTime);
                });
            }
            else
            {
                bgmSource.clip = bgmClipDic[type];
                bgmSource.Play();
            }
        }
        else
        {
            bgmSource.clip = bgmClipDic[type];

            if (fadeTime > 0)
            {
                bgmSource.volume = 0;
                bgmSource.DOFade(PlayerPrefs.GetFloat("BGMVolume", 1), fadeTime);
            }
            else
            {
                bgmSource.volume = PlayerPrefs.GetFloat("BGMVolume", 1);
            }
            bgmSource.Play();
        }
    }
    
    public static void SetBGMVolume (float volume)
    {
        bgmSource.volume = volume;
    }

    public static void SetSFXVolume (float volume)
    {
        sfxSource.volume = volume;
    }
    
}

public enum BgmType
{
    Game
}

public enum SfxType
{
    Jump,
    Turn,
    Click,
    Item,
    Die
}
