using UnityEngine;

public enum CameraMode { MainCamera , POVCamera }
public class CameraState : MonoBehaviour
{

    public ThirdPersonCamera playerCamera;

    public void ChangeCameraState(string cameraState)
    {
        if (cameraState == "MainCam")
        {
            playerCamera.isPOVMode = false;
        }
        else if (cameraState == "PovCam")
        {
            playerCamera.isPOVMode = true;
        }
    }
}
