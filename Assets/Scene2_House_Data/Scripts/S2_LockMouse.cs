using UnityEngine;
using System.Collections;

public class S2_LockMouse : MonoBehaviour
{
    public bool lockMouse = true;
    void Start()
    {
        if (lockMouse)
        {
            StartCoroutine(LockCursorRoutine());
        }
    }

    IEnumerator LockCursorRoutine()
    {
        // รอให้ทุกอย่างใน Scene มัน Setup ตัวเองเสร็จก่อน
        yield return new WaitForSeconds(0.1f);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Mouse Locked by Routine");
    }
}