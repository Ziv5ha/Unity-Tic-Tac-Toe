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
    // Start is called before the first frame update
    void Start()
    {
        gameLogic = GameObject.FindGameObjectWithTag("Map").GetComponent<GameLogic>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
