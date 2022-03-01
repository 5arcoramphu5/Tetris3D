using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform centerObject;
    public float speed;
    private Vector3 initialVectorToCenter;
    private int currRotation = 0;

    private void Awake()
    {
        initialVectorToCenter = centerObject.position - transform.position;
        transform.LookAt(centerObject);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
            Rotate(false);
        
        if(Input.GetKeyDown(KeyCode.Mouse1))
            Rotate(true);
    }

    private void Rotate(bool right)
    {
        currRotation += right ? 90 : -90;
        currRotation = currRotation % 360;
        transform.position = Quaternion.Euler(0, currRotation, 0) * initialVectorToCenter + centerObject.position;
        transform.LookAt(centerObject);
    }
}
