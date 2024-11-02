using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : ADFM
{
    public static AudioManager Instance { get; private set; }
    private bool isPaused = false;
    private GameState gameState;
    private Dictionary<ADFM.Sfx, AudioData> audioDataDictionary = new Dictionary<ADFM.Sfx, AudioData>(); // contains name and the audiodata file to play a sound
    private float masterVolume;
    private float sfxVolume;
    private float musicVolume;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //#if !UNITY_EDITOR
        //    song.enabled = true;
        //#else
        //    song.enabled = false;
        //#endif
        song.enabled = true;

        InitializeAudioDataSources(); // must be done before dictionary is created
        InitializeAudioDataDictionary();
    }
    void Start() { 
        gameState = GameState.Instance;
    }

    void Update()
    {
        if (gameState.MenusOpen > 0 && !isPaused)
        {
            PauseGameSFX();
        }
        else if (gameState.MenusOpen == 0 && isPaused)
        {
            ResumeAllSFX();
        }
    }
    private void ResumeAllSFX()
    {
        foreach (AudioSource source in pausedSources)
        {
            source.Play();
        }
        pausedSources.Clear();
        isPaused = false;
    }

    private void PauseGameSFX()
    {
        
        pausedSources.Clear();
        foreach (AudioSource source in audioSourceList)
        {

            if (source.isPlaying && source != UI)
            {
                source.Pause();
                pausedSources.Add(source);
            }
        }
        isPaused = true;
    }

    protected void InitializeAudioDataDictionary()
    {
        // Add all audio data to a dictionary for quick lookup
        foreach (AudioData audioData in audioDataList)
        {
            audioDataDictionary[audioData._Sfx] = audioData;
        }
    }

    private float minPitch = 0.87f;
    private float maxPitch = 1.09f;
    public void PlaySound(ADFM.Sfx soundEffect)
    {
        if (audioDataDictionary.TryGetValue(soundEffect, out AudioData sfx))
        {
            sfx._AudioSource.clip = sfx._AudioClip;
            if (sfx._AudioSource != UI)
            {
                sfx._AudioSource.pitch = Random.Range(minPitch, maxPitch);
            }
            sfx._AudioSource.Play();
        }
        else
        {
            Debug.LogError($"Audio clip {soundEffect} was not found");
        }
    }

    public void SetMusicVolume(float volume)
    {
        float clampedVol = Mathf.Clamp(volume, 0, 1);
        musicVolume = clampedVol;
        song.volume = clampedVol * masterVolume;
    }    
    public void SetMusicVolume()
    {
        song.volume = musicVolume * masterVolume;
    }

    public void SetSFXVolume(float volume)
    {
        float clampedVol = Mathf.Clamp(volume, 0, 1);
        sfxVolume = clampedVol;
        foreach (var source in audioSourceList)
        {
            source.volume = clampedVol * masterVolume;
        }
    }    
    public void SetSFXVolume()
    {
        foreach (var source in audioSourceList)
        {
            source.volume = sfxVolume * masterVolume;
        }
    }
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp(volume, 0, 1);  
        SetSFXVolume();
        SetMusicVolume();
    }

    public void PauseMusic(bool paused)
    {
        if (song.enabled)
        {
            if (paused)
            {
                song.Pause();
            }
            else
            {
                song.Play();
            }
        }
    }
}
