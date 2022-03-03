using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform centerObject;
    public float rotationSpeed;

    public static Vector2Int localForward = Vector2Int.up;
    public static Vector2Int localRight = Vector2Int.right;

    private bool isRotating = false;
    private Vector3 initialVectorFromCenter;
    private int targetRotation = 0;
    private float currRotation = 0;
    private bool isRotationRight;

    private void Awake()
    {
        initialVectorFromCenter = transform.position - centerObject.position;
        transform.LookAt(centerObject);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
            StartRotation(false);
        
        if(Input.GetKeyDown(KeyCode.M))
            StartRotation(true);

        if(isRotating)
            Rotate();
    }

    private void Rotate()
    {
        currRotation += (isRotationRight ? -1 : 1) * rotationSpeed * Time.deltaTime;

        if(isRotationRight ? (currRotation <= targetRotation) : (currRotation >= targetRotation))
            EndRotation();

        transform.position = Quaternion.Euler(0, currRotation, 0) * initialVectorFromCenter + centerObject.position;
        transform.LookAt(centerObject);
        
    }

    private void EndRotation()
    {
        targetRotation = targetRotation % 360;
        currRotation = targetRotation;
        isRotating = false;

        Quaternion rotation = Quaternion.Euler(0, 0, -targetRotation);
        localForward = GridController.RotateRoundToInt(rotation, Vector2Int.up);
        localRight = GridController.RotateRoundToInt(rotation, Vector2Int.right);
    }

    private void StartRotation(bool right)
    {
        isRotating = true;
        isRotationRight = right;
        targetRotation += right ? -90 : 90;
    }
}
