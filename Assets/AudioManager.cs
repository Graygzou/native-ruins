using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Manager {

    #region Serialize Fields
    [SerializeField]
    private AudioMixer masterMixer;
    [SerializeField]
    private AudioMixerSnapshot paused;
    [SerializeField]
    private AudioMixerSnapshot unpaused;
    #endregion

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
