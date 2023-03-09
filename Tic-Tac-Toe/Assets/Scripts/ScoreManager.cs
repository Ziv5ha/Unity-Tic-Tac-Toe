using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] ScoreViewScript scoreView;
    private int x;
    private int o;
    private int tie;

    private void Start()
    {
        x = PlayerPrefs.HasKey("x") ? PlayerPrefs.GetInt("x") : 0;
        o = PlayerPrefs.HasKey("o") ? PlayerPrefs.GetInt("o") : 0;
        tie = PlayerPrefs.HasKey("tie") ? PlayerPrefs.GetInt("tie") : 0;
        scoreView.UpdateScore(x, o, tie);

    }
    public void IncreaseScore(bool xTurn)
    {
        if (xTurn)
        {
            x++;
            PlayerPrefs.SetInt("x", x);
        }
        else
        {
            o++;
            PlayerPrefs.SetInt("o", o);
        }
        scoreView.UpdateScore(x, o, tie);
    }
    public void IncreaseTieScore()
    {
        tie++;
        PlayerPrefs.SetInt("tie", tie);
        scoreView.UpdateScore(x, o, tie);
    }
    public void ResetScore()
    {
        PlayerPrefs.SetInt("x", 0);
        PlayerPrefs.SetInt("o", 0);
        PlayerPrefs.SetInt("tie", 0);
        scoreView.UpdateScore(0, 0, 0);
    }
}

