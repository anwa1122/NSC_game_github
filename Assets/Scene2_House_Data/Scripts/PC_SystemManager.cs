using UnityEngine;

public class PC_SystemManager : MonoBehaviour
{
    [Header("Apps Windows")]
    public GameObject shopyWindow; // หน้าต่างจ่ายหนี้
    public GameObject jobWindow;   // หน้าต่างเลือกงาน (ที่เราทำไว้ก่อนหน้านี้)

    void Awake()
    {
        // เริ่มมาให้หน้าต่างแอปปิดอยู่เสมอ
        shopyWindow.SetActive(false);
        jobWindow.SetActive(false);
    }

    public void startJobWindow()
    {
        jobWindow.SetActive(true);      // เปิดหน้าต่างงาน
        if (shopyWindow != null) shopyWindow.SetActive(false);   // ปิดหน้าต่างช้อปปิ้ง (กันมันบังกัน)
        Debug.Log("Job Window Opened");
    }

    public void startShopybWindow()
    {
        shopyWindow.SetActive(true);    // เปิดหน้าต่างช้อปปิ้ง
        if (shopyWindow != null) jobWindow.SetActive(false);     // ปิดหน้าต่างงาน
        Debug.Log("Shopy Window Opened");
    }
}