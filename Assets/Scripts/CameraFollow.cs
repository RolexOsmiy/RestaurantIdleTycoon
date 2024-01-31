using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    
    private Transform myTransform;

    private void Awake()
    {
        myTransform = transform;
    }

    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            Vector3 desiredPosition = new Vector3(playerTransform.position.x + offset.x, myTransform.position.y, playerTransform.position.z + offset.z);
            Vector3 smoothedPosition = Vector3.Lerp(myTransform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}