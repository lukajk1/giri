using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    //private CameraController cameraPan;
    private KeybindMap keybindMap;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject inventoryPage;
    [SerializeField] private GameObject settingsPage;
    [SerializeField] private InventoryGrid inventoryGrid;
    [SerializeField] private GameObject leftNavigation;
    [SerializeField] private Image backdrop;

    [Header("buttons")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button quitToMenuButton;
    [SerializeField] private Button quitToDesktopButton;

    private bool inventoryOpenLast = true;
    private bool escMenuOpen = false;
    private GameState gameState;
    public static PauseMenu Instance;

    private BlurOnPause blurOnPause;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Debug.LogError($"Warning - more than one instance of {this} found. Additional occurring on {gameObject.name}");
    }
    void Start() {
        menu.SetActive(false);

        keybindMap = GameState.Instance.KeyMapInstance;
        gameState = GameState.Instance;

        continueButton.onClick.AddListener(ToggleEscMenu);
        quitToMenuButton.onClick.AddListener(QuitRun);
        quitToDesktopButton.onClick.AddListener(QuitToDesktop);

        blurOnPause = FindFirstObjectByType<BlurOnPause>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (GameState.Instance.MenusOpen == 0 || menu.activeSelf))
        {
            ToggleEscMenu();
        }
        else if (Input.GetKeyDown(keybindMap.KeyMap["show tab menu"]) || Input.GetKeyUp(keybindMap.KeyMap["show tab menu"]))
        {
            if (!escMenuOpen && gameState.MenusOpen == 0)
            {
                ToggleTabMenu();
            }

        }
    }

    private void ToggleTabMenu()
    {
        if (!menu.activeSelf) // opening
        {
            SetBackdropAlpha(0.4f);
            leftNavigation.SetActive(false);
            settingsPage.SetActive(false);
            inventoryPage.SetActive(true);
            inventoryGrid.GenerateInventoryGrid();
            menu.SetActive(true);
        }
        else // closing
        {
            menu.SetActive(false);
            SetBackdropAlpha(0.86f);
            leftNavigation.SetActive(true);
        }
        
    }

    private void ToggleEscMenu()
    {
        blurOnPause.SetBlur(!menu.activeSelf);

        if (!menu.activeSelf) // opening
        {
            GameState.Instance.MenusOpen++;
            if (inventoryOpenLast)
            {
                ShowInventory();
            }
            else
            {
                ShowSettings();
            }
            escMenuOpen = true;
            menu.SetActive(true);
        }
        else // closing
        {
            GameState.Instance.MenusOpen--;
            PlayerPrefs.Save();
            escMenuOpen = false;
            menu.SetActive(false);
        }
    }
    private void SetBackdropAlpha(float alpha)
    {
        Color color = backdrop.color;
        color.a = alpha;
        backdrop.color = color;
    }
    public void ShowSettings()
    {
        inventoryPage.SetActive(false);
        settingsPage.SetActive(true);
        inventoryOpenLast = false;
    }
    public void ShowInventory()
    {
        settingsPage.SetActive(false);
        inventoryPage.SetActive(true);
        inventoryGrid.GenerateInventoryGrid();
        inventoryOpenLast = true;
    }

    private void QuitRun()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene(0); //main menu
    }

    private void QuitToDesktop()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }
    void OnApplicationQuit() // triggers (hopefully but out of the control of me/unity, handled by the specifics of how the game is closed) when the application is closed
    {
        PlayerPrefs.Save();
    }

    void OnApplicationFocus(bool hasFocus) {
        if (!hasFocus && !menu.activeSelf && gameState.MenusOpen == 0) {
            ToggleEscMenu();
        }
    }
}
