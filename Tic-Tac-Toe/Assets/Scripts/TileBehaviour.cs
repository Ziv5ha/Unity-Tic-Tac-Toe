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
        // The view doesn't need to know whether a tile is available or not. It simply says "Player clicked on tile X". The GameLogic should know whether this is legal or not, change the board model, and send this down to the view to handle the board itself.
        // For example:
        /*
        GameLogic:
        private void Start() {
            CreateTiles();
            AddListenersToTileEvents();
        }

        private void AddListenersToTileEvents() {
            foreach(TileBehaviour tile in tiles) {
                tile.ETilePressed += TilePressedHandler;
            }
        }

        private void TilePressedHandler(int tileIndex) {
            if(mapArray[GetMapArrayTileByTileIndex(tileIndex) == ""] {
                HandleTileClicked(tileIndex);
            } else {
                Debug.Log("Sorry, this tile has already been pressed");
            }
        }

        TileBehaviour:
        public Action<int> ETilePressed;
        private void OnMouseDown() {
            ETilePressed(index);
        }
         */
        // This keeps the view (TileBehaviour) much more simple and "stupid". Not needing to store any states (clicked) or state handling controllers (GameLogic)
        // We try to keep the views as simple as possible

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
        // See the scroll I wrote earlier.
        // A better approach would be to have the controller that is responsible for this view call all the view changed functions (only rotate in this case)
        if (gameLogic.AvailableTile(clicked))
        {
            RotateTile();
            // See comment in EventsManager.cs
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
