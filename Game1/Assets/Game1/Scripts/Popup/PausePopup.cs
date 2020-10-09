using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PausePopup : BasePopup
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    public override void Init(int id = -1) 
    {
        base.Init();
        Time.timeScale = 0f;

        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1);
    }

    public void OnBgmVolumeChange ()
    {
        SoundManager.SetBGMVolume(bgmSlider.value);
        PlayerPrefs.SetFloat("BGMVolume", bgmSlider.value);
    }

    public void OnSFXVolumeChange ()
    {
        SoundManager.SetSFXVolume(sfxSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
    }

    public override void Close()
    {
        base.Close();
        Time.timeScale = SpeedManager.instance.speed;
    }
}
