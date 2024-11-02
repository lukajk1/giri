using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button achievementsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;

    [Header("Pages")]
    [SerializeField] private GameObject playPage;
    [SerializeField] private GameObject settingsPage;

    private GameObject currentPage;

    private void Awake()
    {
        playButton.onClick.AddListener(() => SetPage("play"));
        settingsButton.onClick.AddListener(() => SetPage("settings"));
        quitButton.onClick.AddListener(Quit);
    }
    private void Start()
    {
        InitialSetup();
    }
    private void InitialSetup()
    {
        playPage.SetActive(false);
        settingsPage.SetActive(false);
        // .... add all pages here
    }

    private void SetPage(string page)
    {
        CloseCurrentPage();

        GameObject pageToSet = null;
        switch (page)
        {
            case "play":
                pageToSet = playPage;
                break;
            case "settings":
                pageToSet = settingsPage;
                break;
            default:
                Debug.LogError("somehow page could not be fetched");
                return;
        }

        currentPage = pageToSet;
        pageToSet.SetActive(true);
    }
    public void CloseCurrentPage()
    {
        PlayerPrefs.Save();
        if (currentPage != null)
        {
            currentPage.SetActive(false);
        }
        else
        {
            return;
        }
    }

    private void Quit()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }
}
