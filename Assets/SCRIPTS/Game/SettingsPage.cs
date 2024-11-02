using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsPage : MonoBehaviour
{
    private const float DEFAULT_AUDIO_VOLUME = 0.5f;
    private const int DEFAULT_RES = 3;
    private const int DEFAULT_FULLSCREEN = 0;
    private const int DEFAULT_CAMERA_LOCKED = 0;

    //private KeybindMap keybindMap;

    [Header("Settings Objects")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle cameraLockToggle;

    [SerializeField] private TMP_Dropdown screenResDropdown;
    
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider sfxVolume;
    [SerializeField] private Slider musicVolume;
    void Start()
    {
        //Debug.Log(PlayerPrefs.GetInt("CameraLocked"));
        //Debug.Log(PlayerPrefs.GetInt("Fullscreen"));
        //Debug.Log(PlayerPrefs.GetInt("ScreenRes"));
        //Debug.Log(PlayerPrefs.GetFloat("MasterVolume"));
        //Debug.Log(PlayerPrefs.GetFloat("SFXVolume"));
        //Debug.Log(PlayerPrefs.GetFloat("MusicVolume"));

        SetValuesFromPrefs();

        sfxVolume.onValueChanged.AddListener(SetSFXVolume);
        musicVolume.onValueChanged.AddListener(SetMusicVolume);
        masterVolume.onValueChanged.AddListener(SetMasterVolume);

        fullscreenToggle.onValueChanged.AddListener(SetFullscreenValue);
        cameraLockToggle.onValueChanged.AddListener(SetCameraLockedValue);
        screenResDropdown.onValueChanged.AddListener(SetScreenRes);

        //masterVolume.value = 0;
    }
    private void SetValuesFromPrefs()
    {
        bool isOn;

        if (PlayerPrefs.HasKey("CameraLocked"))
        {
            isOn = PlayerPrefs.GetInt("CameraLocked") == 0 ? false : true;
        }
        else
        {
            isOn = DEFAULT_CAMERA_LOCKED == 0 ? false : true;
        }
        SetCameraLockedValue(isOn); // set the actual in-game setting which updates playerprefs
        cameraLockToggle.isOn = isOn; // set the ui component to reflect value

        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            isOn = PlayerPrefs.GetInt("Fullscreen") == 0 ? false : true;
        }
        else
        {
            isOn = DEFAULT_FULLSCREEN == 0 ? false : true;
        }
        SetFullscreenValue(isOn);
        fullscreenToggle.isOn = isOn;

        int res;
        if (PlayerPrefs.HasKey("ScreenRes"))
        {
            res = PlayerPrefs.GetInt("ScreenRes");
        }
        else
        {
            res = DEFAULT_RES;
        }
        SetScreenRes(res);
        screenResDropdown.value = res;


        float volume;

        volume = GetVolumeFromPrefs("MasterVolume");
        SetMasterVolume(volume);
        masterVolume.value = volume;

        volume = GetVolumeFromPrefs("SFXVolume");
        SetSFXVolume(volume);
        masterVolume.value = volume;

        volume = GetVolumeFromPrefs("MusicVolume");
        SetMusicVolume(volume);
        masterVolume.value = volume;

        PlayerPrefs.Save();
    }
    private float GetVolumeFromPrefs(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetFloat(key);
        }
        else
        {
            return DEFAULT_AUDIO_VOLUME;
        }
    }
    private void SetSFXVolume(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
    private void SetMusicVolume(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
    private void SetMasterVolume(float value)
    {
        AudioManager.Instance.SetMasterVolume(value);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    private void SetFullscreenValue(bool isOn)
    {
        Screen.fullScreen = isOn;
        PlayerPrefs.SetInt("Fullscreen", isOn ? 1 : 0);
    }
    public void SetCameraLockedValue(bool isOn) 
    {
        if (FindObjectOfType<CameraController>() != null) {
            FindObjectOfType<CameraController>().CameraLocked = isOn;
        }
        cameraLockToggle.isOn = isOn;
        PlayerPrefs.SetInt("CameraLocked", isOn ? 1 : 0);
    }
    public void SetScreenRes(int value)
    {
        switch (value)
        {
            case 0:
                Screen.SetResolution(2560, 1440, fullscreenToggle.isOn);
                break;
            case 1:
                Screen.SetResolution(1920, 1080, fullscreenToggle.isOn);
                break;
            case 2:
                Screen.SetResolution(1600, 900, fullscreenToggle.isOn);
                break;
            case 3:
                Screen.SetResolution(1280, 720, fullscreenToggle.isOn);
                break;
            default:
                break;
        }
        PlayerPrefs.SetInt("ScreenRes", value);
        screenResDropdown.value = value;
    }
    void OnApplicationQuit() // triggers (hopefully but out of the control of me/unity, handled by the specifics of how the game is closed) when the application is closed
    {
        PlayerPrefs.Save();
    }

}