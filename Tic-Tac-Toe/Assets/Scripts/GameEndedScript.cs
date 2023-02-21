using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameEndedScript : MonoBehaviour
{
    public TextMeshProUGUI winnerText;
    public void ShowWinScreen(string text){
        winnerText.text = text;
        gameObject.SetActive(true);
    }
    public void PlayAgain(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ExitToMenu(){
        SceneManager.LoadScene(0);
    }
}
