using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UISystemConfig : UIWindow
{
    public Image musicOff;
    public Image soundOff;

    public Toggle toggleMusic;
    public Toggle toggleSound;

    public Slider sliderMusic;
    public Slider sliderSound;

    public Button close_Btn;
    private void Start()
    {
        musicOff.enabled = !Config.MusicOn;
        soundOff.enabled = !Config.SoundOn;
        this.toggleMusic.isOn = Config.MusicOn;
        this.toggleSound.isOn = Config.SoundOn;
        this.sliderMusic.value = Config.MusicVolume;
        this.sliderSound.value = Config.SoundVolume;
        close_Btn.onClick.AddListener(OnCloseClick);
        sliderMusic.onValueChanged.AddListener(MusicVolume);
        sliderSound.onValueChanged.AddListener(SoundVolume);
        toggleMusic.onValueChanged.AddListener(MusicToogle);
        toggleSound.onValueChanged.AddListener(SoundToogle);
    }

    public override void OnYesClick()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        PlayerPrefs.Save();
        base.OnYesClick();
    }

    public void MusicToogle(bool on)
    {
        musicOff.enabled = !on;
        Config.MusicOn = on;
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
    }

    public void SoundToogle(bool on)
    {
        soundOff.enabled = !on;
        Config.SoundOn = on;
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
    }

    public void MusicVolume(float vol)
    {
        Config.MusicVolume = (int)vol;
        Debug.LogFormat("MusicVolume:{0}", Config.MusicVolume);
        PlaySound();
    }

    public void SoundVolume(float vol)
    {
        Config.SoundVolume = (int)vol;
        Debug.LogFormat("SoundVolume:{0}", Config.SoundVolume);
        PlaySound();
    }

    float lastPlay = 0;
    private void PlaySound()
    {
        if (Time.realtimeSinceStartup - lastPlay > 0.1)
        {
            lastPlay = Time.realtimeSinceStartup;
            SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        }
    }
}
