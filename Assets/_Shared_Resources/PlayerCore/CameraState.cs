using UnityEngine;

public enum CameraMode { MainCamera , POVCamera , FreezeCamera , UnFreezeCamera}
public class CameraState : MonoBehaviour
{

    public static CameraState Instance;
    public PlayerCamera playerCamera;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }
    public void ChangeCameraState(CameraMode cameraState)
    {
        if (cameraState == CameraMode.MainCamera)
        {
            playerCamera.isPOVMode = false;
        }
        else if (cameraState == CameraMode.POVCamera)
        {
            playerCamera.isPOVMode = true;
        }
        else if (cameraState == CameraMode.FreezeCamera)
        {
            playerCamera.canRotate = false;
        }
        else if (cameraState ==  CameraMode.UnFreezeCamera)
        {
            playerCamera.canRotate = true;
        }
    }
}
