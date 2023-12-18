using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    [HideInInspector] public Animator animator;

    private void Start()
    {
        animator = transform.GetComponent<Animator>();
    }

}
