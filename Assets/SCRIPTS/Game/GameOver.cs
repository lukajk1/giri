using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private TextMeshProUGUI gameOverMessage;
    [SerializeField] private TextMeshProUGUI enemiesKilled;
    [SerializeField] private TextMeshProUGUI heatNumber;
    [SerializeField] private TextMeshProUGUI damageDealt;
    [SerializeField] private TextMeshProUGUI damageTaken;
    [SerializeField] private TextMeshProUGUI activesUsed;
    
    void Start() {
        gameOverCanvas.SetActive(false);
    }

    public void GameEnd(bool isWin)
    {
        StartCoroutine(GameEndCR(isWin));
    }

    private IEnumerator GameEndCR(bool isWin) {
        GameState.Instance.MenusOpen++;
        yield return new WaitForSeconds(1);

        FindFirstObjectByType<OnDeathPostProcessing>().StartEffects();

        enemiesKilled.text = $"monster kills: {AddCommasToNumber(GameState.Instance.EnemiesKilled)}";
        heatNumber.text = $"heat reached: {GameState.Instance.HeatNumber}/5";
        damageDealt.text = $"total damage dealt: {AddCommasToNumber(GameState.Instance.Player.TotalDamageDealt)}";
        damageTaken.text = $"total damage taken: {AddCommasToNumber(GameState.Instance.Player.TotalDamageTaken)}";
        activesUsed.text = $"item actives used: {AddCommasToNumber(GameState.Instance.Player.TotalActivesUsed)}";

        if (isWin)
        {
            gameOverMessage.text = $"you win!";
        }
        else
        {
            gameOverMessage.text = $"you died!";
        }

        gameOverCanvas.SetActive(true);
    }
    private string AddCommasToNumber(double number)
    {
        return number.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
    }
    public void NewGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
