using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLineScript : MonoBehaviour
{
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
