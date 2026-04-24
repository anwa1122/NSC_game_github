using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;
using System; // ต้องมีอันนี้เพื่อใช้ List


public class PC_SystemManager : MonoBehaviour
{
    // เพิ่มแค่บรรทัดนี้เพื่อทำ Instance
    public static PC_SystemManager Instance;   //PAY System |||| FreelanceHubManager

    [Header("Apps Windows")]
    public GameObject payWindow; // หน้าต่างจ่ายหนี้
    public GameObject freelanceWindow;   // หน้าต่างเลือกงาน (ที่เราทำไว้ก่อนหน้านี้)
    public GameObject menuPanel;


    public List<GameObject> allWindows = new List<GameObject>();
    public GameObject currentWindow;

    [Header("Ui setting")]
    public float zoomSpeed = 3f;
    public float showSpeed = 3f;
    public Vector2 hidePosition;
    public Vector2 showPosition;
    private RectTransform rect;
    private bool isShow;

    public List<GameObject> windowLists;
    private GameObject firstWindow;
    void Awake()
    {
        // เพิ่มส่วนนี้เพื่อให้ Instance ใช้งานได้
        if (Instance == null) Instance = this;

        // เริ่มมาให้หน้าต่างแอปปิดอยู่เสมอ
        payWindow.SetActive(false);
        freelanceWindow.SetActive(false);

        currentWindow = menuPanel;
        windowLists.Add(currentWindow);
    }
    void Update()
    {
        if (isShow)
        {
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, showPosition, Time.deltaTime * showSpeed);

            if (rect.anchoredPosition.x == showPosition.x + 50f)
            {
                isShow = false;
            }
        }

        
    }

    public void startFreeLanceWindow()
    {
        EnterWindow(freelanceWindow);
        //freelanceWindow.SetActive(true);
        ///if (payWindow != null) payWindow.SetActive(false);   // ปิดหน้าต่างช้อปปิ้ง (กันมันบังกัน)
        //if (menuPanel != null) menuPanel.SetActive(false);
    }

    public void startPaybWindow()
    {
        EnterWindow(payWindow);
        //payWindow.SetActive(true);
        //if (freelanceWindow != null) freelanceWindow.SetActive(false);     // ปิดหน้าต่างงาน
        //if (menuPanel != null) menuPanel.SetActive(false);
    }

    public void EnterWindow(GameObject targetWindow)
    {
        
        foreach (GameObject window in allWindows)
        {
            if (window != targetWindow)
            {
                window.SetActive(false);
            }
            else
            {               
                rect =   window.GetComponent<RectTransform>();
                showPosition = rect.anchoredPosition;
                rect.anchoredPosition = hidePosition;

                isShow = true;
                //window.SetActive(true);
                currentWindow = targetWindow;
                windowLists.Add(currentWindow);

                firstWindow = windowLists[windowLists.Count - 1];
                Debug.Log(firstWindow);
                firstWindow.SetActive(true);
            }
        }
    }

    public void ExitWindow(GameObject targetWindow)
    {
        // แก้ไขให้เริ่ม Coroutine ซูมออกแทนการปิดทันที
        StartCoroutine(ZoomOutCanvas(targetWindow));
        windowLists.RemoveAt(windowLists.Count - 1);

        firstWindow = windowLists[windowLists.Count - 1];
        Debug.Log(firstWindow);
        firstWindow.SetActive(true);
    }

    public void CloseAllWindows()
    {
        foreach (GameObject window in allWindows)
        {
            if (window != null) window.SetActive(false);
        }
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

    // เพิ่มฟังก์ชันซูมออกตามที่ต้องการ
    IEnumerator ZoomOutCanvas(GameObject gameObject)
    {
        if (menuPanel != null) menuPanel.SetActive(true);
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * zoomSpeed;
            float smoothT = Mathf.SmoothStep(0, 1, t);
            // จาก 1 กลับไป 0
            gameObject.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, smoothT);
            yield return null;
        }

        // --- ส่วนที่แก้ไข ---
        gameObject.SetActive(false);             // ปิดหน้าต่างไปก่อน
        gameObject.transform.localScale = Vector3.one; // รีเซ็ตขนาดกลับมาเป็น 1 ทันที (เตรียมพร้อมสำหรับตอนเปิดครั้งหน้า)
        
        
    }
}