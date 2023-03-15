using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameEndedScript : MonoBehaviour
{
    public TextMeshProUGUI winnerText;
    public ScoreManager scoreManager;
    public void ShowWinScreen(string text)
    {
        winnerText.text = text;
        gameObject.SetActive(true);
    }
    public void PlayAgain()
    {
        gameObject.SetActive(false);
        EventsManager.OnPlayAgain?.Invoke();
    }
    public void ExitToMenu()
    {
        scoreManager.ResetScore();
        SceneManager.LoadScene(0);
    }
}
