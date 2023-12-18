using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
   private Image thisImage;
    private float neededSize;
    private float startSize;

    void Start()
    {
        thisImage = transform.GetComponent<Image>();
        startSize = transform.localScale.x;
        neededSize = startSize;
    }
    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(neededSize, neededSize, neededSize), 10 * Time.deltaTime);
    }
    public void OnEnter()
    {
        neededSize = startSize + 0.1f;
    }
    public void OnExit()
    {
        neededSize = startSize;
    }

}
