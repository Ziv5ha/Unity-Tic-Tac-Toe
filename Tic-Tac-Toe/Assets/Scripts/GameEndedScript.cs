using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// I don't like the name of this class. What does it do? Is it a view? Is it a controller? Why does it tell the ScoreManager what to do?
// In general: what is the position of this class in the hierarchy of the game?
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
        // See comment in EventsManager.cs
        EventsManager.OnPlayAgain?.Invoke();
    }
    public void ExitToMenu()
    {
        scoreManager.ResetScore();
        SceneManager.LoadScene(0);
    }
}
