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
    [SerializeField] private float rotationSpeen = 1;
    private bool clicked = false; 
    void Start()
    {
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
        return !clicked && !gameLogic.gameEnded;
    }
}
