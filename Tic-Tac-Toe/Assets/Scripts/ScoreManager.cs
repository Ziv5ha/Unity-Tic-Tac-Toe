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
        //!@! This is a bit of an overkill. We have PlayerPrefs in Unity that works out of the box. No need to use System.IO
        string scoreLocation = Application.persistentDataPath + "/GameScore.json";
        string scoreJson = System.IO.File.ReadAllText(scoreLocation);
        Score score = JsonUtility.FromJson<Score>(scoreJson);
        x = score.X;
        o = score.O;
        tie = score.Tie;
        //!@! Is this a controller or a view? Consider separating the two roles to two different classes.
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

    //!@! naming conventions! Not that important just keep note of this.
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
