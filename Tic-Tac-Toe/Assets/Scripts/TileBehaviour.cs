using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    private Animator anim;
    private GameLogic gameLogic;
    public int index;
    [SerializeField] private float rotationSpeen = 1;
    private bool clicked = false; 
    void Start()
    {
        gameLogic = GameObject.FindGameObjectWithTag("Map").GetComponent<GameLogic>();
        anim = GetComponent<Animator>();
    }

    private void OnMouseDown() {
        if (!clicked && !gameLogic.gameEnded){
            RotateTile();
            gameLogic.PlayTurn(index);
            clicked = true;
        }
    }
    private void RotateTile(){
        float singleStep = rotationSpeen * Time.deltaTime;
        if (gameLogic.xTurn){
            anim.SetTrigger("x");
        } else {
            anim.SetTrigger("o");
        }
    }
}
