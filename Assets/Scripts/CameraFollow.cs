using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerLocation;
    public Vector3 offsetCam;

    void LateUpdate()
    {
        offsetCam = new Vector3 (offsetCam.x, offsetCam.y, -10);
        transform.position = playerLocation.transform.position + offsetCam;
    }
}

