using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // target obj transform

    [SerializeField] private Vector3 offset; // offset from player graphic obj
    [SerializeField] private float smoothSpeed = .25f; // lerp utility
    private float startYPosition;

    private void Start()
    {
        startYPosition = transform.position.y;
    }
    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        desiredPosition.y = startYPosition;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * GameManager.instance.AccelerationMultiplier);
        transform.position = smoothedPosition;
    }
}
