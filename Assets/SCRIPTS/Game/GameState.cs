using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static EventManager;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    [Header("managers")]
    public AudioManager Audio;
    public KeybindMap KeyMapInstance;
    [SerializeField] private InventoryManager inventoryManager; public InventoryManager InventoryManager => inventoryManager;
    //[SerializeField] private EventManager eventManager; public EventManager EventManager => eventManager;
    [SerializeField] private EnemySpawner enemySpawner; public EnemySpawner EnemySpawner => enemySpawner;

    [Header("player")]
    [SerializeField] private PlayerUnit player; public PlayerUnit Player => player;    
    [SerializeField] private GameObject playerRootObject; public GameObject PlayerRootObject => playerRootObject;
    [SerializeField] private Transform playerTransform; public Transform PlayerTransform => playerTransform;
    [SerializeField] private PlayerController playerController; public PlayerController PlayerController => playerController;


    private float timeScale = 1f; 
    public float TimeScale
    {
        get { return timeScale; }
        set
        {
            if (value < 0)
            {
                Debug.LogError("Error: Attempted to set a negative TimeScale. Value must be non-negative.");
                return;
            }
            timeScale = value;
        }
    }
    private int menusOpen = 0;
    [HideInInspector] public int MenusOpen
    {
        get { return menusOpen; }
        set
        {
            if (value < 0)
            {
                Debug.LogError("can't set negative value to menusopen");
                return;
            }
            else if (value == 0)
            {
                menusOpen = value;
                Time.timeScale = 1;
            }
            else
            {
                menusOpen = value;
                //Time.timeScale = 0;
            }
        }
    }

    private int _enemiesKilled = 0;  // This private field holds the actual value.
    [HideInInspector] public int EnemiesKilled
    {
        get { return _enemiesKilled; }  // Corrected to return the private field.
        set
        {
            // Validate the input to ensure it's not negative
            if (value < 0)
            {
                Debug.LogError("Attempt to set EnemiesKilled to a negative value; operation ignored.");
                return;  // Ignore the negative value and leave the current count unchanged
            }
            _enemiesKilled = value;  // Correctly assigns the value to the private field.
        }
    }

    [Header("stat text")]
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI heatNumberText;
    [SerializeField] private TextMeshProUGUI difficultyText;


    private GameData gameData;
    private int heatNumber = 1;
    private int tier = 1;
    [HideInInspector]
    public int HeatNumber
    {
        get { return heatNumber; }
        set
        {
            if (value < 0)
            {
                Debug.LogError("can't set negative value to heatnumber");
                return;
            }
            heatNumber = value;
        }
    }
    [HideInInspector] public int MaxEnemiesOnScreen { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }

        MaxEnemiesOnScreen = 13;
    }
    void Start()
    {
        Audio = AudioManager.Instance;
        EventManager.OnEnemyKilled += EnemyKilled;
        SetHeatNumber();

        gameData = GameData.Instance;
        tier = gameData.Tier;
        difficultyText.text = $"{tier}";
        MaxEnemiesOnScreen = tier; // just a placeholder calculation
        EnemiesKilled = 0;
    }
    void Update()
    {
        if (MenusOpen > 0 && Cursor.lockState != CursorLockMode.None)
        {
            SetCursorLocked(false);
            FindObjectOfType<CameraController>().SetPanEnabled(false);
            //Audio.PauseMusic(true);
        }
        else if (MenusOpen == 0 && Cursor.lockState != CursorLockMode.Confined)
        {
            SetCursorLocked(true);
            FindObjectOfType<CameraController>().SetPanEnabled(true);
            //Audio.PauseMusic(false);
        }
    }

    private void EnemyKilled(EnemyUnit enemy)
    {
        EnemiesKilled++;
        if (EnemiesKilled >= 100) 
        {
            FindFirstObjectByType<GameOver>().GameEnd(true);
        }
        killsText.text = $"{EnemiesKilled:000}";

        bool heatChanged = false;
        switch (EnemiesKilled)
        {
            case 24:
                HeatNumber = 2;
                heatChanged = true;
                break;
            case 50:
                HeatNumber = 3;
                heatChanged = true;
                break;
            case 99:
                HeatNumber = 4;
                heatChanged = true;
                break;
            default:
                break;
        }
        if (heatChanged) SetHeatNumber();
    }

    private void SetHeatNumber()
    {
        heatNumberText.text = $"{HeatNumber}/5";
    }

    public void SetCursorLocked(bool setLock)
    {
        Cursor.lockState = setLock ? CursorLockMode.Confined : CursorLockMode.None;
    }
}
