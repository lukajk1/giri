using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayPageLogic : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button incrementDifficultyButton;
    [SerializeField] private Button decrementDifficultyButton;
    [SerializeField] private Button backButton;

    [SerializeField] private TextMeshProUGUI tierLabelText;
    [SerializeField] private TextMeshProUGUI tierMessageText;

    void Start()
    {
        AdjustDifficulty(0); // this sets the text to the current difficulty level as stored in GameData
        startGameButton.onClick.AddListener(StartGame);
        incrementDifficultyButton.onClick.AddListener(() => AdjustDifficulty(1));
        decrementDifficultyButton.onClick.AddListener(() => AdjustDifficulty(-1));
        backButton.onClick.AddListener(ClosePlayPage);
    }

    private void AdjustDifficulty(int increment)
    {
        int resultantTier = GameData.Instance.Tier += increment;

        if (resultantTier > 0 && resultantTier <= GameData.TIER_MAX) // still needs validation because setting to properties can't return a value... 
        {
            tierLabelText.text = $"tier {resultantTier}";
            GameData.Instance.Tier = resultantTier;
            tierMessageText.text = GameData.Instance.GetTierMessage();
        }
    }
    private void ClosePlayPage()
    {
        FindObjectOfType<MainMenuManager>().CloseCurrentPage();
    }


    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
