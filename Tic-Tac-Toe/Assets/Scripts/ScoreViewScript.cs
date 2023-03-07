using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreViewScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI XUI;
    [SerializeField] TextMeshProUGUI TieUI;
    [SerializeField] TextMeshProUGUI OUI;
    
    public void UpdateScore(int x, int o, int tie){
        XUI.text = x.ToString();
        OUI.text = o.ToString();
        TieUI.text = tie.ToString();
    }
}
