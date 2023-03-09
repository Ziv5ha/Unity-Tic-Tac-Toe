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
        // !@! OK I really don't like this. You are not changing between scenes, please do not use the scene loader to reset the scene unless it's a visual thing,
        // this a very heavy operation to call instead of just letting your logic handle resetting the game.
        // Also note that this is what is causing the null reference error when pressing play again over the tile.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ExitToMenu()
    {
        scoreManager.ResetScore();
        SceneManager.LoadScene(0);
    }
}
