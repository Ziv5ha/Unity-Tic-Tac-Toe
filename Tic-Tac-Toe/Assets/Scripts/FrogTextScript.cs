using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//!@! Now this is a perfect class!
public class FrogTextScript : MonoBehaviour
{
    public TextMeshProUGUI frogText;

    private string[] winTextBank = {
        "Ninja Frog congratulates you on your win.",
        "Ninja Frog congratulates you on a great game.",
        "That was a close One!",
        "Great game everyone.",
        "Best game I've seen so far.",
        "HOORAY! YOU WIN!",
        "HA HA! YOU LOSE!",
        "Fantastic game.",
        "Great effort.",
        "That was fast.",
        "That was quick.",
        "Rematch?"
        };
    private string[] tieTextBank = {
        "Ninja Frog congratulates you on a great game.",
        "That was a close One!",
        "Great game everyone.",
        "Maybe the real win is the friends we made along the way.",
        "BOOO! I 'm here to see you win!",
        "Fantastic game.",
        "Great effort.",
        "This game went nowhere.",
        "Worst game I have ever seen.",
        "Rematch?"
    };


    public void Win()
    {
        string randomText = winTextBank[Random.Range(0, winTextBank.Length - 1)];
        frogText.text = randomText;
    }
    public void Tie()
    {
        string randomText = tieTextBank[Random.Range(0, tieTextBank.Length - 1)];
        frogText.text = randomText;
    }

}
