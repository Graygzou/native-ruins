using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour, IManager {

    #region Serialize Fields
    [SerializeField]
    private AudioMixer masterMixer;
    [SerializeField]
    private AudioMixerSnapshot paused;
    [SerializeField]
    private AudioMixerSnapshot unpaused;
    [SerializeField]
    private AudioMixerSnapshot volumeDown;

    [SerializeField]
    private float fadeUpTime = 0.01f;
    [SerializeField]
    private float fadeDownTime = 0.05f;
    [SerializeField]
    private float fadeTimePause = 0.01f;
    #endregion

    public void Init()
    {
        FadeUp();
    }

    public void InitMainScene()
    {
        FadeUp();
    }

    public void FadeUp()
    {
        unpaused.TransitionTo(fadeUpTime);
    }

    public void FadeDown()
    {
        volumeDown.TransitionTo(fadeDownTime);
    }

    public void FadePause()
    {
        paused.TransitionTo(fadeTimePause);
    }

    private bool isPaused = false;

    public void SetMasterSounds(float masterLevel)
    {
        masterMixer.SetFloat("MasterVolume", masterLevel);
    }

    public void SetSoundsEffectsLevel(float sfxLevel)
    {
        masterMixer.SetFloat("SFXVolume", sfxLevel);
    }

    public void setMusicLevel(float musicLevel)
    {
        masterMixer.SetFloat("MusicsVolume", musicLevel);
    }

    public void PauseGame()
    {
        isPaused = Time.timeScale == 0;
        Time.timeScale = isPaused ? 1.0f : 0.0f;
        
        if (isPaused)
        {
            paused.TransitionTo(0.1f);
        }
        else
        {
            unpaused.TransitionTo(0.1f);
        }
    }
}
