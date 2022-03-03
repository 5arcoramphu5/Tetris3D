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
    private Vector3 initialVectorToCenter;
    private int targetRotation = 0;
    private float currRotation = 0;
    private bool isRotationRight;

    private void Awake()
    {
        initialVectorToCenter = centerObject.position - transform.position;
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

        transform.position = Quaternion.Euler(0, currRotation, 0) * initialVectorToCenter + centerObject.position;
        transform.LookAt(centerObject);
    }

    private void EndRotation()
    {
        currRotation = targetRotation;
        isRotating = false;

        Quaternion rotation = Quaternion.Euler(0, 0, targetRotation);
        Vector2 rotForw = rotation * Vector2.up;
        Vector2 rotRight = rotation * Vector2.right;
        //todo
        localForward = new Vector2Int(Mathf.RoundToInt(rotForw.x), Mathf.RoundToInt(rotForw.y));
        localRight = new Vector2Int(Mathf.RoundToInt(rotRight.x), Mathf.RoundToInt(rotRight.y));
        Debug.DrawLine(transform.position, transform.position + new Vector3(localRight.x, 0, localRight.y) * 10, Color.yellow, 1f);
        Debug.DrawLine(transform.position, transform.position + new Vector3(localForward.x, 0, localForward.y) * 10, Color.yellow, 1f);
    }

    private void StartRotation(bool right)
    {
        isRotating = true;
        isRotationRight = right;
        targetRotation += right ? -90 : 90;
        targetRotation = targetRotation % 360;
    }
}
