using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    private float axisX;
    private float axisZ;
    [HideInInspector] public Transform target;
    public float speed;
    private Vector3 targetPosition;
    public LineRenderer line;

    private void Start()
    {
        axisX = transform.position.x;
           targetPosition = new Vector3(axisX, transform.position.y, transform.position.z);
    }

    private void Update()
    {
        if (!line.gameObject.activeSelf)
        {
            targetPosition = new Vector3(axisX, transform.position.y, target.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}
