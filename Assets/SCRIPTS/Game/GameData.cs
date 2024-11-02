using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;
    //[SerializeField] public AudioManager _audio; public AudioManager Audio => _audio;

    [HideInInspector] 
    private int tier = 1; // Default value
    public int Tier
    {
        get => tier;
        set
        {
            if (value < 1 || value > TIER_MAX)
            {
                Debug.LogError($"Attempted to set DifficultyLevel outside of valid range: {value}");
            }
            else
            {
                tier = value;
                PlayerPrefs.SetInt("DifficultyLevel", value);
                PlayerPrefs.Save();
            }
        }
    }

    public const int TIER_MAX = 9;

    [HideInInspector] public string SelectedCharacterCmdName { get; set; }
    //[HideInInspector] public IBoon SelectedBoon { get; set; } // (interface does not exist yet) 

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

        if (PlayerPrefs.HasKey("DifficultyLevel"))
        {
            Tier = PlayerPrefs.GetInt("DifficultyLevel");
        }
    }

    public string GetTierMessage()
    {
        string difficultyMessage = string.Empty;

        switch (Tier)
        {
            case 1:
                break;
            case 2:
                difficultyMessage = "enemies have 20% more health.";
                break;
            case 3:
                difficultyMessage = "enemies have 20% reduced cooldowns.";
                break;
            case 4:
                difficultyMessage = "enemies do 20% more damage. enemy cap increased by 15%.";
                break;
            case 5:
                difficultyMessage = "enemies do 20% more damage. enemy cap increased by 15%.";
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            default: 
                break;
        }
        if (Tier > 2)
        {
            difficultyMessage += " (+ previous buffs to enemies)";
        }

        return difficultyMessage;
    }

}
