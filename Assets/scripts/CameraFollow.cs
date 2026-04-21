using UnityEngine;
using System;

public class CameraFollow : MonoBehaviour
{

    public Transform target; // The player's transform
    public Transform EndPlayerCamPos;
    public Transform FinishLineCamPosition;

    public float smoothSpeed = 0.125f; // The smoothness of the camera follow
    public float smoothSpeedAfterGameStart = 0.125f; // The smoothness of the camera follow
    public bool isRotation = true;
    private void LateUpdate()
    {
        if (target == null || GameManager.Instance.UI.IsPlayerControlsEnable == false)
            return;

        // Calculate the desired position for the camera
        Vector3 desiredPosition = target.position;

        //// Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // Calculate the desired rotation for the camera

        //// Set the camera's position to the smoothed position
        transform.position = smoothedPosition;
        if (isRotation)
        {
            Invoke(nameof(DisableRotation), .1f);
            transform.rotation = target.rotation;
        }

    }
    void DisableRotation()
    {
        smoothSpeed = 2;
        isRotation = false;
    }
}


