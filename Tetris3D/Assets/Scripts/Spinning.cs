using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning : MonoBehaviour
{
    public float speed;
    private RectTransform rT;
    private static float currentRotation = 0;

    private void Awake()
    {
        rT = GetComponent<RectTransform>();
    }

    private void Update()
    {
        currentRotation += speed*Time.deltaTime;
        currentRotation %= 360;
        rT.localRotation = Quaternion.Euler(0, currentRotation, 0);
    }
}
