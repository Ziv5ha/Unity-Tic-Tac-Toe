using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITileBehaviour : MonoBehaviour
{
    private Animator anim;
    // !@! speeeeeeeeeeeeeeeeeen
    [SerializeField] private float SpinSpeed = 1;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void RotateUITile(bool xTurn)
    {
        float singleStep = SpinSpeed * Time.deltaTime;
        if (xTurn)
        {
            anim.SetTrigger("playedX");
        }
        else
        {
            anim.SetTrigger("playedO");
        }
    }
}
