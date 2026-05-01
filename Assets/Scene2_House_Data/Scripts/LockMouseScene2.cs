using UnityEngine;
using System.Collections;

public class LockMouseScene2 : MonoBehaviour
{
    void Start()
    {
        // สั่งให้เริ่มทำงานหลังจากรอ 0.1 วินาที
        StartCoroutine(LockCursorRoutine());
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