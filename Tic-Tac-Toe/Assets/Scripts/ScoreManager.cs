using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI XUI;
    [SerializeField] TextMeshProUGUI TieUI;
    [SerializeField] TextMeshProUGUI OUI;
    private int x;
    private int o;
    private int tie;

    private void Start() {
        string scoreLocation = Application.persistentDataPath + "/GameScore.json";
        string scoreJson = System.IO.File.ReadAllText(scoreLocation);
        Score score = JsonUtility.FromJson<Score>(scoreJson);
        x = score.X;
        o = score.O;
        tie = score.Tie;
        updateScoreOnUI();

    }
    public void IncreaseScore(bool xTurn){
        if (xTurn){
            x++;
        } else {
            o++;
        }
        SaveIntoJson();
    }
    public void IncreaseTieScore(){
        tie++;
        SaveIntoJson();
    }
    public void ResetScore(){
        x = 0;
        o = 0;
        tie = 0;
        SaveIntoJson();
    }

    private void updateScoreOnUI(){
        XUI.text = x.ToString();
        OUI.text = o.ToString();
        TieUI.text = tie.ToString();
    }

    private void SaveIntoJson(){
        updateScoreOnUI();
        string score = JsonUtility.ToJson(new Score(x, o, tie)); 
        System.IO.File.WriteAllText(Application.persistentDataPath + "/GameScore.json", score);
    }
}

public class Score{
    public int X;
    public int O;
    public int Tie;

    public Score(int x, int o, int tie)
    {
        X = x;
        O = o;
        Tie = tie;
    }
}
