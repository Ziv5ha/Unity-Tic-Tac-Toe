using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITileBehaviour : MonoBehaviour
{
    // I would add the animator component also in the scene. We already have another [SerializeField] here it's not like we are instantiating this GameObject or that we need this reference to be set dynamically.
    private Animator anim;
    [SerializeField] private float SpinSpeed = 1;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void RotateUITile(bool xTurn)
    {
        // this row does nothing
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
