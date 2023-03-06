using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    private Animator anim;
    private MeshRenderer meshRenderer;
    private GameLogic gameLogic;
    public int index;
    // !@! speeeeeen
    [SerializeField] private float rotationSpeen = 1;
    private bool clicked = false; 
    void Start()
    {
        // !@! See comments on GameLogic script. Generally speaking, a view should never tell a controller what to do directly. We will go more in depth on this.
        gameLogic = GameObject.FindGameObjectWithTag("Map").GetComponent<GameLogic>();
        anim = GetComponent<Animator>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnMouseEnter() {
        if (availableTile()){
            meshRenderer.material.color = new Color(0f, 0f, 0f, 0.10f);
        }
    }
    private void OnMouseExit() {
        meshRenderer.material.color =  new Color(255f, 255f, 255f, 0.01f);
    }
    private void OnMouseDown() {
        if (availableTile()){
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
    private bool availableTile(){
        //!@! This function might be more fitting in a controller class. Just call the function on gameLogic and it will know if the game is ended or not.
        return !clicked && !gameLogic.gameEnded;
    }
}
