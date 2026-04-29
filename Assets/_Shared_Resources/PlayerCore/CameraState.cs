using UnityEngine;

// เพิ่มสถานะใหม่สำหรับใช้งานในบ้าน
public enum CameraMode { MainCamera, POVCamera, FreezeCamera, UnFreezeCamera, PCMode, ExitPCMode }

public class CameraState : MonoBehaviour
{
    public static CameraState Instance;
    public PlayerCamera playerCamera;

    void Awake()
    {
        if (Instance == null) Instance = this;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void ChangeCameraState(CameraMode cameraState)
    {
        if (cameraState == CameraMode.MainCamera)
        {
            playerCamera.isPOVMode = false;
            playerCamera.isSubtleMouseMode = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (cameraState == CameraMode.POVCamera)
        {
            playerCamera.isPOVMode = true;
            playerCamera.isSubtleMouseMode = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (cameraState == CameraMode.FreezeCamera)
        {
            playerCamera.canRotate = false;
        }
        else if (cameraState == CameraMode.UnFreezeCamera)
        {
            playerCamera.canRotate = true;
        }
        // --- ส่วนที่เพิ่มใหม่สำหรับ Scene 2 ---
        else if (cameraState == CameraMode.PCMode)
        {
            playerCamera.isPOVMode = true;
            playerCamera.isSubtleMouseMode = true; // เปิดโหมดกล้องขยับตามเมาส์
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (cameraState == CameraMode.ExitPCMode)
        {
            playerCamera.isSubtleMouseMode = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void ChangeCameraPosition(Transform target)
    {
        playerCamera.LockCameraPosition(target); // target = null คือ unlock
    }
}