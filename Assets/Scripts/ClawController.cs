using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawController : MonoBehaviour
{
    private bool move;
    public Collider2D colliderofthis;
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && colliderofthis.bounds.Contains(touch.position))
            {
                move = true;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                move = false;
                transform.localPosition = Vector3.zero;
            }
            if (move)
            {
                transform.position = touch.position;
            }
        }     
        else
        {
            move = false;
            transform.localPosition = Vector3.zero;
        }

        //if (Input.GetMouseButtonDown(0) && colliderofthis.bounds.Contains(Input.mousePosition))
        //{
        //    move = true;            
        //}
        //else if(Input.GetMouseButtonUp(0))
        //{
        //    move = false;
        //    transform.localPosition = Vector3.zero;
        //}

    }

}

