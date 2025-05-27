using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private Vector3 offset;

    float offsetX;

    [SerializeField] private float maximumHeight = 0;
    [SerializeField] private float minimumHeight = 0;

    private void LateUpdate()
    {
        if (player != null)
        {
            CameraMovement();
        }
    }

    void CameraMovement()
    {
        Vector3 desiredPos = player.position + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        transform.position = smoothedPos;

        if (transform.position.y < minimumHeight)
        {
            transform.position = new Vector3(transform.position.x, minimumHeight, transform.position.z);
        }

        if (transform.position.y > maximumHeight)
        {
            transform.position = new Vector3(transform.position.x, maximumHeight, transform.position.z);
        }
    }
}