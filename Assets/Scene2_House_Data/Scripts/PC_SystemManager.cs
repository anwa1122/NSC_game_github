using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class PC_SystemManager : MonoBehaviour
{
    [Header("Apps Windows")]
    public GameObject shopyWindow; // หน้าต่างจ่ายหนี้
    public GameObject jobWindow;   // หน้าต่างเลือกงาน (ที่เราทำไว้ก่อนหน้านี้)
    [Header("Zoom setting")]
    public float zoomSpeed = 3f;

    void Awake()
    {
        // เริ่มมาให้หน้าต่างแอปปิดอยู่เสมอ
        shopyWindow.SetActive(false);
        jobWindow.SetActive(false);
    }

    public void startJobWindow()
    {
        jobWindow.SetActive(true);
        if (shopyWindow != null) shopyWindow.SetActive(false);   // ปิดหน้าต่างช้อปปิ้ง (กันมันบังกัน)
        Debug.Log("Job Window Opened");
    }

    public void startShopybWindow()
    {
        shopyWindow.SetActive(true);
        if (shopyWindow != null) jobWindow.SetActive(false);     // ปิดหน้าต่างงาน
        Debug.Log("Shopy Window Opened");
    }

    IEnumerator ZoomInCanvas(GameObject gameObject)
    {
        gameObject.SetActive(true);
        gameObject.transform.localScale = Vector3.zero; // เริ่มจาก 0

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * zoomSpeed;
            // ใช้ SmoothStep เพื่อให้การซูมดูนุ่มนวลขึ้น (ช้าช่วงปลาย)
            float smoothT = Mathf.SmoothStep(0, 1, t);
            gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, smoothT);
            yield return null;
        }
        gameObject.transform.localScale = Vector3.one;
    }
}