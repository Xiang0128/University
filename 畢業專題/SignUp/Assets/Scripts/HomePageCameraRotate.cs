using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePageCameraRotate : MonoBehaviour
{
    // Set the center point to rotate around
    public Vector3 center = new Vector3(5, 0, -3);

    // Rotation speed
    public float rotationSpeed = 100f;

    void Update()
    {
        // Rotate the camera around the center point along the Y-axis
        transform.RotateAround(center, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
