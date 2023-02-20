using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLineScript : MonoBehaviour
{
    private GameLogic gameLogic;
    public int placement;

    private void Start() {
        gameLogic = GameObject.FindGameObjectWithTag("Map").GetComponent<GameLogic>();
    }
    // public void PlaceHorizontaly(){
    //     int row = placement % gameLogic.mapSize;
    //     int column = placement / gameLogic.mapSize;
    //     transform.position = new Vector3 ()
    // }
    public void isVertical(){
        transform.Rotate(Vector3.forward, -90);
    }
    public void isDiagonal(){
        transform.Rotate(Vector3.forward, 45);
    }    
    public void isOtherDiagonal(){
        transform.Rotate(Vector3.forward, -45);
    }
}
