using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform centerObject;
    public float horizontalRotationSpeed;
    public float verticalRotationSpeed;
    public Vector3 initialVectorFromCenter;
    public float maxVerticalOffset;

    public static Vector2Int localForward = Vector2Int.up;
    public static Vector2Int localRight = Vector2Int.right;

    private Vector3 currentPosition = Vector3.zero;
    private float currentYPositionOffset = 0;
    private int targetHorizontalRotation = 0;

    private void Awake()
    {
        currentPosition = centerObject.position + initialVectorFromCenter;
        ApplyPositionChanges();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
            StartCoroutine( RotateHorizontally(true) );

        if(Input.GetKeyDown(KeyCode.A))
            StartCoroutine( RotateHorizontally(false) );

        if(Input.GetKey(KeyCode.W))
            RotateVertically(true);
            
        if(Input.GetKey(KeyCode.S))
            RotateVertically(false);
    }

    void RotateVertically(bool up)
    {
        currentYPositionOffset += (up ? 1 : -1) * Time.deltaTime * verticalRotationSpeed;

        if(up)
        {
            if(currentYPositionOffset > maxVerticalOffset)
                currentYPositionOffset = maxVerticalOffset;
        }
        else
        {
            if(currentYPositionOffset < -maxVerticalOffset)
                currentYPositionOffset = -maxVerticalOffset;
        }

        ApplyPositionChanges();
    }

    IEnumerator RotateHorizontally(bool right)
    {
        float newRotation = targetHorizontalRotation;
        targetHorizontalRotation += right ? -90 : 90;

        while(true)
        {
            newRotation += (right ? -1 : 1) * horizontalRotationSpeed * Time.deltaTime;

            if(right ? (newRotation <= targetHorizontalRotation) : (newRotation >= targetHorizontalRotation))
                break;

            SetYRotation(newRotation);
            yield return null;
        }

        targetHorizontalRotation = targetHorizontalRotation % 360;

        Quaternion rotation = Quaternion.Euler(0, 0, -targetHorizontalRotation);
        localForward = GridController.RotateRoundToInt(rotation, Vector2Int.up);
        localRight = GridController.RotateRoundToInt(rotation, Vector2Int.right);

        SetYRotation(targetHorizontalRotation);
    }

    private void SetYRotation(float rotation)
    {
        currentPosition = Quaternion.Euler(0, rotation, 0) * initialVectorFromCenter + centerObject.position;
        ApplyPositionChanges();
    }

    private void ApplyPositionChanges()
    {
        transform.position = currentPosition + Vector3.up * currentYPositionOffset;
        transform.LookAt(centerObject);
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(centerObject.position, centerObject.position + initialVectorFromCenter);
        Gizmos.DrawSphere(centerObject.position + initialVectorFromCenter, 0.1f);
    }
    #endif
}
