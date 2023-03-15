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
        // I created actions to play a turn and restart the game but I still need to access the gameLogic here to know if a tile is available and to know which way to rotate the tile.
        gameLogic = GameObject.FindGameObjectWithTag("Map").GetComponent<GameLogic>();
        anim = GetComponent<Animator>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnMouseEnter()
    {
        if (gameLogic.AvailableTile(clicked))
        {
            meshRenderer.material.color = new Color(0f, 0f, 0f, 0.10f);
        }
    }
    private void OnMouseExit()
    {
        meshRenderer.material.color = new Color(255f, 255f, 255f, 0.01f);
    }
    private void OnMouseDown()
    {
        if (gameLogic.AvailableTile(clicked))
        {
            RotateTile();
            EventsManager.OnPlayTurn?.Invoke(index);
            clicked = true;
        }
    }
    private void RotateTile()
    {
        float singleStep = rotationSpeen * Time.deltaTime;
        if (gameLogic.xTurn)
        {
            anim.SetTrigger("x");
        }
        else
        {
            anim.SetTrigger("o");
        }
    }

}
